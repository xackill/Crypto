using System;

namespace AnonymousCurrency.DataModels
{
    public class BankCheckingEnvelope : IEncryptedSecretsField
    {
        public Guid OwnerId { get; set; }
        public byte[] EncryptedContent { get; set; }
        public byte[] EncryptedSecrets { get; set; }
    }
}