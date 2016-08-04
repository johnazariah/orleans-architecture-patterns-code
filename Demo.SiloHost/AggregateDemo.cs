using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class AggregateDemo
    {
        public static async Task Run()
        {
            var accountAggregateGrain =
                GrainClient.GrainFactory.GetGrain<IBankAccountAggregateGrain>(Constants.BankAccountAggregateGrainId);
            var accountGrains =
                Constants.BankAccountGrainIds.Select(id => GrainClient.GrainFactory.GetGrain<IBankAccountGrain>(id))
                         .ToList();

            // register the account grains with the aggregate
            foreach (var accountGrain in accountGrains)
            {
                await accountAggregateGrain.RegisterGrain(accountGrain);
            }

            // run a set of operations...
            foreach (var accountGrain in accountGrains.Take(10))
            {
                await accountGrain.CreditAmount(100.0M);
            }

            //...and see the aggregate value updated consequently
            var totalBalance = await accountAggregateGrain.GetAggregateValue();
            Console.WriteLine($"Total Balance (should be 10 * 100) {totalBalance.Value.Balance}");

            // run another set of operations...
            foreach (var accountGrain in accountGrains.Take(10))
            {
                await accountGrain.CreditAmount(100.0M);
            }

            // ...and see the aggregate value accumulate
            totalBalance = await accountAggregateGrain.GetAggregateValue();
            Console.WriteLine($"Total Balance (should be 10 * 100 * 2) {totalBalance.Value.Balance}");

            DevelopmentSiloHost.WaitForInteraction();
        }
    }
}