using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TxnManager.Domain.Model;
using TxnManager.Domain.Service.Filters;

namespace TxnManager.Domain.Service.Abstractions
{
    public interface ITransactionsService
    {
        Task UpsertTransactionsAsync(List<Transaction> transactions,
            CancellationToken cancellationToken = default);

        Task<List<Transaction>> GetAllAsync(TransactionsFilter transactionsFilter = null);
    }
}
