using Demo.SmartCache.GrainInterfaces.State;
using Patterns.Aggregates.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    public interface IBankAccountAggregateGrain :
        IAggregateGrain<IBankAccountGrain, BankAccountOperation, BankAccountState, BankAccountAggregateBalance>
    {
    }
}