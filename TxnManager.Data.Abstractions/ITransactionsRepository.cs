using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TxnManager.Domain.Model;

namespace TxnManager.Data.Abstractions
{
    public interface ITransactionsRepository
    {
        Task SaveTransactionsAsync(List<Transaction> transaction);
        Task UpsertTransactionsAsync(List<Transaction> transactions);
        Task<List<Transaction>> GetAllAsync();
        Task<List<Transaction>> GetAllByFiltersAsync(string currency, TransactionStatus? status = null,
            DateTime? from = null, DateTime? to = null);
    }
}