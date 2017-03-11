using System.Data.Entity;
using AnonymousCurrency.DataBaseModels;
using Core.Workers;

namespace AnonymousCurrency.Workers
{
    public class AnonymousCurrencyContext : Context
    {
        public DbSet<SignedEnvelope> Envelopes { get; set; }
        public DbSet<UsedCheck> UsedChecks { get; set; }
        public DbSet<BankCustomer> BankCustomers { get; set; }
    }
}
