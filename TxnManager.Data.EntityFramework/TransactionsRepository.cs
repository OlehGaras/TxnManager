using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TxnManager.Data.Abstractions;
using TxnManager.Data.EntityFramework.Entities;
using TxnManager.Domain.Model;

namespace TxnManager.Data.EntityFramework
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IRepository<TransactionEntity> _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionsRepository(IRepository<TransactionEntity> transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            var entities = await _transactionRepository.All().ToListAsync();
            return _mapper.Map<List<Transaction>>(entities);
        }

        public async Task SaveTransactionsAsync(List<Transaction> transactions)
        {
            var entities = _mapper.Map<List<TransactionEntity>>(transactions);
            await _transactionRepository.AddRangeAsync(entities);
        }

        public async Task UpsertTransactionsAsync(List<Transaction> transactions)
        {
            var transactionIds = transactions.Select(transaction => transaction.TransactionId).ToList();

            var transactionIdsToUpdate = _transactionRepository
                .All(entity => transactionIds.Contains(entity.Id))
                .Select(entity => entity.Id)
                .ToList();

            if (transactionIdsToUpdate.Any())
            {
                var transactionsToUpdate = transactions
                    .Where(t => transactionIdsToUpdate.Contains(t.TransactionId))
                    .ToList();

                var entitiesToUpdate = _mapper.Map<List<TransactionEntity>>(transactionsToUpdate);
                await _transactionRepository.UpdateRange(entitiesToUpdate);
            }

            var transactionIdsToAdd = transactionIds.Except(transactionIdsToUpdate);
            var transactionsToAdd =
                transactions
                    .Where(transaction => transactionIdsToAdd.Contains(transaction.TransactionId))
                    .ToList();

            await SaveTransactionsAsync(transactionsToAdd);
        }

        public async Task<List<Transaction>> GetAllByFiltersAsync(string currency,
            TransactionStatus? status = null, DateTime? from = null, DateTime? to = null)
        {
            var query = _transactionRepository.All();

            if (!string.IsNullOrEmpty(currency))
            {
                query = query.Where(entity => entity.CurrencyCode == currency);
            }

            if (status.HasValue)
            {
                query = query.Where(entity => entity.Status == (int)status.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(entity => entity.TransactionDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(entity => entity.TransactionDate <= to.Value);
            }

            var entities = await query.ToListAsync();

            return _mapper.Map<List<Transaction>>(entities);
        }
    }
}