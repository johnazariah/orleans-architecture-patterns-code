using System.Threading.Tasks;
using Orleans;

namespace Patterns.SmartCache.Interface
{
    public interface ICachedItemGrain<TItem> : IGrainWithGuidKey
    {
        Task<TItem> GetItem();
        Task<TItem> SetItem(TItem item);
    }
}