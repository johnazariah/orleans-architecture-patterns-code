using System;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces.State;
using Patterns.StateMachine.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    public interface IBankAccountStateMachineGrain :
        IStateMachineGrain<BankAccountStateMachineData, BankAccountStateMachineMessage>
    {
        Task<BankAccountStateMachineData> Deposit(BankAccountStateMachineAmount bankAccountStateMachineAmount);
        Task<BankAccountStateMachineData> Withdrawal(BankAccountStateMachineAmount bankAccountStateMachineAmount);
        Task<BankAccountStateMachineData> Close();
    }
}