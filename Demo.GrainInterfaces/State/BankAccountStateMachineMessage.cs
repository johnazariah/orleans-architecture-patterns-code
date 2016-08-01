using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    public abstract class BankAccountStateMachineMessage
    {
        public static readonly BankAccountStateMachineMessage CloseMessage =
            new ChoiceTypes.CloseStateMachineMessage();

        public static BankAccountStateMachineMessage NewDepositMessage(
            BankAccountStateMachineAmount bankAccountStateMachineAmount)
            => new ChoiceTypes.DepositStateMachineMessage(bankAccountStateMachineAmount);

        public static BankAccountStateMachineMessage NewWithdrawMessage(
            BankAccountStateMachineAmount bankAccountStateMachineAmount)
            => new ChoiceTypes.WithdrawStateMachineMessage(bankAccountStateMachineAmount);

        public abstract T Match<T>(
            Func<T> closeMessageFunc,
            Func<BankAccountStateMachineAmount, T> depositMessageFunc,
            Func<BankAccountStateMachineAmount, T> withdrawMessageFunc);


        private static class ChoiceTypes
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            public class CloseStateMachineMessage : BankAccountStateMachineMessage
            {
                public override T Match<T>(
                    Func<T> closeMessageFunc,
                    Func<BankAccountStateMachineAmount, T> depositMessageFunc,
                    Func<BankAccountStateMachineAmount, T> withdrawMessageFunc) => closeMessageFunc();
            }

            public class DepositStateMachineMessage : BankAccountStateMachineMessage
            {
                public DepositStateMachineMessage(BankAccountStateMachineAmount item)
                {
                    Item = item;
                }

                private BankAccountStateMachineAmount Item { get; }

                public override T Match<T>(
                    Func<T> closeMessageFunc,
                    Func<BankAccountStateMachineAmount, T> depositMessageFunc,
                    Func<BankAccountStateMachineAmount, T> withdrawMessageFunc)
                {
                    return depositMessageFunc(Item);
                }
            }

            public class WithdrawStateMachineMessage : BankAccountStateMachineMessage
            {
                public WithdrawStateMachineMessage(BankAccountStateMachineAmount item)
                {
                    Item = item;
                }

                private BankAccountStateMachineAmount Item { get; }

                public override T Match<T>(
                    Func<T> closeMessageFunc,
                    Func<BankAccountStateMachineAmount, T> depositMessageFunc,
                    Func<BankAccountStateMachineAmount, T> withdrawMessageFunc)
                {
                    return withdrawMessageFunc(Item);
                }
            }

            // ReSharper restore MemberHidesStaticFromOuterClass
        }
    }
}