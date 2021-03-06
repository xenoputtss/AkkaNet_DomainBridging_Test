﻿using Akka.Actor;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class ConsumerActor : ReceiveActor
    {
        private readonly ActorSelection _consoleWriter;
        public ConsumerActor(ActorSelection consoleWriter)
        {
            _consoleWriter = consoleWriter;
            Receive<ConsumerDomain.Commands.CreateConsumer>(m =>
            {
                var e = new ConsumerDomain.Events.ConsumerCreated(userName: m.UserName, pin: m.Pin);
                _consoleWriter.Tell(e);
            });

            Receive<ConsumerDomain.Commands.AddFirstName>(m =>
            {
                var e = new ConsumerDomain.Events.FirstNameSet(firstName: m.FirstName);
                _consoleWriter.Tell(e);
            });

            Receive<ConsumerDomain.Commands.AddLastName>(m =>
            {
                var e = new ConsumerDomain.Events.LastNameSet(lastName: m.LastName);
                _consoleWriter.Tell(e);
            });
        }
    }
}
