using System;
using System.Threading.Tasks;
using Orleans;
using Patterns.StateMachine.Interface;

namespace Patterns.StateMachine.Implementation
{
    public abstract class StateMachineGrain<TGrainState, TStateMachineData, TStateMachineState, TStateMachineMessage> :
        Grain<TGrainState>,
        IStateMachineGrain<TStateMachineData, TStateMachineMessage>
        where TGrainState : StateMachineGrainState<TStateMachineData, TStateMachineState>
        where TStateMachineState : class
    {
        protected static Task<TGrainState> HandleInvalidMessage()
        {
            throw new InvalidMessage();
        }

        public async Task<TStateMachineData> ProcessMessage(TStateMachineMessage message)
        {
            // TODO : provide event-sourcing hooks here for messages and transitions

            // TODO : verify pre-transition conditions here

            // process the message and get the new state            
            var processor = GetProcessorFunc(State.StateMachineState);
            var postTransitionState = await processor(State, message);

            // TODO : verify post-transition conditions here - ensure transition is allowed by the FSM


            State = postTransitionState;
            await WriteStateAsync();

            return await Task.FromResult(State.StateMachineData);
        }

        protected abstract Func<TGrainState, TStateMachineMessage, Task<TGrainState>> GetProcessorFunc(
            TStateMachineState state);

        public class InvalidMessage : Exception
        {
            public InvalidMessage() : base("Invalid message")
            {
            }

            public InvalidMessage(TStateMachineMessage message) : base($"Invalid message : {message}")
            {
            }
        }
    }
}