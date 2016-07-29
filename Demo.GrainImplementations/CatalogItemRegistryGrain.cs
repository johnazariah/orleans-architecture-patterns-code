using Demo.SmartCache.GrainInterfaces;
using Orleans.Providers;
using Patterns.Registry.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    [StorageProvider(ProviderName = "RegistryStore")]
    public class CatalogItemRegistryGrain : RegistryGrain<CatalogItem, ICatalogItemGrain>, ICatalogItemRegistryGrain
    {
    }

}