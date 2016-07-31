using Patterns.Registry.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    public interface ICatalogItemRegistryGrain : IRegistryGrain<ICatalogItemGrain>
    {
    }
}