using System;
using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class EventSourcingDemo
    {
        public static void Run()
        {
            var bankBalanceGrain =
                GrainClient.GrainFactory.GetGrain<IBankAccountGrain>(Constants.SingleBankAccountGrainId);

            ShowAccountState(bankBalanceGrain);
            DevelopmentSiloHost.WaitForInteraction();

            bankBalanceGrain.CreditAmount(100.0M)
                            .Wait();

            ShowAccountState(bankBalanceGrain);
            DevelopmentSiloHost.WaitForInteraction();

            bankBalanceGrain.DebitAmount(50.0M)
                            .Wait();

            ShowAccountState(bankBalanceGrain);
            DevelopmentSiloHost.WaitForInteraction();

            bankBalanceGrain.CreditAmount(50.0M)
                            .Wait();

            ShowAccountState(bankBalanceGrain);
            DevelopmentSiloHost.WaitForInteraction();
        }

        private static void ShowAccountState(IBankAccountGrain bankBalanceGrain)
        {
            Console.WriteLine(bankBalanceGrain.GetState()
                                              .Result.Balance);

            var history = bankBalanceGrain.GetEvents()
                                          .Result;

            foreach (var item in history)
            {
                Console.WriteLine(item);
            }
        }
    }
}