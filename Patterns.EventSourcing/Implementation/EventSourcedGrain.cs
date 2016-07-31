using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Patterns.EventSourcing.Interface;

namespace Patterns.EventSourcing.Implementation
{
    public abstract class EventSourcedGrain<TEvent, TGrainState> :
        Grain<EventSourcedGrainState<TEvent, TGrainState>>,
        IEventSourcedGrain<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
    {
        public Task<TGrainState> GetState() => Task.FromResult(State.CurrentState);

        public Task<List<TimestampedValue<TEvent>>> GetEvents(DateTime? startTime = null, DateTime? endTime = null)
            =>
                Task.FromResult(
                    State.Events.Where(
                        _ =>
                            (_.Timestamp >= (startTime ?? DateTime.MinValue)) &&
                            (_.Timestamp < (endTime ?? DateTime.MaxValue)))
                         .ToList());

        protected async Task<TGrainState> ProcessEvent(TEvent grainEvent)
        {
            var timestampedGrainEvent = new TimestampedValue<TEvent>(grainEvent, DateTime.UtcNow);
            State.Events.Add(timestampedGrainEvent);
            State.CurrentState = State.CurrentState.ApplyEvent(timestampedGrainEvent, State.CurrentState);
            await WriteStateAsync();
            return State.CurrentState;
        }
    }
}