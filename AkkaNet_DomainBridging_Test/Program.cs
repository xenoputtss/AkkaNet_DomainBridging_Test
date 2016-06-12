using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Akka.Actor;
using Akka.Routing;
using AkkaNet_DomainBridging_Test.Actors;

namespace AkkaNet_DomainBridging_Test
{
    class Program
    {
        public static ActorSystem ActorSystem;


        static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("DomainBridge");

            var cw = ActorSystem.ActorOf(Props.Create(() => new ConsoleWriter()),"consoleWriter");
            //var c = ActorSystem.ActorOf(Props.Create(() => new ConsumerActor(cw)));

            var distro = ActorSystem.ActorOf(Props.Create(() => new DistributionActor()));
            //var router = ActorSystem.ActorOf(Props.Create(() => new TranslatorActor(c)).WithRouter(
            //    new ConsistentHashingPool(2).WithHashMapping(o =>
            //    {
            //        if (o is IAmAnAggregateMessage)
            //        {
            //            Console.WriteLine(((IAmAnAggregateMessage)o).AggregateId);
            //            return ((IAmAnAggregateMessage)o).AggregateId;
            //        }
            //        throw new Exception("Hashing Failure");
            //    })), "MasterTranslatorRouter");


            PlayLegacyMessages1(actor: distro);

            //PlayLegacyMessages2(actor: distro);

            //PlayLegacyMessages3(actor: distro);

            //PlayLegacyMessages4(actor: distro);

            //PlayLegacyMessages5(actor: distro);


            //_myActor = ActorSystem.ActorOf<TranslatorActor>("Actor1");
            //PlayLegacyMessages1a(actor: distro);
            Console.ReadKey();
        }



        public static void PlayLegacyMessages1(IActorRef actor)
        {
            //UserName1
            //Pin1
            //FirstName1
            //LastName1
            var userName = new LegacyDomain.Events.UserNameAdded("case1", "TestUser1");
            var pin = new LegacyDomain.Events.PinAdded("abcd1", "TestUser1");
            var firstName = new LegacyDomain.Events.FirstNameAdded("case", "TestUser1");
            var lastName = new LegacyDomain.Events.LastNameAdded("one", "TestUser1");

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
            var userName = new LegacyDomain.Events.UserNameAdded("case2", "TestUser1");
            var pin = new LegacyDomain.Events.PinAdded("abcd2", "TestUser1");
            var firstName = new LegacyDomain.Events.FirstNameAdded("case", "TestUser1");
            var lastName = new LegacyDomain.Events.LastNameAdded("two", "TestUser1");


            actor.Tell(firstName);
            actor.Tell(lastName);
            actor.Tell(userName);
            actor.Tell(pin);

            //Result, 3 messages
            // 1 Created
            // 1 FirstNameAdded
            // 1 LastNameAdded
        }

        public static void PlayLegacyMessages3(IActorRef actor)
        {
            //Pin1
            //LastName1
            //FirstName1
            //UserName1
            var userName = new LegacyDomain.Events.UserNameAdded("case3", "TestUser1");
            var pin = new LegacyDomain.Events.PinAdded("abcd3", "TestUser1");
            var firstName = new LegacyDomain.Events.FirstNameAdded("case", "TestUser1");
            var lastName = new LegacyDomain.Events.LastNameAdded("three", "TestUser1");

            actor.Tell(pin);
            actor.Tell(firstName);
            actor.Tell(lastName);
            actor.Tell(userName);

            //Result, 3 messages
            // 1 Created
            // 1 LastNameAdded
            // 1 FirstNameAdded
        }

        public static void PlayLegacyMessages4(IActorRef actor)
        {
            var userName1 = new LegacyDomain.Events.UserNameAdded("User1", "TestUser1");
            var pin1 = new LegacyDomain.Events.PinAdded("abcd1", "TestUser1");
            var firstName1 = new LegacyDomain.Events.FirstNameAdded("user1", "TestUser1");
            var lastName1 = new LegacyDomain.Events.LastNameAdded("one", "TestUser1");

            var userName2 = new LegacyDomain.Events.UserNameAdded("User2", "TestUser2");
            var pin2 = new LegacyDomain.Events.PinAdded("abcd2", "TestUser2");
            var firstName2 = new LegacyDomain.Events.FirstNameAdded("user2", "TestUser2");
            var lastName2 = new LegacyDomain.Events.LastNameAdded("two", "TestUser2");

            var userName3 = new LegacyDomain.Events.UserNameAdded("User3", "TestUser3");
            var pin3 = new LegacyDomain.Events.PinAdded("abcd3", "TestUser3");
            var firstName3 = new LegacyDomain.Events.FirstNameAdded("user3", "TestUser3");
            var lastName3 = new LegacyDomain.Events.LastNameAdded("three", "TestUser3");


            //actor.Tell(pin2);
            //actor.Tell(pin1);
            //actor.Tell(userName2);
            //actor.Tell(userName1);

            ///*
            //Pin1
            //LastName1
            //FirstName1
            actor.Tell(pin1);
            actor.Tell(lastName1);
            actor.Tell(firstName1);

            //UserName2
            //Pin2
            //FirstName2
            //LastName2
            actor.Tell(userName2);
            actor.Tell(pin2);
            actor.Tell(firstName2);
            actor.Tell(lastName2);

            //UserName3
            //Pin3
            //FirstName3
            //LastName3
            actor.Tell(userName3);
            actor.Tell(pin3);
            actor.Tell(firstName3);
            actor.Tell(lastName3);

            //UserName1
            actor.Tell(userName1);

            //Result
            // Created user 2 (3 messages)
            // Created user 3 (3 messages)
            // Created user 1 (3 messages)
            //*/
        }

        public static void PlayLegacyMessages5(IActorRef actor)
        {
            var userName1 = new LegacyDomain.Events.UserNameAdded("User1", "TestUser1");
            var pin1 = new LegacyDomain.Events.PinAdded("abcd1", "TestUser1");
            var firstName1 = new LegacyDomain.Events.FirstNameAdded("user1", "TestUser1");
            var lastName1 = new LegacyDomain.Events.LastNameAdded("one", "TestUser1");

            var userName2 = new LegacyDomain.Events.UserNameAdded("User2", "TestUser2");
            var pin2 = new LegacyDomain.Events.PinAdded("broken pin", "TestUser2");
            var firstName2 = new LegacyDomain.Events.FirstNameAdded("user2", "TestUser2");
            var lastName2 = new LegacyDomain.Events.LastNameAdded("two", "TestUser2");

            var userName3 = new LegacyDomain.Events.UserNameAdded("User3", "TestUser3");
            var pin3 = new LegacyDomain.Events.PinAdded("abcd3", "TestUser3");
            var firstName3 = new LegacyDomain.Events.FirstNameAdded("user3", "TestUser3");
            var lastName3 = new LegacyDomain.Events.LastNameAdded("three", "TestUser3");


            //actor.Tell(pin2);
            //actor.Tell(pin1);
            //actor.Tell(userName2);
            //actor.Tell(userName1);

            ///*
            //Pin1
            //LastName1
            //FirstName1
            actor.Tell(pin1);
            actor.Tell(lastName1);
            actor.Tell(firstName1);

            //UserName2
            //Pin2
            //FirstName2
            //LastName2
            actor.Tell(userName2);
            actor.Tell(pin2);
            actor.Tell(firstName2);
            actor.Tell(lastName2);

            //UserName3
            //Pin3
            //FirstName3
            //LastName3
            actor.Tell(userName3);
            actor.Tell(pin3);
            actor.Tell(firstName3);
            actor.Tell(lastName3);

            //UserName1
            actor.Tell(userName1);

            //Result
            // Created user 2 (3 messages)
            // Created user 3 (3 messages)
            // Created user 1 (3 messages)
            //*/
        }
    }

}
