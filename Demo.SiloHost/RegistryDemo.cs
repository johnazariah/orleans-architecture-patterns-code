using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class RegistryDemo
    {
        // Partial Application FTW!
        private static Func<Guid, int, Task> CreateAndRegisterCatalogItem(ICatalogItemRegistryGrain registryGrain)
            => async (id, index) =>
            {
                var grainId = id;
                var grainState =
                    new CatalogItem
                    {
                        DisplayName = $"Item {index}",
                        SKU = id.ToString(),
                        ShortDescription = $"This is the {index}th item"
                    };

                var grain = GrainClient.GrainFactory.GetGrain<ICatalogItemGrain>(grainId);
                await grain.SetItem(grainState);
                await registryGrain.RegisterItem(grain);
            };

        private static Task SetupCatalog()
        {
            var catalogRegistry = GrainClient.GrainFactory.GetGrain<ICatalogItemRegistryGrain>(Constants.RegistryId);
            var tasks = Constants.ItemIds.Select(CreateAndRegisterCatalogItem(catalogRegistry));

            return Task.WhenAll(tasks.ToArray());
        }

        private static async Task ReadCatalog()
        {
            var catalogRegistry = GrainClient.GrainFactory.GetGrain<ICatalogItemRegistryGrain>(Constants.RegistryId);
            var items = await catalogRegistry.ListItems();
            foreach (var item in items)
            {
                var state = await item.GetItem();
                Console.WriteLine(state);
            }
        }

        public static void Run()
        {
            SetupCatalog()
                .ContinueWith(_ => ReadCatalog())
                .ContinueWith(_ => DevelopmentSiloHost.WaitForInteraction())
                .Wait();
        }
    }
}