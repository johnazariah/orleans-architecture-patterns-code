using Demo.SmartCache.GrainInterfaces;
using Orleans.Providers;
using Patterns.SmartCache.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    [StorageProvider(ProviderName = "RegistryItemStore")]
    public class CatalogItemGrain : CachedItemGrain<CatalogItem>, ICatalogItemGrain
    {
    }
}