using System;
using System.Linq;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class AggregateDemo
    {
        public static void Run()
        {
            var bankTotalBalanceGrain =
                GrainClient.GrainFactory.GetGrain<IBankAccountAggregateGrain>(Constants.BankAccountAggregateGrainId);

            var bankAccountGrains =
                Constants.BankAccountGrainIds.Select(id =>
                {
                    var bankAccountGrain = GrainClient.GrainFactory.GetGrain<IBankAccountGrain>(id);
                    bankTotalBalanceGrain.RegisterGrain(bankAccountGrain);
                    return bankAccountGrain;
                }).ToList();

            foreach (var bankAccountGrain in bankAccountGrains.Take(10))
            {
                bankAccountGrain.CreditAmount(100.0M)
                                .Wait();
            }
            
            var totalBalance = bankTotalBalanceGrain.GetAggregateValue().Result.Value.Balance;
            Console.WriteLine($"Total Balance (should be 10 * 100) {totalBalance}");

            foreach (var bankAccountGrain in bankAccountGrains.Take(10))
            {
                bankAccountGrain.CreditAmount(100.0M)
                                .Wait();
            }

            totalBalance = bankTotalBalanceGrain.GetAggregateValue().Result.Value.Balance;
            Console.WriteLine($"Total Balance (should be 10 * 100 * 2) {totalBalance}");

            DevelopmentSiloHost.WaitForInteraction();
        }
    }
}