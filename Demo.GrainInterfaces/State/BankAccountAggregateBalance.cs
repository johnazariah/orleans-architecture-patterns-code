using System;
using Patterns.EventSourcing.Interface;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public class BankAccountAggregateBalance : ICanApplyEvent<BankAccountOperation, BankAccountAggregateBalance>
    {
        public BankAccountAggregateBalance(decimal amount)
        {
            Balance = amount;
        }

        public BankAccountAggregateBalance() : this(0.0M)
        {
        }

        public decimal Balance { get; }

        public BankAccountAggregateBalance ApplyEvent(TimestampedValue<BankAccountOperation> value,
            BankAccountAggregateBalance currentState)
        {
            return value.Value.Match(Credit(currentState), Debit(currentState));
        }

        private static Func<decimal, BankAccountAggregateBalance> Credit(BankAccountAggregateBalance current)
            => amount => new BankAccountAggregateBalance(current.Balance + amount);

        private static Func<decimal, BankAccountAggregateBalance> Debit(BankAccountAggregateBalance current)
            => amount => new BankAccountAggregateBalance(current.Balance - amount);
    }
}