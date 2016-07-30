using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Patterns.EventSourcing.Interface;

namespace Patterns.EventSourcing.Implementation
{
    public abstract class EventSourcedGrain<TEventType, TGrainState> :
        Grain<EventSourcedGrainState<TEventType, TGrainState>>,
        IEventSourcedGrain<TEventType, TGrainState>
        where TGrainState : ICanApplyEvent<TEventType, TGrainState>, new()
    {
        public Task<TGrainState> GetState() => Task.FromResult(State.CurrentState);

        public Task<List<TimestampedEvent<TEventType>>> GetEvents() => Task.FromResult(State.Events);

        protected async Task<TGrainState> ProcessEvent(TEventType grainEvent)
        {
            var timestampedGrainEvent = new TimestampedEvent<TEventType>(grainEvent, DateTime.UtcNow);
            State.Events.Add(timestampedGrainEvent);
            State.CurrentState = State.CurrentState.ApplyEvent(timestampedGrainEvent, State.CurrentState);
            await WriteStateAsync();
            return State.CurrentState;
        }
    }
}