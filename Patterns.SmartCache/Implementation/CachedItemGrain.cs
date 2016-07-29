using System.Threading.Tasks;
using Orleans;
using Patterns.SmartCache.Interface;

namespace Patterns.SmartCache.Implementation
{
    public abstract class CachedItemGrain<TRegistryItem> : Grain<TRegistryItem>, ICachedItemGrain<TRegistryItem>
    {
        public Task<TRegistryItem> GetItem()
        {
            return Task.FromResult(State);
        }

        public async Task<TRegistryItem> SetItem(TRegistryItem item)
        {
            State = item;
            await WriteStateAsync();

            return State;
        }
    }
}