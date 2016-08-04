using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public abstract partial class BankAccountStateMachineBalance
    {
        public static readonly BankAccountStateMachineBalance ZeroBankAccountStateMachineBalance =
            new ChoiceTypes.ZeroBankAccountStateMachineBalance();

        private BankAccountStateMachineBalance()
        {
        }

        public static BankAccountStateMachineBalance NewActiveBalance(BankAccountStateMachineAmount amount)
            => new ChoiceTypes.ActiveBankAccountStateMachineBalance(amount);

        public static BankAccountStateMachineBalance NewOverdrawnBalance(BankAccountStateMachineAmount amount)
            => new ChoiceTypes.OverdrawnBankAccountStateMachineBalance(amount);

        public abstract TResult Match<TResult>(Func<TResult> zeroBalanceFunc,
            Func<BankAccountStateMachineAmount, TResult> activeBalanceFunc,
            Func<BankAccountStateMachineAmount, TResult> overdrawnBalanceFunc);

        private static partial class ChoiceTypes
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            [Serializable]
            public partial class ZeroBankAccountStateMachineBalance : BankAccountStateMachineBalance
            {
                public override TResult Match<TResult>(Func<TResult> zeroBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> activeBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> overdrawnBalanceFunc) => zeroBalanceFunc();
            }

            // ReSharper restore MemberHidesStaticFromOuterClass
            [Serializable]
            public partial class ActiveBankAccountStateMachineBalance : BankAccountStateMachineBalance
            {
                public ActiveBankAccountStateMachineBalance(BankAccountStateMachineAmount amount)
                {
                    Amount = amount;
                }

                private BankAccountStateMachineAmount Amount { get; }

                public override TResult Match<TResult>(Func<TResult> zeroBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> activeBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> overdrawnBalanceFunc) => activeBalanceFunc(Amount);
            }

            [Serializable]
            public partial class OverdrawnBankAccountStateMachineBalance : BankAccountStateMachineBalance
            {
                public OverdrawnBankAccountStateMachineBalance(BankAccountStateMachineAmount amount)
                {
                    Amount = amount;
                }

                private BankAccountStateMachineAmount Amount { get; }

                public override TResult Match<TResult>(Func<TResult> zeroBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> activeBalanceFunc,
                    Func<BankAccountStateMachineAmount, TResult> overdrawnBalanceFunc) => overdrawnBalanceFunc(Amount);
            }
        }
    }

    public abstract partial class BankAccountStateMachineBalance
    {
        public abstract BankAccountStateMachineBalance Deposit(BankAccountStateMachineAmount amount);
        public abstract BankAccountStateMachineBalance Withdraw(BankAccountStateMachineAmount amount);

        private static partial class ChoiceTypes
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            public partial class ZeroBankAccountStateMachineBalance
            {
                public override BankAccountStateMachineBalance Deposit(BankAccountStateMachineAmount amount)
                {
                    return NewActiveBalance(amount);
                }

                public override BankAccountStateMachineBalance Withdraw(BankAccountStateMachineAmount amount)
                {
                    return NewOverdrawnBalance(amount);
                }
            }

            // ReSharper restore MemberHidesStaticFromOuterClass
            public partial class ActiveBankAccountStateMachineBalance
            {
                public override BankAccountStateMachineBalance Deposit(BankAccountStateMachineAmount amount)
                {
                    return NewActiveBalance(Amount.Add(amount));
                }

                public override BankAccountStateMachineBalance Withdraw(BankAccountStateMachineAmount amount)
                {
                    return Amount.Equals(amount)
                        ? ZeroBankAccountStateMachineBalance
                        : (Amount.CompareTo(amount) > 0
                            ? NewActiveBalance(Amount.Subtract(amount))
                            : NewOverdrawnBalance(amount.Subtract(Amount)));
                }
            }

            public partial class OverdrawnBankAccountStateMachineBalance
            {
                public override BankAccountStateMachineBalance Deposit(BankAccountStateMachineAmount amount)
                {
                    return NewOverdrawnBalance(Amount.Add(amount));
                }

                public override BankAccountStateMachineBalance Withdraw(BankAccountStateMachineAmount amount)
                {
                    return Amount.Equals(amount)
                        ? ZeroBankAccountStateMachineBalance
                        : (Amount.CompareTo(amount) > 0
                            ? NewOverdrawnBalance(Amount.Subtract(amount))
                            : NewActiveBalance(amount.Subtract(Amount)));
                }
            }
        }
    }
}