using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    //internal class StateMachineDemo
    //{
    //    public static void Run()
    //    {
    //        InitializeStateMachine()
    //            .ContinueWith(_ => ReadItems())
    //            .ContinueWith(_ => DevelopmentSiloHost.WaitForInteraction())
    //            .Wait();
    //    }
    //}

    internal class SmartCacheDemo
    {
        public static async Task CreateCatalogItem(Guid id, int index)
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
        }

        public static Task InitializeItems()
        {
            var createTasks = Constants.ItemIds.Select(CreateCatalogItem);
            return Task.WhenAll(createTasks.ToArray());
        }

        public static async Task ReadItems()
        {
            foreach (var itemId in Constants.ItemIds)
            {
                var grain = GrainClient.GrainFactory.GetGrain<ICatalogItemGrain>(itemId);
                var state = await grain.GetItem();
                Console.WriteLine(state);
            }
        }

        public static void Run()
        {
            InitializeItems()
                .ContinueWith(_ => ReadItems())
                .ContinueWith(_ => DevelopmentSiloHost.WaitForInteraction())
                .Wait();
        }
    }
}