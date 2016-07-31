using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Patterns.Registry.Interface;

namespace Patterns.Registry.Implementation
{
    [StorageProvider(ProviderName = "RegistryStore")]
    public abstract class RegistryGrain<TRegisteredGrain> : Grain<RegistryState<TRegisteredGrain>>,
        IRegistryGrain<TRegisteredGrain> where TRegisteredGrain : IGrain
    {
        public Task<List<TRegisteredGrain>> GetRegisteredGrains()
        {
            return Task.FromResult(State.RegisteredGrains.ToList());
        }

        public async Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item)
        {
            if (State.RegisteredGrains == null)
            {
                State.RegisteredGrains = new HashSet<TRegisteredGrain>();
            }
            State.RegisteredGrains.Add(item);
            await WriteStateAsync();
            return item;
        }
    }
}