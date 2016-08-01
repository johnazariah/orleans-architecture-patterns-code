using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    public class BankAccountStateMachineAmount : IComparable
    {
        public BankAccountStateMachineAmount(decimal value)
        {
            Value = value;
            if (value < 0.0M) throw new ArgumentOutOfRangeException(nameof(value), value, "Cannot be negative");
        }

        public decimal Value { get; }

        public int CompareTo(object other)
        {
            var otherAmount = other as BankAccountStateMachineAmount;
            return Value.CompareTo(otherAmount?.Value ?? decimal.MinusOne);
        }

        public override bool Equals(object obj)
        {
            var otherAmount = obj as BankAccountStateMachineAmount;
            return Value == otherAmount?.Value;
        }

        protected bool Equals(BankAccountStateMachineAmount other)
        {
            return Equals((object) other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public BankAccountStateMachineAmount Add(BankAccountStateMachineAmount other)
            => new BankAccountStateMachineAmount(Value + other.Value);

        public BankAccountStateMachineAmount Subtract(BankAccountStateMachineAmount other)
            => new BankAccountStateMachineAmount(Value - other.Value);
    }
}