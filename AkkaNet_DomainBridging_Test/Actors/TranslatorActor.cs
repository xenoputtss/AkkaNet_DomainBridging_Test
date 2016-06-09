﻿using System;
using System.CodeDom;
using Akka.Actor;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private string UserName = null;
        private string Pin = null;
        private readonly IActorRef _bo;

        public TranslatorActor(IActorRef bo)
        {
            _bo = bo;
            Become(WaitingForMiniumumData);
        }

        private void WaitingForMiniumumData()
        {
            Receive<LegacyDomain.Events.UserNameAdded>(e =>
            {
                UserName = e.UserName;
                IsNowValidCheck();
            });
            Receive<LegacyDomain.Events.PinAdded>(e =>
            {
                Pin = e.Pin;
                IsNowValidCheck();
            });
            ReceiveAny(_ => Stash.Stash());
        }

        private void IsNowValidCheck()
        {
            if (WeAreValid)
            {
                var create = new ConsumerDomain.Commands.CreateConsumer(userName: UserName, pin: Pin);
                _bo.Tell(create);
                Become(Phase2);
                Stash.UnstashAll();
            }
        }

        private void Phase2()
        {
            //Receive<LegacyDomain.Events.UserNameAdded>(e => _consoleWriter.Tell(e.UserName));
            //Receive<LegacyDomain.Events.PinAdded>(e => _consoleWriter.Tell(e.Pin));
            Receive<LegacyDomain.Events.FirstNameAdded>(e =>
            {
                var m = new ConsumerDomain.Commands.AddFirstName(e.FirstName);
                _bo.Tell(m);
            });
            Receive<LegacyDomain.Events.LastNameAdded>(e =>
            {
                var m = new ConsumerDomain.Commands.AddLastName(e.LastName);
                _bo.Tell(m);
            });


        }
        private bool WeAreValid => UserName != null && Pin != null;
    }
}
