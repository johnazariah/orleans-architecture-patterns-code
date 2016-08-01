using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    public abstract class BankAccountStateMachineData
    {
        public static Balance NewBalance(BankAccountStateMachineBalance bankAccountStateMachineBalance)
            => new Balance(bankAccountStateMachineBalance);

        public T Match<T>(Func<Balance, T> balanceFunc)
        {
            return balanceFunc((Balance) this);
        }

        public class Balance : BankAccountStateMachineData
        {
            public Balance(BankAccountStateMachineBalance bankAccountStateMachineBalance)
            {
                Item = bankAccountStateMachineBalance;
            }

            public BankAccountStateMachineBalance Item { get; }
        }
    }
}