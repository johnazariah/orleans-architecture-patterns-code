using System.Threading.Tasks;
using Orleans;

namespace Patterns.StateMachine.Interface
{
    public interface IStateMachineGrain<TStateMachineData, in TStateMachineMessage> : IGrainWithGuidKey
    {
        Task<TStateMachineData> ProcessMessage(TStateMachineMessage message);
    }
}