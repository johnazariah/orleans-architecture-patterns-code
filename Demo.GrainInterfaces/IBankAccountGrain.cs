using System.Threading.Tasks;
using Demo.SmartCache.GrainInterfaces.State;
using Patterns.EventSourcing.Interface;

namespace Demo.SmartCache.GrainInterfaces
{
    #region BankAccountOperation Event [Discriminated Union Type]

    #endregion

    public interface IBankAccountGrain : IEventSourcedGrain<BankAccountOperation, BankAccountState>
    {
        Task<BankAccountState> CreditAmount(decimal amount);

        Task<BankAccountState> DebitAmount(decimal amount);
    }

}