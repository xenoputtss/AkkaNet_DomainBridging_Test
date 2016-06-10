﻿using System;
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
                Console.WriteLine(msg.GetType().Name);
                System.Threading.Thread.Sleep(250);
                Console.WriteLine(_tf.FormatObjects(new[] { msg }));
            });
        }
    }
}