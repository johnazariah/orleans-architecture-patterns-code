using System;
using System.Collections.Generic;

namespace Patterns.EventSourcing
{
    [Serializable]
    public class EventSourcedGrainState<TEvent, TGrainState> where TGrainState : new()
    {
        public List<TimestampedEvent<TEvent>> Events { get; set; } = new List<TimestampedEvent<TEvent>>();

        public TGrainState CurrentState { get; set; } = new TGrainState();
    }
}