using System;

namespace Demo.SmartCache.GrainInterfaces.State
{
    [Serializable]
    public abstract class BankAccountStateMachineState
    {
        public static readonly BankAccountStateMachineState ZeroBalanceStateMachineState =
            new ChoiceTypes.ZeroBalanceStateMachineState();

        public static readonly BankAccountStateMachineState ActiveStateMachineState =
            new ChoiceTypes.ActiveStateMachineState();

        public static readonly BankAccountStateMachineState OverdrawnStateMachineState =
            new ChoiceTypes.OverdrawnStateMachineState();

        public static readonly BankAccountStateMachineState ClosedStateMachineState =
            new ChoiceTypes.ClosedStateMachineState();


        public abstract T Match<T>(
            Func<T> zeroBalanceStateFunc,
            Func<T> activeStateFunc,
            Func<T> overdrawnStateFunc,
            Func<T> closedStateFunc);

        private static class ChoiceTypes
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            [Serializable]
            public class ZeroBalanceStateMachineState : BankAccountStateMachineState
            {
                public override T Match<T>(
                    Func<T> zeroBalanceStateFunc,
                    Func<T> activeStateFunc,
                    Func<T> overdrawnStateFunc,
                    Func<T> closedStateFunc) => zeroBalanceStateFunc();
            }

            [Serializable]
            public class ActiveStateMachineState : BankAccountStateMachineState
            {
                public override T Match<T>(
                    Func<T> zeroBalanceStateFunc,
                    Func<T> activeStateFunc,
                    Func<T> overdrawnStateFunc,
                    Func<T> closedStateFunc) => activeStateFunc();
            }

            [Serializable]
            public class OverdrawnStateMachineState : BankAccountStateMachineState
            {
                public override T Match<T>(
                    Func<T> zeroBalanceStateFunc,
                    Func<T> activeStateFunc,
                    Func<T> overdrawnStateFunc,
                    Func<T> closedStateFunc) => overdrawnStateFunc();
            }

            [Serializable]
            public class ClosedStateMachineState : BankAccountStateMachineState
            {
                public override T Match<T>(
                    Func<T> zeroBalanceStateFunc,
                    Func<T> activeStateFunc,
                    Func<T> overdrawnStateFunc,
                    Func<T> closedStateFunc) => closedStateFunc();
            }

            // ReSharper restore MemberHidesStaticFromOuterClass
        }
    }
}