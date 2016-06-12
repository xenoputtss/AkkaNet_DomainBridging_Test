using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaNet_DomainBridging_Test.Actors;

namespace AkkaNet_DomainBridging_Test
{
    public class DistributionActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _actorRefs = new Dictionary<string, IActorRef>();

        public DistributionActor()
        {
            ReceiveAny(m =>
            {
                if (m is IAmAnAggregateMessage)
                {
                    var im = (IAmAnAggregateMessage)m;
                    if(!_actorRefs.ContainsKey(im.AggregateId))
                        _actorRefs.Add(im.AggregateId,Context.ActorOf(Props.Create(()=>new TranslatorActor(im.AggregateId))));

                    var actor = _actorRefs[im.AggregateId];
                    actor.Tell(m);
                }
                else
                {
                    throw new Exception("I can't handle this");
                }
            });
        }
    }
}
