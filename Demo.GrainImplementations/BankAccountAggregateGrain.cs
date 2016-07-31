using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans.Providers;
using Patterns.Aggregates.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    [StorageProvider(ProviderName = "EventStore")]
    public class BankAccountAggregateGrain :
        LazilyComputedAggregateGrain
            <IBankAccountGrain, BankAccountOperation, BankAccountState, BankAccountAggregateBalance>,
        IBankAccountAggregateGrain
    {
    }
}