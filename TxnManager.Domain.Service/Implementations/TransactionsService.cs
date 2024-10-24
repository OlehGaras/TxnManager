using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TxnManager.Data.Abstractions;
using TxnManager.Domain.Model;
using TxnManager.Domain.Service.Abstractions;
using TxnManager.Domain.Service.Filters;

namespace TxnManager.Domain.Service.Implementations
{
    public class TransactionsService: ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ILogger<TransactionsService> _logger;

        public TransactionsService(ITransactionsRepository transactionsRepository,
            ILogger<TransactionsService> logger)
        {
            _transactionsRepository = transactionsRepository;
            _logger = logger;
        }

        public async Task UpsertTransactionsAsync(List<Transaction> transactions,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var duplicates = transactions
                    .GroupBy(transaction => transaction.TransactionId)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key)
                    .ToList();

                if (duplicates.Any())
                {
                    throw new BusinessRuleValidationException($"Duplicated transaction Ids found: {string.Join(',', duplicates)}");
                }

                await _transactionsRepository.UpsertTransactionsAsync(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,
                    $"Error occured during upserting transactions: {nameof(TransactionsService)}.");
                throw;
            }
        }

        public async Task<List<Transaction>> GetAllAsync(TransactionsFilter transactionsFilter = null)
        {
            try
            {
                // filter is empty => return all
                if (transactionsFilter == null)
                {
                    return await _transactionsRepository.GetAllAsync();
                }

                // filter has non existed currency => return empty list
                if (!TryParseTransactionStatus(transactionsFilter.Status, out TransactionStatus? status))
                {
                    return new List<Transaction>();
                }

                List<Transaction> transactions;

                // filter does not have correct range => don't filter by range
                if (transactionsFilter.Range == null || !transactionsFilter.Range.IsValid)
                {
                    transactions = await _transactionsRepository.GetAllByFiltersAsync(transactionsFilter.Currency, status);
                }
                // filter has correct range
                else
                {
                    transactions = await _transactionsRepository.GetAllByFiltersAsync(transactionsFilter.Currency, status,
                        transactionsFilter.Range.From, transactionsFilter.Range.To);
                }

                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured getting transactions by filter. Filter: {transactionsFilter}");
                throw;
            }
        }

        private bool TryParseTransactionStatus(string status, out TransactionStatus? parsedStatus)
        {
            parsedStatus = null;
            if (string.IsNullOrEmpty(status))
            {
                return true;
            }

            if (Enum.TryParse(typeof(TransactionStatus), status, true, out object transactionStatus))
            {
                parsedStatus = (TransactionStatus)transactionStatus;
                return true;
            }

            return false;
        }
    }
}