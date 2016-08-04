using System.Threading.Tasks;
using Patterns.EventSourcing.Interface;
using Patterns.Registry.Interface;

namespace Patterns.Aggregates.Interface
{
    public interface IAggregateGrain<TGrain, TEvent, TGrainState, TAggregateValue> :
        IRegistryGrain<TGrain>
        where TGrain : IEventSourcedGrain<TEvent, TGrainState>
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>, new()
        where TAggregateValue : ICanApplyEvent<TEvent, TAggregateValue>, new()
    {
        Task<TimestampedValue<TAggregateValue>> GetAggregateValue();
    }
}