using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Demo.SmartCache.GrainInterfaces.State;
using Orleans.Providers;
using Patterns.EventSourcing.Implementation;

namespace Demo.SmartCache.GrainImplementations
{
    [StorageProvider(ProviderName = "EventStore")]
    public class BankAccountGrain : EventSourcedGrain<BankAccountOperation, BankAccountState>,
        IBankAccountGrain
    {
        public Task<BankAccountState> CreditAmount(decimal amount)
            => ProcessEvent(BankAccountOperation.NewCredit(amount));

        public Task<BankAccountState> DebitAmount(decimal amount) 
            => ProcessEvent(BankAccountOperation.NewDebit(amount));
    }
}