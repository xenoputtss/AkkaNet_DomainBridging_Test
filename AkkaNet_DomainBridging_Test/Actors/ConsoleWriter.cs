using System;
using Akka.Actor;
using Tababular;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class ConsoleWriter : ReceiveActor
    {
        private readonly TableFormatter _tf = new TableFormatter();
        public ConsoleWriter()
        {
            ReceiveAny(msg =>
            {
                System.Threading.Thread.Sleep(250);
                var serializedObject = _tf.FormatObjects(new[] { msg });
                Console.Out.WriteLine(msg.GetType().Name);
                Console.Out.WriteLine(serializedObject);

            });
        }
    }
}