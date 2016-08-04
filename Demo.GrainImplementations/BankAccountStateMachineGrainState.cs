using System;
using Demo.SmartCache.GrainInterfaces.State;
using Patterns.StateMachine.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    [Serializable]
    public class BankAccountStateMachineGrainState :
        StateMachineGrainState<BankAccountStateMachineData, BankAccountStateMachineState>
    {
        public BankAccountStateMachineGrainState()
            : this(
                BankAccountStateMachineData.NewBalance(BankAccountStateMachineBalance.ZeroBankAccountStateMachineBalance),
                BankAccountStateMachineState.ZeroBalanceStateMachineState)
        {
        }

        public BankAccountStateMachineGrainState(BankAccountStateMachineData stateMachineData,
            BankAccountStateMachineState stateMachineState)
            : base(stateMachineData, stateMachineState)
        {
        }
    }
}