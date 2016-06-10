using System;
using Akka.Actor;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private string AggregateId = null;
        private string UserName = null;
        private string Pin = null;

        private readonly IActorRef _destinationWriter;

        public TranslatorActor()
        {
            //var consoleWriter = Context.ActorOf(Props.Create(() => new ConsoleWriter()));
            var consoleWriter = Context.ActorSelection("/user/consoleWriter");
            _destinationWriter = Context.ActorOf(Props.Create(() => new ConsumerActor(consoleWriter)));

            Become(WaitingForMiniumumData);
        }

        private void WaitingForMiniumumData()
        {
            Receive<LegacyDomain.Events.UserNameAdded>(e =>
            {
                //Console.WriteLine(e.AggregateId + " " + e.UserName);
                CheckForAggregateConsistency(e);
                UserName = e.UserName;
                IsNowValidCheck();
            });
            Receive<LegacyDomain.Events.PinAdded>(e =>
            {
                //Console.WriteLine(e.AggregateId +" "+ e.Pin);
                CheckForAggregateConsistency(e);
                Pin = e.Pin;
                IsNowValidCheck();
            });
            ReceiveAny(_ => Stash.Stash());
        }

        private void CheckForAggregateConsistency(IAmAnAggregateMessage m)
        {
            if (AggregateId == null)
                AggregateId = m.AggregateId;
            if (AggregateId != m.AggregateId)
                throw new Exception($@"Aggregate Mismatched in {nameof(TranslatorActor)} with CurrentId='{AggregateId}' and Message AggregateId='{m.AggregateId}'");
        }

        private void IsNowValidCheck()
        {
            if (WeAreValid)
            {
                var create = new ConsumerDomain.Commands.CreateConsumer(userName: UserName, pin: Pin, aggregateId: AggregateId);
                _destinationWriter.Tell(create);
                Become(Phase2);
                Stash.UnstashAll();
            }
        }

        private void Phase2()
        {
            Receive<LegacyDomain.Events.FirstNameAdded>(e =>
            {
                CheckForAggregateConsistency(e);
                var m = new ConsumerDomain.Commands.AddFirstName(e.FirstName, aggregateId: AggregateId);
                _destinationWriter.Tell(m);
            });
            Receive<LegacyDomain.Events.LastNameAdded>(e =>
            {
                CheckForAggregateConsistency(e);
                var m = new ConsumerDomain.Commands.AddLastName(e.LastName, aggregateId: AggregateId);
                _destinationWriter.Tell(m);
            });


        }
        private bool WeAreValid => UserName != null && Pin != null;
    }
}
