using System;
using Akka.Actor;
using Tababular;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class ConsoleWriter : ReceiveActor
    {
        private TableFormatter _tf = new TableFormatter();
        public ConsoleWriter()
        {
            ReceiveAny(msg =>
            {
                Console.WriteLine(_tf.FormatObjects(new[] { msg }));
            });
        }
    }
}