using System;
using System.Runtime.Serialization;
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

        public InvalidMessage(string message, Exception innerException) : base(message, innerException) { }

        protected InvalidMessage(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        //public InvalidMessage(TStateMachineMessage message) : base($"Invalid message : {message}")
        //{
        //}
    }

}