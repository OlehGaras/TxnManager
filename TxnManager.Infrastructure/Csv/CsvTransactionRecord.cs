using System;

namespace TxnManager.Infrastructure.Csv
{
    public class CsvTransactionRecord
    {
        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public CsvTransactionStatus Status { get; set; }
    }
}
