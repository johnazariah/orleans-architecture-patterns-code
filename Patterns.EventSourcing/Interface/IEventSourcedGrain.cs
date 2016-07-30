using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Patterns.EventSourcing.Interface
{
    public interface IEventSourcedGrain<TEvent, TGrainState> : IGrainWithGuidKey
        where TGrainState : ICanApplyEvent<TEvent, TGrainState>
    {
        Task<TGrainState> GetState();

        Task<List<TimestampedEvent<TEvent>>> GetEvents();
    }
}