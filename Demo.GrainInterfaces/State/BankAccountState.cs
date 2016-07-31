using System;
using Patterns.EventSourcing.Interface;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public class BankAccountState : ICanApplyEvent<BankAccountOperation, BankAccountState>
    {
        public BankAccountState(decimal amount)
        {
            Balance = amount;
        }

        public BankAccountState() : this(0.0M)
        {
        }

        public decimal Balance { get; }

        public BankAccountState ApplyEvent(TimestampedValue<BankAccountOperation> value, BankAccountState currentState)
        {
            return value.Value.Match(Credit(currentState), Debit(currentState));
        }

        private static Func<decimal, BankAccountState> Credit(BankAccountState current)
            => amount => new BankAccountState(current.Balance + amount);

        private static Func<decimal, BankAccountState> Debit(BankAccountState current)
            => amount => new BankAccountState(current.Balance - amount);
    }
}