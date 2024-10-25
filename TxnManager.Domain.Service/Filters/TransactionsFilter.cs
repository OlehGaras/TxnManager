namespace TxnManager.Domain.Service.Filters
{
    public class TransactionsFilter
    {
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateRangeFilter Range { get; set; }

        public override string ToString()
        {
            return $"Currency: {Currency}; Status: {Status}; From: {Range?.From}; To: {Range?.To}";
        }
    }
}