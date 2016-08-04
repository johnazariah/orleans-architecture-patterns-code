using System;
using Patterns.EventSourcing.Interface;
using Patterns.Registry.Implementation;

namespace Patterns.Aggregates.Interface
{
    [Serializable]
    public class AggregateGrainState<TEvent, TGrain, TGrainState, TAggregateValue> : RegistryState<TGrain>
        where TGrain : IEventSourcedGrain<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
        where TAggregateValue : ICanApplyEvent<TEvent, TAggregateValue>, new()
    {
        public DateTime LastUpdatedTime { get; set; } = DateTime.MinValue;

        public TAggregateValue AggregateValue { get; set; } = new TAggregateValue();
    }
}