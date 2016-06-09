using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AkkaNet_DomainBridging_Test.Actors
{
    public class TranslatorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        public TranslatorActor()
        {
            Become(WaitingForMiniumumData);
        }

        private void WaitingForMiniumumData()
        {
            Receive<LegacyDomain.Events.UserNameAdded>(e =>
            {
                Console.WriteLine(e.ToString());
            });
            Receive<LegacyDomain.Events.PinAdded>(e =>
            {
                Console.WriteLine(e.ToString());
            });
        }
    }
}
