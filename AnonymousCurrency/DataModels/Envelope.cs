using System;

namespace AnonymousCurrency.DataModels
{
    public class Envelope : IEnvelope
    {
        public Guid OwnerId { get; set; }
        public byte[] EncryptedContent { get; set; }
        public byte[] EncryptedSecrets { get; set; }
    }
}