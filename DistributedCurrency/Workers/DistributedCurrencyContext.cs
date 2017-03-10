using System.Data.Entity;
using Core.Workers;
using DistributedCurrency.DataBaseModels;

namespace DistributedCurrency.Workers
{
    public class DistributedCurrencyContext : Context
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}