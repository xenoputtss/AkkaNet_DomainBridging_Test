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

                var isError = serializedObject.Contains("broken");
                var output = isError ? Console.Error : Console.Out;

                output.WriteLine(msg.GetType().Name);
                output.WriteLine(serializedObject);

                if (isError)
                {
                    throw new Exception("Everything's ruined!");
                }
            });
        }
    }
}