using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces;
using Orleans.Providers;
using Patterns.EventSourcing;

namespace Demo.SmartCache.GrainImplementations
{
    [StorageProvider(ProviderName = "EventStore")]
    public class BankBalanceEventSourcedGrain : EventSourcedGrain<BankingOperation, BankBalance>,
        IBankBalanceEventSourcedGrain
    {
        public Task<BankBalance> CreditAmount(decimal amount) => ProcessEvent(BankingOperation.NewCredit(amount));

        public Task<BankBalance> DebitAmount(decimal amount) => ProcessEvent(BankingOperation.NewDebit(amount));
    }
}