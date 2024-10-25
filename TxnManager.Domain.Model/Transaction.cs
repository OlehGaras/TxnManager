using System;
using System.Globalization;
using System.Linq;


namespace TxnManager.Domain.Model
{
    public class Transaction
    {
        public string TransactionId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public DateTime TransactionDate { get; }
        public TransactionStatus Status { get; }

        // Domain model should always be in valid state, so we throw in case of some business rule violation
        public Transaction(string transactionId, decimal amount, string currency,
            DateTime transactionDate, TransactionStatus status)
        {
            var iso4217CurrencySymbols = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct()
                .ToList();

            var details = $"{transactionId}|{amount}|{currency}|{transactionDate}|{status}";
            if (string.IsNullOrEmpty(transactionId))
            {
                throw new BusinessRuleValidationException($"Value for {nameof(TransactionId)} property should be not empty.",
                    details);
            }

            if (transactionId.Length > 50)
            {
                throw new BusinessRuleValidationException($"Value for {nameof(TransactionId)} property is too long.",
                    details);
            }

            if (string.IsNullOrEmpty(currency))
            {
                throw new BusinessRuleValidationException($"Value for {nameof(CurrencyCode)} should not be empty.",
                    details);
            }

            if (!iso4217CurrencySymbols.Contains(currency))
            {
                throw new BusinessRuleValidationException($"Value for {nameof(CurrencyCode)} is not proper iso4217 currency code.",
                    details);
            }

            TransactionId = transactionId;
            Amount = amount;
            CurrencyCode = currency;
            TransactionDate = transactionDate;
            Status = status;
        }
    }
}
