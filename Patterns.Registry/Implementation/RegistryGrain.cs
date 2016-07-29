using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Patterns.Registry.Interface;
using Patterns.SmartCache.Interface;

namespace Patterns.Registry.Implementation
{
    [StorageProvider(ProviderName = "RegistryStore")]
    public abstract class RegistryGrain<TRegistryItem, TRegistryItemGrain> : Grain<RegistryState<TRegistryItemGrain>>,
        IRegistryGrain<TRegistryItem, TRegistryItemGrain> where TRegistryItemGrain : ICachedItemGrain<TRegistryItem>
    {
        public Task<List<TRegistryItemGrain>> ListItems()
        {
            return Task.FromResult(State.ItemGrains.ToList());
        }

        public async Task<TRegistryItemGrain> RegisterItem(TRegistryItemGrain item)
        {
            if (State.ItemGrains == null)
            {
                State.ItemGrains = new HashSet<TRegistryItemGrain>();
            }
            State.ItemGrains.Add(item);
            await WriteStateAsync();
            return item;
        }
    }
}