﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Internal;
using AkkaNet_DomainBridging_Test.Actors;

namespace AkkaNet_DomainBridging_Test
{
    class Program
    {
        public static ActorSystem ActorSystem;

        static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("DomainBridge");

            var cw = ActorSystem.ActorOf(Props.Create(() => new ConsoleWriter()));
            var c = ActorSystem.ActorOf(Props.Create(() => new ConsumerActor(cw)));

            var _myActor = ActorSystem.ActorOf(Props.Create(() => new TranslatorActor(c)));
            PlayLegacyMessages1(actor: _myActor);

            _myActor = ActorSystem.ActorOf(Props.Create(() => new TranslatorActor(c)));
            PlayLegacyMessages2(actor: _myActor);

            //_myActor = ActorSystem.ActorOf<TranslatorActor>("Actor1");
            //PlayLegacyMessages1a(actor: _myActor);
            Console.ReadKey();
        }



        public static void PlayLegacyMessages1(IActorRef actor)
        {
            //UserName1
            //Pin1
            //FirstName1
            //LastName1
            var userName = new LegacyDomain.Events.UserNameAdded("jpierson");
            var pin = new LegacyDomain.Events.PinAdded("abcd");
            var firstName = new LegacyDomain.Events.FirstNameAdded("Jeff");
            var lastName = new LegacyDomain.Events.LastNameAdded("Pierson");

            actor.Tell(userName);
            actor.Tell(pin);
            actor.Tell(firstName);
            actor.Tell(lastName);


            //Result, 3 messages
            // 1 Created
            // 1 FirstNameAdded
            // 1 LastNameAdded
        }
        public static void PlayLegacyMessages1a(IActorRef actor)
        {
            //UserName1
            //Pin1
            //FirstName1
            //LastName1
            var userName = new LegacyDomain.Events.UserNameAdded("jpierson1");
            var pin = new LegacyDomain.Events.PinAdded("abcd1");
            var firstName = new LegacyDomain.Events.FirstNameAdded("Jeff1");
            var lastName = new LegacyDomain.Events.LastNameAdded("Pierson1");

            actor.Tell(userName);
            actor.Tell(pin);
            actor.Tell(firstName);
            actor.Tell(lastName);


            //Result, 3 messages
            // 1 Created
            // 1 FirstNameAdded
            // 1 LastNameAdded
        }
        public static void PlayLegacyMessages2(IActorRef actor)
        {
            //FirstName1
            //LastName1
            //UserName1
            //Pin1
            var firstName = new LegacyDomain.Events.FirstNameAdded("Ryan");
            var lastName = new LegacyDomain.Events.LastNameAdded("Forster");
            var userName = new LegacyDomain.Events.UserNameAdded("rforster");
            var pin = new LegacyDomain.Events.PinAdded("1234");


            actor.Tell(firstName);
            actor.Tell(lastName);
            actor.Tell(userName);
            actor.Tell(pin);

            //Result, 3 messages
            // 1 Created
            // 1 FirstNameAdded
            // 1 LastNameAdded
        }

        public static void PlayLegacyMessages3()
        {
            //Pin1
            //LastName1
            //FirstName1
            //UserName1

            //Result, 3 messages
            // 1 Created
            // 1 LastNameAdded
            // 1 FirstNameAdded
        }


        public static void PlayLegacyMessages4()
        {
            //Pin1
            //LastName1
            //FirstName1

            //UserName2
            //Pin2
            //FirstName2
            //LastName2

            //UserName3
            //Pin3
            //FirstName3
            //LastName3

            //UserName1


            //Result
            // Created user 2 (3 messages)
            // Created user 3 (3 messages)
            // Created user 1 (3 messages)

        }

    }

}
