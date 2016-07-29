using System.Threading.Tasks;
using Orleans;

namespace Patterns.SmartCache.Interface
{
    public interface ICachedItemGrain<TRegistryItem> : IGrainWithGuidKey
    {
        Task<TRegistryItem> GetItem();
        Task<TRegistryItem> SetItem(TRegistryItem item);
    }
}