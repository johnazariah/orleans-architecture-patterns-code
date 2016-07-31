using System;
using System.Threading.Tasks;
using Patterns.EventSourcing.Interface;

namespace Patterns.Aggregates.Implementation
{
    public class LazilyComputedAggregateGrain<TGrain, TEvent, TGrainState, TAggregateState> :
        AbstractAggregateGrain<TGrain, TEvent, TGrainState, TAggregateState>
        where TGrain : IEventSourcedGrain<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
        where TAggregateState : ICanApplyEvent<TEvent, TAggregateState>, new()
    {
        private async Task<TAggregateState> ComputeAggregateValue()
        {
            var earliestTimestamp = State.LastUpdatedTime;
            State.LastUpdatedTime = DateTime.UtcNow;

            Console.WriteLine($"Accessing events between {earliestTimestamp} and {State.LastUpdatedTime}");

            foreach (var grain in State.RegisteredGrains)
            {
                foreach (var e in await grain.GetEvents(earliestTimestamp, State.LastUpdatedTime))
                {
                    State.AggregateValue = State.AggregateValue.ApplyEvent(e, State.AggregateValue);
                }
            }
            await WriteStateAsync();

            return State.AggregateValue;
        }

        public override async Task<TimestampedValue<TAggregateState>> GetAggregateValue()
        {
            await ComputeAggregateValue();
            return new TimestampedValue<TAggregateState>(State.AggregateValue, State.LastUpdatedTime);
        }
    }
}