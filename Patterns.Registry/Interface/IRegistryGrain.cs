using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Patterns.Registry.Interface
{
    public interface IRegistryGrain<TRegisteredGrain> : IGrainWithGuidKey
        where TRegisteredGrain : IGrain
    {
        Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item);

        Task<List<TRegisteredGrain>> GetRegisteredGrains();
    }
}