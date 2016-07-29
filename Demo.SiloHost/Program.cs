using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class Program
    {
        private static readonly Guid CatalogRegistryId = Guid.Parse("{d8c4eea3-ddd4-4d55-9287-a3201a698ae6}");

        private static readonly Guid[] ItemIds = new[]
        {
            "{80f81f8c-0d25-4c41-9bb6-1dfe316e2906}",
            "{7ebc2041-c353-4358-a80e-8bb86102b281}",
            "{05ecdb3a-b2bb-44e3-82fa-74a02bddf5fe}",
            "{3e939f25-bd50-4ce3-954a-3879c05122b6}",
            "{680902fd-58e0-4685-8771-4bca361d1964}",
            "{764b2999-81d2-484c-bddc-3b72d1278356}",
            "{fc59711f-5bc5-4023-a066-27c7453dee46}",
            "{b8383295-4db2-4f3a-a29e-d4818d2e9a7d}",
            "{83615992-ac35-476a-b0f1-7e11fdb47966}",
            "{6ed36967-379c-4b46-a7dc-44685b9ee1ae}"
        }.Select(Guid.Parse)
         .ToArray();

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            using (new DevelopmentSiloHost())
            {
                using (new DevelopmentOrleansClientContext())
                {
                    SetupCatalog()
                        .ContinueWith(_ => ReadCatalog())
                        .ContinueWith(_ => DevelopmentSiloHost.WaitForInteraction())
                        .Wait();
                }
            }
        }

        private static Task SetupCatalog()
        {
            var catalogRegistry = GrainClient.GrainFactory.GetGrain<ICatalogItemRegistryGrain>(CatalogRegistryId);
            var tasks = ItemIds.Select(CreateAndRegisterCatalogItem(catalogRegistry));

            return Task.WhenAll(tasks.ToArray());
        }

        private static async Task ReadCatalog()
        {
            var catalogRegistry = GrainClient.GrainFactory.GetGrain<ICatalogItemRegistryGrain>(CatalogRegistryId);
            var items = await catalogRegistry.ListItems();
            foreach (var item in items)
            {
                var state = await item.GetItem();
                Console.WriteLine(state);
            }
        }

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
    }
}