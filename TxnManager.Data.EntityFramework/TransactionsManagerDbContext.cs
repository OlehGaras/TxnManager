using Microsoft.EntityFrameworkCore;
using TxnManager.Data.EntityFramework.Entities;


namespace TxnManager.Data.EntityFramework
{
    public class TransactionsManagerDbContext: DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }

        public TransactionsManagerDbContext()
        {
        }

        public TransactionsManagerDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
