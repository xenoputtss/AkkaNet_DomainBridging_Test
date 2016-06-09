using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Pattern;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private string UserName = null;
        private string Pin = null;

        public TranslatorActor()
        {
            Become(WaitingForMiniumumData);
        }

        private void WaitingForMiniumumData()
        {
            Receive<LegacyDomain.Events.UserNameAdded>(e =>
            {
                UserName = e.UserName;
                if (WeAreValid)
                {
                    Console.WriteLine($@" Create User with {UserName} and {Pin}");
                    Become(Phase2);
                    //Stash.UnstashAll();
                }
            });
            Receive<LegacyDomain.Events.PinAdded>(e =>
            {
                Pin = e.Pin;
                if (WeAreValid)
                {
                    Console.WriteLine($@" Create User with {UserName} and {Pin}");
                    Become(Phase2);
                    //Stash.UnstashAll();
                }
            });

            //ReceiveAny(_ => Stash.Stash());
        }

        private void Phase2()
        {
            Receive<LegacyDomain.Events.UserNameAdded>(e => Console.Write(e.UserName));
            Receive<LegacyDomain.Events.PinAdded>(e => Console.WriteLine(e.Pin));
            Receive<LegacyDomain.Events.FirstNameAdded>(e => Console.WriteLine(e.FirstName));
            Receive<LegacyDomain.Events.LastNameAdded>(e=>Console.WriteLine(e.LastName));
        }
        private bool WeAreValid => UserName != null && Pin != null;
    }
}
