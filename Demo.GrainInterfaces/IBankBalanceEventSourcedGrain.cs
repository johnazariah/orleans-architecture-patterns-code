using System;
using System.Threading.Tasks;
using Patterns.EventSourcing.Interface;

namespace Demo.SmartCache.GrainInterfaces
{

    #region BankingOperation Event

    [Serializable]
    public abstract class BankingOperation
    {
        private BankingOperation()
        {
        }

        public static BankingOperation NewCredit(decimal amount) => new ChoiceTypes.Credit(amount);
        public static BankingOperation NewDebit(decimal amount) => new ChoiceTypes.Debit(amount);

        public abstract TResult Match<TResult>(Func<decimal, TResult> creditFunc, Func<decimal, TResult> debitFunc);

        private static class ChoiceTypes
        {

            [Serializable]
            internal class Credit : BankingOperation
            {
                public Credit(decimal amount)
                {
                    Amount = amount;
                }

                private decimal Amount { get; }

                public override TResult Match<TResult>(Func<decimal, TResult> creditFunc,
                    Func<decimal, TResult> debitFunc) => creditFunc(Amount);

                public override string ToString()
                {
                    return $"CR {Amount}";
                }
            }


            [Serializable]
            internal class Debit : BankingOperation
            {
                public Debit(decimal amount)
                {
                    Amount = amount;
                }

                private decimal Amount { get; }

                public override TResult Match<TResult>(Func<decimal, TResult> creditFunc,
                    Func<decimal, TResult> debitFunc) => debitFunc(Amount);

                public override string ToString()
                {
                    return $"DR {Amount}";
                }
            }
        }
    }

    #endregion

    [Serializable]
    public class BankBalance : ICanApplyEvent<BankingOperation, BankBalance>
    {
        public BankBalance(decimal amount)
        {
            BalanceAmount = amount;
        }

        public BankBalance() : this(0.0M)
        {
        }

        public decimal BalanceAmount { get; }

        public BankBalance ApplyEvent(TimestampedEvent<BankingOperation> _event, BankBalance currentState)
        {
            return _event.EventArgs.Match(Credit(currentState), Debit(currentState));
        }

        private static Func<decimal, BankBalance> Credit(BankBalance current)
            => amount => new BankBalance(current.BalanceAmount + amount);

        private static Func<decimal, BankBalance> Debit(BankBalance current)
            => amount => new BankBalance(current.BalanceAmount - amount);
    }

    public interface IBankBalanceEventSourcedGrain : IEventSourcedGrain<BankingOperation, BankBalance>
    {
        Task<BankBalance> CreditAmount(decimal amount);
        Task<BankBalance> DebitAmount(decimal amount);
    }
}