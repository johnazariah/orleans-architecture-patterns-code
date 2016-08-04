using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Patterns.Aggregates.Interface;
using Patterns.EventSourcing.Interface;

namespace Patterns.Aggregates.Implementation
{
    public abstract class AbstractAggregateGrain<TGrain, TEvent, TGrainState, TAggregateState> :
        Grain<AggregateGrainState<TEvent, TGrain, TGrainState, TAggregateState>>,
        IAggregateGrain<TGrain, TEvent, TGrainState, TAggregateState>
        where TGrain : IEventSourcedGrain<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
        where TAggregateState : ICanApplyEvent<TEvent, TAggregateState>, new()
    {
        public abstract Task<TimestampedValue<TAggregateState>> GetAggregateValue();
       
        public async Task<TGrain> RegisterGrain(TGrain item)
        {
            if (State.RegisteredGrains == null)
            {
                State.RegisteredGrains = new HashSet<TGrain>();
            }
            State.RegisteredGrains.Add(item);
            await WriteStateAsync();
            return item;
        }

        public Task<List<TGrain>> GetRegisteredGrains() => Task.FromResult(State.RegisteredGrains.ToList());
    }
}