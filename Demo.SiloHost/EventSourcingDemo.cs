using System;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class EventSourcingDemo
    {
        public static void Run()
        {
            var bankBalanceGrain =
                GrainClient.GrainFactory.GetGrain<IBankBalanceEventSourcedGrain>(Constants.BankBalanceGrainId);

            bankBalanceGrain.CreditAmount(100.0M).Wait();
            bankBalanceGrain.DebitAmount(50.0M).Wait();
            bankBalanceGrain.CreditAmount(50.0M).Wait();

            var currentBalance = bankBalanceGrain.GetState()
                                                 .Result;
            Console.WriteLine(currentBalance.BalanceAmount);

            var history = bankBalanceGrain.GetEvents()
                                          .Result;

            foreach (var item in history)
            {
                Console.WriteLine(item);
            }

            DevelopmentSiloHost.WaitForInteraction();
        }
    }
}