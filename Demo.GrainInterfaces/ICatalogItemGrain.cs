using Demo.SmartCache.GrainInterfaces.State;
using Patterns.SmartCache.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    public interface ICatalogItemGrain : ICachedItemGrain<CatalogItem>
    {
    }
}