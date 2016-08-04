using System;
using System.Threading.Tasks;
using Orleans;

namespace Patterns.StateMachine.Interface
{
    public interface IStateMachineGrain<TStateMachineData, in TStateMachineMessage> : IGrainWithGuidKey
    {
        Task<TStateMachineData> ProcessMessage(TStateMachineMessage message);
    }

    [Serializable]
    public class InvalidMessage : Exception
    {
        public InvalidMessage() : base("Invalid message")
        {
        }

        //public InvalidMessage(TStateMachineMessage message) : base($"Invalid message : {message}")
        //{
        //}
    }

}