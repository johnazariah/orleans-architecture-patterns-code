using System;

namespace Patterns.StateMachine.Implementation
{
    public class StateMachineGrainState<TStateMachineData, TStateMachineState> :
        Tuple<TStateMachineData, TStateMachineState> where TStateMachineState : class
    {
        public StateMachineGrainState(TStateMachineData stateMachineData, TStateMachineState stateMachineState)
            : base(stateMachineData, stateMachineState)
        {
        }

        public TStateMachineData StateMachineData => Item1;
        public TStateMachineState StateMachineState => Item2;

        public class StateTransitionResult<TTransitionResultState> : Tuple<TStateMachineData, TTransitionResultState>
        {
            public StateTransitionResult(TStateMachineData stateMachineData, TTransitionResultState stateMachineState)
                : base(stateMachineData, stateMachineState)
            {
            }

            public TStateMachineData StateMachineData => Item1;
            public TTransitionResultState StateMachineState => Item2;

            public static explicit operator StateMachineGrainState<TStateMachineData, TStateMachineState>(
                StateTransitionResult<TTransitionResultState> _this)
                =>
                    new StateMachineGrainState<TStateMachineData, TStateMachineState>(
                        _this.StateMachineData,
                        _this.StateMachineState as TStateMachineState);
        }
    }
}