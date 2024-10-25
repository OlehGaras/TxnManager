using System;

namespace TxnManager.Domain.Service.Filters
{
    public class DateRangeFilter
    {
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MaxValue;
        public bool IsValid => From <= To;
    }
}