using System;
using System.Threading.Tasks;
using Orleans;
using Patterns.SmartCache.Interface;

namespace Patterns.SmartCache.Implementation
{
    [Serializable]
    public class CacheContainer<T>
    {
        public CacheContainer() : this(default(T))
        {
        }

        public CacheContainer(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    public abstract class CachedItemGrain<TRegistryItem> : Grain<CacheContainer<TRegistryItem>>,
        ICachedItemGrain<TRegistryItem>
    {
        public Task<TRegistryItem> GetItem()
        {
            return Task.FromResult(State.Value);
        }

        public async Task<TRegistryItem> SetItem(TRegistryItem item)
        {
            State.Value = item;
            await WriteStateAsync();

            return State.Value;
        }
    }
}