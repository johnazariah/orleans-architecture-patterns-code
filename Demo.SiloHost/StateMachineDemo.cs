using System;
using System.Data.Common;
using System.Threading.Tasks;
using Demo.SmartCache.GrainImplementations;
using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans;
using Patterns.DevBreadboard;
using Patterns.StateMachine.Implementation;
using Patterns.StateMachine.Interface;

namespace Patterns.SmartCache.Host
{
    internal class StateMachineDemo
    {
        public static async Task Run()
        {
            var bankAccountStateMachine =
                GrainClient.GrainFactory.GetGrain<IBankAccountStateMachineGrain>(Constants.SingleBankAccountGrainId);

            var data = await bankAccountStateMachine.GetBalance();
            PrintBalance(data.Balance);

            data = await bankAccountStateMachine.Deposit(new BankAccountStateMachineAmount(100.0M));
            PrintBalance(data.Balance);

            data = await bankAccountStateMachine.Withdraw(new BankAccountStateMachineAmount(20.0M));
            PrintBalance(data.Balance);

            try
            {
                data = await bankAccountStateMachine.Close();
                PrintBalance(data.Balance);
            }
            catch (InvalidMessage)
            {
                Console.WriteLine("Tried to close an account which wasn't in ZeroBalance State and failed as expected!");
            }

            data = await bankAccountStateMachine.Withdraw(new BankAccountStateMachineAmount(100.0M));
            PrintBalance(data.Balance);

            try
            {
                data = await bankAccountStateMachine.Withdraw(new BankAccountStateMachineAmount(10.0M));
                PrintBalance(data.Balance);
            }
            catch (InvalidMessage)
            {
                Console.WriteLine("Tried to withdraw from an Overdrawn account and failed as expected!");
            }

            data = await bankAccountStateMachine.Deposit(new BankAccountStateMachineAmount(20.0M));
            PrintBalance(data.Balance);

            data = await bankAccountStateMachine.Close();
            PrintBalance(data.Balance);

            try
            {
                data = await bankAccountStateMachine.Deposit(new BankAccountStateMachineAmount(10.0M));
                PrintBalance(data.Balance);
            }
            catch (InvalidMessage)
            {
                Console.WriteLine("Tried to deposit into a Closed account and failed as expected!");
            }

        }

        private static void PrintBalance(BankAccountStateMachineBalance balance)
        {
            Console.WriteLine(balance.Match(() => "Zero Balance",
                _ => $"Active Balance: {_.Value}",
                _ => $"Overdrawn Balance: {_.Value}"));

            DevelopmentSiloHost.WaitForInteraction();
        }
    }
}