using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public class BankAccountStateMachineData
    {
        public BankAccountStateMachineData() : this(BankAccountStateMachineBalance.ZeroBankAccountStateMachineBalance)
        {
        }

        protected BankAccountStateMachineData(BankAccountStateMachineBalance bankAccountStateMachineBalance)
        {
            Balance = bankAccountStateMachineBalance;
        }

        public BankAccountStateMachineBalance Balance { get; set; }

        public static BankAccountStateMachineData NewBalance(BankAccountStateMachineBalance balance)
            => new BankAccountStateMachineData(balance);
    }
}