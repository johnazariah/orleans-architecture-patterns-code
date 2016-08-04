using System;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Orleans;
using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class EventSourcingDemo
    {
        public static async Task Run()
        {
            var accountGrain = GrainClient.GrainFactory.GetGrain<IBankAccountGrain>(Constants.SingleBankAccountGrainId);

            await ShowAccountState(accountGrain);

            await accountGrain.CreditAmount(100.0M);
            await ShowAccountState(accountGrain);

            await accountGrain.DebitAmount(50.0M);
            await ShowAccountState(accountGrain);

            await accountGrain.CreditAmount(50.0M);
            await ShowAccountState(accountGrain);
        }

        private static async Task ShowAccountState(IBankAccountGrain accountGrain)
        {
            var bankAccountState = await accountGrain.GetState();
            var history = await accountGrain.GetEvents();

            Console.WriteLine(bankAccountState.Balance);
            foreach (var item in history)
            {
                Console.WriteLine(item);
            }

            DevelopmentSiloHost.WaitForInteraction();
        }
    }
}