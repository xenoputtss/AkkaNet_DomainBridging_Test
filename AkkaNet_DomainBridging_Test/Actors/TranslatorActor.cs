using System;
using Akka.Actor;
using Akka.Persistence;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceivePersistentActor, IWithUnboundedStash
    {
        #region Base Class and Interface crap
        public IStash Stash { get; set; }
        public override string PersistenceId => AggregateId;
        #endregion

        private string AggregateId = null;  //Possbily the base of an "Aggregate Actor"

        private readonly IActorRef _destinationWriter;  //Translator needs to know where to write to

        #region Some sort of Object State
        private string UserName = null;
        private string Pin = null;
        private bool WeAreValid => UserName != null && Pin != null;
        #endregion

        public TranslatorActor(string aggregateId)
        {
            AggregateId = aggregateId;

            var consoleWriter = Context.ActorSelection("/user/consoleWriter");
            _destinationWriter = Context.ActorOf(Props.Create(() => new ConsumerActor(consoleWriter)));

            SetupRecovery();

            WaitingForMiniumumData();
        }

        private void SetupRecovery()
        {
            //Recover<>
        }

        /// <summary>
        /// Initial State of this actor, gather minimum data until and stash all other messages
        /// </summary>
        private void WaitingForMiniumumData()
        {
            Command<LegacyDomain.Events.UserNameAdded>(e =>
            {
                //Console.WriteLine(e.AggregateId + " " + e.UserName);
                CheckForAggregateConsistency(e);
                UserName = e.UserName;
                MinimumDataNeededForDesitnationDomainCheck();
            });
            Command<LegacyDomain.Events.PinAdded>(e =>
            {
                //Console.WriteLine(e.AggregateId +" "+ e.Pin);
                CheckForAggregateConsistency(e);
                Pin = e.Pin;
                MinimumDataNeededForDesitnationDomainCheck();
            });
            CommandAny(_ => Stash.Stash());
        }

        /// <summary>
        /// After minimum data has been received, this is where the actor will remain for ever.
        /// 
        /// It will only be shuttleing data at this point
        /// </summary>
        private void PostMinimumDataReceived()
        {
            Command<LegacyDomain.Events.FirstNameAdded>(e =>
            {
                CheckForAggregateConsistency(e);
                var m = new ConsumerDomain.Commands.AddFirstName(e.FirstName, aggregateId: AggregateId);
                _destinationWriter.Tell(m);
            });
            Command<LegacyDomain.Events.LastNameAdded>(e =>
            {
                CheckForAggregateConsistency(e);
                var m = new ConsumerDomain.Commands.AddLastName(e.LastName, aggregateId: AggregateId);
                _destinationWriter.Tell(m);
            });
        }

        /// <summary>
        /// This is just here for exploratory purposes, once we understand how to create an Aggregate Actor, this should be removed.
        /// </summary>
        private void CheckForAggregateConsistency(IAmAnAggregateMessage m)
        {
            if (AggregateId == null)
                AggregateId = m.AggregateId;
            if (AggregateId != m.AggregateId)
                throw new Exception($@"Aggregate Mismatched in {nameof(TranslatorActor)} with CurrentId='{AggregateId}' and Message AggregateId='{m.AggregateId}'");
        }

        private void MinimumDataNeededForDesitnationDomainCheck()
        {
            if (WeAreValid)
            {
                var create = new ConsumerDomain.Commands.CreateConsumer(userName: UserName, pin: Pin, aggregateId: AggregateId);
                _destinationWriter.Tell(create);
                Become(PostMinimumDataReceived);
                Stash.UnstashAll();
            }
        }



    }
}
