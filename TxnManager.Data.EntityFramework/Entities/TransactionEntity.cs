using System;
using System.ComponentModel.DataAnnotations;

namespace TxnManager.Data.EntityFramework.Entities
{
    public class TransactionEntity
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
