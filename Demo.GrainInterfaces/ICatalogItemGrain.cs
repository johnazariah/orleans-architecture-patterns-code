using System;
using Patterns.SmartCache.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    [Serializable]
    public class CatalogItem
    {
        public string DisplayName { get; set; }
        public string SKU { get; set; }
        public string ShortDescription { get; set; }

        public override string ToString()
        {
            return $@"[SKU : {SKU}] [DisplayName : {DisplayName}]
    [ShortDescription: {ShortDescription}]";
        }
    }

    public interface ICatalogItemGrain : ICachedItemGrain<CatalogItem>
    {
    }
}