using Akka.Actor;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class ConsumerActor : ReceiveActor
    {
        private readonly IActorRef _consoleWriter;
        public ConsumerActor(IActorRef consoleWriter)
        {
            _consoleWriter = consoleWriter;
            Receive<ConsumerDomain.Commands.CreateConsumer>(m =>
            {
                var e = new ConsumerDomain.Events.ConsumerCreated(userName: m.UserName, pin: m.Pin);
                System.Threading.Thread.Sleep(250);
                _consoleWriter.Tell(e);
            });

            Receive<ConsumerDomain.Commands.AddFirstName>(m =>
            {
                var e = new ConsumerDomain.Events.FirstNameSet(firstName: m.FirstName);
                System.Threading.Thread.Sleep(250);
                _consoleWriter.Tell(e);
            });

            Receive<ConsumerDomain.Commands.AddLastName>(m =>
            {
                var e = new ConsumerDomain.Events.LastNameSet(lastName: m.LastName);
                System.Threading.Thread.Sleep(250);
                _consoleWriter.Tell(e);
            });
        }
    }
}
