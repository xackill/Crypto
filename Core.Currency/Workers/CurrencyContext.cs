using System.Data.Entity;
using Core.Currency.DataBaseModels;

namespace Core.Currency.Workers
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext() : base(Secret.ConnectionString)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}