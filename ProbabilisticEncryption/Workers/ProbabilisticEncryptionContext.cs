using System.Data.Entity;
using Core.Workers;
using ProbabilisticEncryption.DataBaseModels;

namespace ProbabilisticEncryption.Workers
{
    public class ProbabilisticEncryptionContext : Context
    {
        public DbSet<KeyContainer> KeyContainers { get; set; }
        public DbSet<EncryptedMessage> EncryptedMessages { get; set; }
    }
}