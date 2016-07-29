using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Patterns.SmartCache.Interface;

namespace Patterns.Registry.Interface
{
    public interface IRegistryGrain<TRegistryItem, TRegistryItemGrain> : IGrainWithGuidKey
        where TRegistryItemGrain : ICachedItemGrain<TRegistryItem>
    {
        Task<TRegistryItemGrain> RegisterItem(TRegistryItemGrain item);

        Task<List<TRegistryItemGrain>> ListItems();
    }
}