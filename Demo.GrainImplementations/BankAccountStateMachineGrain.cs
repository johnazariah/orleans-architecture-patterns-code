using System;
using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans.Providers;
using Patterns.StateMachine.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    using TProcessorFunc =
        Func<BankAccountStateMachineGrainState, BankAccountStateMachineMessage, Task<BankAccountStateMachineGrainState>>
        ;

    [StorageProvider(ProviderName = "EventStore")]
    public partial class BankAccountStateMachineGrain :
        StateMachineGrain
            <BankAccountStateMachineGrainState, BankAccountStateMachineData, BankAccountStateMachineState,
                BankAccountStateMachineMessage>,
        IBankAccountStateMachineGrain
    {
        public Task<BankAccountStateMachineData> GetBalance()
        {
            return Task.FromResult(State.StateMachineData);
        }

        public async Task<BankAccountStateMachineData> Deposit(BankAccountStateMachineAmount amount)
            => await ProcessMessage(BankAccountStateMachineMessage.NewDepositMessage(amount));

        public async Task<BankAccountStateMachineData> Withdraw(BankAccountStateMachineAmount amount)
            => await ProcessMessage(BankAccountStateMachineMessage.NewWithdrawMessage(amount));

        public async Task<BankAccountStateMachineData> Close()
            => await ProcessMessage(BankAccountStateMachineMessage.CloseMessage);

        protected override TProcessorFunc GetProcessorFunc(BankAccountStateMachineState state)
        {
            return state.Match<TProcessorFunc>(
                () => ZeroBalanceStateProcessor,
                () => ActiveStateProcessor,
                () => OverdrawnStateProcessor,
                () => ClosedStateProcessor);
        }
    }

    #region ZeroBalanceState

    public partial class BankAccountStateMachineGrain
    {
        private static async Task<BankAccountStateMachineGrainState> ZeroBalanceStateProcessor(
            BankAccountStateMachineGrainState state,
            BankAccountStateMachineMessage message)
            =>
                await
                    message.Match(
                        ZeroBalanceStateMessageDelegator.HandleZeroBalanceStateCloseMessage(state),
                        ZeroBalanceStateMessageDelegator.HandleZeroBalanceStateDepositMessage(state),
                        HandleInvalidMessage);

        private static class ZeroBalanceStateMessageDelegator
        {
            private static readonly IZeroBalanceStateMessageHandler Handler = new ZeroBalanceStateMessageHandler();

            public static Func<Task<BankAccountStateMachineGrainState>> HandleZeroBalanceStateCloseMessage(
                BankAccountStateMachineGrainState state)
            {
                return async () =>
                {
                    var result = await Handler.Close(state);
                    return (BankAccountStateMachineGrainState) result;
                };
            }

            public static Func<BankAccountStateMachineAmount, Task<BankAccountStateMachineGrainState>>
                HandleZeroBalanceStateDepositMessage(BankAccountStateMachineGrainState state)
            {
                return async _ =>
                {
                    var result = await Handler.Deposit(state, _);
                    return (BankAccountStateMachineGrainState) result;
                };
            }
        }

        private interface IZeroBalanceStateMessageHandler
        {
            Task<ZeroBalanceStateMessageHandler.ZeroBalanceCloseResult> Close(BankAccountStateMachineGrainState state);

            Task<ZeroBalanceStateMessageHandler.ZeroBalanceDepositResult> Deposit(
                BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount);
        }

        private partial class ZeroBalanceStateMessageHandler : IZeroBalanceStateMessageHandler
        {
            internal abstract class ZeroBalanceDepositResultState
            {
                public static readonly ZeroBalanceDepositResultState ActiveState = new ChoiceTypes.ActiveState();

                private readonly BankAccountStateMachineState _bankAccountState;

                private ZeroBalanceDepositResultState(BankAccountStateMachineState bankAccountStateMachineState)
                {
                    _bankAccountState = bankAccountStateMachineState;
                }

                public static explicit operator BankAccountStateMachineState(ZeroBalanceDepositResultState _)
                    => _._bankAccountState;

                private static class ChoiceTypes
                {
                    // ReSharper disable MemberHidesStaticFromOuterClass
                    public class ActiveState : ZeroBalanceDepositResultState
                    {
                        public ActiveState() : base(BankAccountStateMachineState.ActiveStateMachineState)
                        {
                        }
                    }

                    // ReSharper restore MemberHidesStaticFromOuterClass
                }
            }

            public class ZeroBalanceDepositResult :
                StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>.StateTransitionResult
                    <ZeroBalanceDepositResultState>
            {
                public ZeroBalanceDepositResult(BankAccountStateMachineData stateMachineData,
                    ZeroBalanceDepositResultState stateMachineState)
                    : base(stateMachineData, stateMachineState)
                {
                }

                public static explicit operator BankAccountStateMachineGrainState(ZeroBalanceDepositResult result)
                    =>
                        new BankAccountStateMachineGrainState(result.StateMachineData,
                            (BankAccountStateMachineState) result.StateMachineState);
            }

            internal abstract class ZeroBalanceCloseResultState
            {
                public static readonly ZeroBalanceCloseResultState ClosedState = new ChoiceTypes.ClosedState();

                private readonly BankAccountStateMachineState _bankAccountState;

                private ZeroBalanceCloseResultState(BankAccountStateMachineState bankAccountStateMachineState)
                {
                    _bankAccountState = bankAccountStateMachineState;
                }

                public static explicit operator BankAccountStateMachineState(ZeroBalanceCloseResultState _)
                    => _._bankAccountState;

                private static class ChoiceTypes
                {
                    // ReSharper disable MemberHidesStaticFromOuterClass
                    public class ClosedState : ZeroBalanceCloseResultState
                    {
                        public ClosedState() : base(BankAccountStateMachineState.ClosedStateMachineState)
                        {
                        }
                    }

                    // ReSharper restore MemberHidesStaticFromOuterClass
                }
            }

            public class ZeroBalanceCloseResult :
                StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>.StateTransitionResult
                    <ZeroBalanceCloseResultState>
            {
                public ZeroBalanceCloseResult(BankAccountStateMachineData stateMachineData,
                    ZeroBalanceCloseResultState stateMachineState)
                    : base(stateMachineData, stateMachineState)
                {
                }

                public static explicit operator BankAccountStateMachineGrainState(ZeroBalanceCloseResult result)
                    =>
                        new BankAccountStateMachineGrainState(result.StateMachineData,
                            (BankAccountStateMachineState) result.StateMachineState);
            }
        }
    }

    public partial class BankAccountStateMachineGrain
    {
        private partial class ZeroBalanceStateMessageHandler
        {
            public Task<ZeroBalanceDepositResult> Deposit(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount)
            {
                var newBalance = state.StateMachineData.Balance.Deposit(amount);

                var stateMachineData = BankAccountStateMachineData.NewBalance(newBalance);
                var stateMachineState = ZeroBalanceDepositResultState.ActiveState;

                return Task.FromResult(new ZeroBalanceDepositResult(stateMachineData, stateMachineState));
            }

            public Task<ZeroBalanceCloseResult> Close(BankAccountStateMachineGrainState state)
            {
                var stateMachineData =
                    BankAccountStateMachineData.NewBalance(
                        BankAccountStateMachineBalance.ZeroBankAccountStateMachineBalance);
                var stateMachineState = ZeroBalanceCloseResultState.ClosedState;

                return Task.FromResult(new ZeroBalanceCloseResult(stateMachineData, stateMachineState));
            }
        }
    }

    #endregion

    #region ActiveState

    public partial class BankAccountStateMachineGrain
    {
        private static async Task<BankAccountStateMachineGrainState> ActiveStateProcessor(
            BankAccountStateMachineGrainState state,
            BankAccountStateMachineMessage message)
            =>
                await
                    message.Match(
                        HandleInvalidMessage,
                        ActiveStateMessageDelegator.HandleActiveStateDepositMessage(state),
                        ActiveStateMessageDelegator.HandleActiveStateWithdrawMessage(state));

        private static class ActiveStateMessageDelegator
        {
            private static readonly IActiveStateMessageHandler Handler = new ActiveStateMessageHandler();

            public static Func<BankAccountStateMachineAmount, Task<BankAccountStateMachineGrainState>>
                HandleActiveStateDepositMessage(BankAccountStateMachineGrainState state)
            {
                return async _ =>
                {
                    var result = await Handler.Deposit(state, _);
                    return (BankAccountStateMachineGrainState) result;
                };
            }

            public static Func<BankAccountStateMachineAmount, Task<BankAccountStateMachineGrainState>>
                HandleActiveStateWithdrawMessage(BankAccountStateMachineGrainState state)
            {
                return async _ =>
                {
                    var result = await Handler.Withdraw(state, _);
                    return (BankAccountStateMachineGrainState) result;
                };
            }
        }

        private interface IActiveStateMessageHandler
        {
            Task<ActiveStateMessageHandler.ActiveDepositResult> Deposit(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount);

            Task<ActiveStateMessageHandler.ActiveWithdrawResult> Withdraw(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount);
        }

        private partial class ActiveStateMessageHandler : IActiveStateMessageHandler
        {
            internal abstract class ActiveDepositResultState
            {
                public static readonly ActiveDepositResultState ActiveState = new ChoiceTypes.ActiveState();
                private readonly BankAccountStateMachineState _bankAccountState;

                private ActiveDepositResultState(BankAccountStateMachineState bankAccountStateMachineState)
                {
                    _bankAccountState = bankAccountStateMachineState;
                }

                public static explicit operator BankAccountStateMachineState(ActiveDepositResultState _)
                    => _._bankAccountState;

                private static class ChoiceTypes
                {
                    // ReSharper disable MemberHidesStaticFromOuterClass
                    public class ActiveState : ActiveDepositResultState
                    {
                        public ActiveState() : base(BankAccountStateMachineState.ActiveStateMachineState)
                        {
                        }
                    }

                    // ReSharper restore MemberHidesStaticFromOuterClass
                }
            }

            public class ActiveDepositResult :
                StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>.StateTransitionResult
                    <ActiveDepositResultState>
            {
                public ActiveDepositResult(BankAccountStateMachineData stateMachineData,
                    ActiveDepositResultState stateMachineState)
                    : base(stateMachineData, stateMachineState)
                {
                }

                public static explicit operator BankAccountStateMachineGrainState(ActiveDepositResult result)
                    =>
                        new BankAccountStateMachineGrainState(result.StateMachineData,
                            (BankAccountStateMachineState) result.StateMachineState);
            }

            internal abstract class ActiveWithdrawResultState
            {
                public static readonly ActiveWithdrawResultState ActiveState = new ChoiceTypes.ActiveState();
                public static readonly ActiveWithdrawResultState OverdrawnState = new ChoiceTypes.OverdrawnState();
                public static readonly ActiveWithdrawResultState ZeroBalanceState = new ChoiceTypes.ZeroBalanceState();
                private readonly BankAccountStateMachineState _bankAccountState;

                private ActiveWithdrawResultState(BankAccountStateMachineState bankAccountStateMachineState)
                {
                    _bankAccountState = bankAccountStateMachineState;
                }

                public static explicit operator BankAccountStateMachineState(ActiveWithdrawResultState _)
                    => _._bankAccountState;

                private static class ChoiceTypes
                {
                    // ReSharper disable MemberHidesStaticFromOuterClass
                    public class ActiveState : ActiveWithdrawResultState
                    {
                        public ActiveState() : base(BankAccountStateMachineState.ActiveStateMachineState)
                        {
                        }
                    }

                    public class OverdrawnState : ActiveWithdrawResultState
                    {
                        public OverdrawnState() : base(BankAccountStateMachineState.OverdrawnStateMachineState)
                        {
                        }
                    }

                    public class ZeroBalanceState : ActiveWithdrawResultState
                    {
                        public ZeroBalanceState() : base(BankAccountStateMachineState.ZeroBalanceStateMachineState)
                        {
                        }
                    }

                    // ReSharper restore MemberHidesStaticFromOuterClass
                }
            }

            public class ActiveWithdrawResult :
                StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>.StateTransitionResult
                    <ActiveWithdrawResultState>
            {
                public ActiveWithdrawResult(
                    BankAccountStateMachineData stateMachineData,
                    ActiveWithdrawResultState stateMachineState) : base(stateMachineData, stateMachineState)
                {
                }

                public static explicit operator BankAccountStateMachineGrainState(ActiveWithdrawResult result)
                    =>
                        new BankAccountStateMachineGrainState(result.StateMachineData,
                            (BankAccountStateMachineState) result.StateMachineState);
            }
        }
    }

    public partial class BankAccountStateMachineGrain
    {
        private partial class ActiveStateMessageHandler
        {
            public Task<ActiveDepositResult> Deposit(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount)
            {
                var newBalance = state.StateMachineData.Balance.Deposit(amount);

                var stateMachineData = BankAccountStateMachineData.NewBalance(newBalance);
                var stateMachineState = ActiveDepositResultState.ActiveState;

                return Task.FromResult(new ActiveDepositResult(stateMachineData, stateMachineState));
            }

            public Task<ActiveWithdrawResult> Withdraw(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount)
            {
                var newBalance = state.StateMachineData.Balance.Withdraw(amount);

                var stateMachineData = BankAccountStateMachineData.NewBalance(newBalance);

                var stateMachineState = newBalance.Match(() => ActiveWithdrawResultState.ZeroBalanceState,
                    _ => ActiveWithdrawResultState.ActiveState,
                    _ => ActiveWithdrawResultState.OverdrawnState);

                return Task.FromResult(new ActiveWithdrawResult(stateMachineData, stateMachineState));
            }
        }
    }

    #endregion

    #region OverdrawnState

    public partial class BankAccountStateMachineGrain
    {
        private static async Task<BankAccountStateMachineGrainState> OverdrawnStateProcessor(
            BankAccountStateMachineGrainState state,
            BankAccountStateMachineMessage message)
            =>
                await
                    message.Match(
                        HandleInvalidMessage,
                        OverdrawnStateMessageDelegator.HandleOverdrawnStateDepositMessage(state),
                        HandleInvalidMessage);

        private static class OverdrawnStateMessageDelegator
        {
            private static readonly IOverdrawnStateMessageHandler Handler = new OverdrawnStateMessageHandler();

            public static Func<BankAccountStateMachineAmount, Task<BankAccountStateMachineGrainState>>
                HandleOverdrawnStateDepositMessage(BankAccountStateMachineGrainState state)
            {
                return async _ =>
                {
                    var result = await Handler.Deposit(state, _);
                    return (BankAccountStateMachineGrainState) result;
                };
            }
        }

        private interface IOverdrawnStateMessageHandler
        {
            Task<OverdrawnStateMessageHandler.OverdrawnDepositResult> Deposit(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount);
        }

        private partial class OverdrawnStateMessageHandler : IOverdrawnStateMessageHandler
        {
            internal abstract class OverdrawnDepositResultState
            {
                public static readonly OverdrawnDepositResultState ActiveState = new ChoiceTypes.ActiveState();
                public static readonly OverdrawnDepositResultState OverdrawnState = new ChoiceTypes.OverdrawnState();
                public static readonly OverdrawnDepositResultState ZeroBalanceState = new ChoiceTypes.ZeroBalanceState();

                private readonly BankAccountStateMachineState _bankAccountState;

                private OverdrawnDepositResultState(BankAccountStateMachineState bankAccountStateMachineState)
                {
                    _bankAccountState = bankAccountStateMachineState;
                }

                public static explicit operator BankAccountStateMachineState(OverdrawnDepositResultState _)
                    => _._bankAccountState;

                private static class ChoiceTypes
                {
                    // ReSharper disable MemberHidesStaticFromOuterClass
                    public class ActiveState : OverdrawnDepositResultState
                    {
                        public ActiveState() : base(BankAccountStateMachineState.ActiveStateMachineState)
                        {
                        }
                    }

                    public class OverdrawnState : OverdrawnDepositResultState
                    {
                        public OverdrawnState() : base(BankAccountStateMachineState.OverdrawnStateMachineState)
                        {
                        }
                    }

                    public class ZeroBalanceState : OverdrawnDepositResultState
                    {
                        public ZeroBalanceState() : base(BankAccountStateMachineState.ZeroBalanceStateMachineState)
                        {
                        }
                    }

                    // ReSharper restore MemberHidesStaticFromOuterClass
                }
            }

            public class OverdrawnDepositResult :
                StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>.StateTransitionResult
                    <OverdrawnDepositResultState>
            {
                public OverdrawnDepositResult(
                    BankAccountStateMachineData stateMachineData,
                    OverdrawnDepositResultState stateMachineState) : base(stateMachineData, stateMachineState)
                {
                }

                public static explicit operator BankAccountStateMachineGrainState(OverdrawnDepositResult result)
                    =>
                        new BankAccountStateMachineGrainState(result.StateMachineData,
                            (BankAccountStateMachineState) result.StateMachineState);
            }
        }
    }

    public partial class BankAccountStateMachineGrain
    {
        private partial class OverdrawnStateMessageHandler
        {
            public Task<OverdrawnDepositResult> Deposit(BankAccountStateMachineGrainState state,
                BankAccountStateMachineAmount amount)
            {
                var newBalance = state.StateMachineData.Balance.Deposit(amount);

                var stateMachineData = BankAccountStateMachineData.NewBalance(newBalance);

                var stateMachineState = newBalance.Match(() => OverdrawnDepositResultState.ZeroBalanceState,
                    _ => OverdrawnDepositResultState.ActiveState,
                    _ => OverdrawnDepositResultState.OverdrawnState);

                return Task.FromResult(new OverdrawnDepositResult(stateMachineData, stateMachineState));
            }
        }
    }

    #endregion

    #region ClosedState

    public partial class BankAccountStateMachineGrain
    {
        private static async Task<BankAccountStateMachineGrainState> ClosedStateProcessor(
            BankAccountStateMachineGrainState state,
            BankAccountStateMachineMessage message)
            =>
                await
                    message.Match(
                        HandleInvalidMessage,
                        HandleInvalidMessage,
                        HandleInvalidMessage);
    }

    #endregion
}