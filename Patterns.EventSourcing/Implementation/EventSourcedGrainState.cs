using System;
using System.Collections.Generic;
using Patterns.EventSourcing.Interface;

namespace Patterns.EventSourcing.Implementation
{
    [Serializable]
    public class EventSourcedGrainState<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
    {
        public List<TimestampedValue<TEvent>> Events { get; set; } = new List<TimestampedValue<TEvent>>();

        public TGrainState CurrentState { get; set; } = new TGrainState();
    }
}