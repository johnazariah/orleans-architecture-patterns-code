using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public abstract class BankAccountOperation
    {
        private BankAccountOperation()
        {
        }

        public static BankAccountOperation NewCredit(decimal amount) => new ChoiceTypes.Credit(amount);
        public static BankAccountOperation NewDebit(decimal amount) => new ChoiceTypes.Debit(amount);

        public abstract TResult Match<TResult>(Func<decimal, TResult> creditFunc, Func<decimal, TResult> debitFunc);

        private static class ChoiceTypes
        {

            [Serializable]
            internal class Credit : BankAccountOperation
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
            internal class Debit : BankAccountOperation
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
}