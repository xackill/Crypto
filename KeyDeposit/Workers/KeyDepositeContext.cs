using System.Data.Entity;
using Core.Workers;
using KeyDeposit.DataBaseModels;

namespace KeyDeposit.Workers
{
    public class KeyDepositeContext : Context
    {
        public DbSet<KeyContainer> Keys { get; set; }
    }
}