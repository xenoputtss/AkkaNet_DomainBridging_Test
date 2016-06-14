using System;
using Akka.Actor;
using Akka.Persistence;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceivePersistentActor
    {
        #region Base Class and Interface crap
        public override string PersistenceId => AggregateId;
        #endregion

        private string AggregateId = null;  //Possbily the base of an "Aggregate Actor"
        private readonly IActorRef _destinationWriter;  //Translator needs to know where to write to

        private LegacyUserState state = new LegacyUserState();

        public TranslatorActor(string aggregateId)
        {
            AggregateId = aggregateId;

            var consoleWriter = Context.ActorSelection("/user/consoleWriter");
            _destinationWriter = Context.ActorOf(Props.Create(() => new ConsumerActor(consoleWriter)));

            SetupRecovery();

            Become(WaitingForMiniumumData);
        }

        private void SetupRecovery()
        {
            Recover<SnapshotOffer>(offer =>
            {
                var s = offer.Snapshot as LegacyUserState;
                if (s == null)
                    return;
                state = s;
                if (state.WeAreValid)
                    Become(PostMinimumDataReceived);
            });
            Recover<LegacyDomain.Events.UserNameAdded>(e => state.UserName = e.UserName);
            Recover<LegacyDomain.Events.PinAdded>(e => state.Pin = e.Pin);
        }

        /// <summary>
        /// Initial State of this actor, gather minimum data until and stash all other messages
        /// </summary>
        private void WaitingForMiniumumData()
        {
            Command<LegacyDomain.Events.UserNameAdded>(e =>
            {
                CheckForAggregateConsistency(e);
                Persist(e, a =>
                {
                    state.UserName = a.UserName;
                    MinimumDataNeededForDesitnationDomainCheck();
                });
            });
            Command<LegacyDomain.Events.PinAdded>(e =>
            {
                //Console.WriteLine(e.AggregateId +" "+ e.Pin);
                CheckForAggregateConsistency(e);
                if (e.Pin.Contains("broken"))
                {
                    throw new Exception("Everything's ruined!");
                }

                Persist(e, a =>
                {
                    state.Pin = a.Pin;
                    MinimumDataNeededForDesitnationDomainCheck();
                });
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
            Command<SaveSnapshotSuccess>(success =>
            {
                // soft-delete the journal up until the sequence # at
                // which the snapshot was taken
                DeleteMessages(success.Metadata.SequenceNr);
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

        public void MinimumDataNeededForDesitnationDomainCheck()
        {
            if (state.WeAreValid)
            {
                var create = new ConsumerDomain.Commands.CreateConsumer(userName: state.UserName, pin: state.Pin, aggregateId: AggregateId);
                _destinationWriter.Tell(create);

                Become(PostMinimumDataReceived);
                Stash.UnstashAll();

                SaveSnapshot(state);
            }
        }


        private class LegacyUserState
        {
            public string UserName = null;
            public string Pin = null;
            public bool WeAreValid => UserName != null && Pin != null;


        }
    }
}
