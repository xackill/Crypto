using System;

namespace AnonymousCurrency.DataModels
{
    public class Envelope
    {
        public Guid OwnerId { get; set; }
        public byte[] EncryptedEnvelopeData { get; set; }
        public byte[] SecretData { get; set; }
    }
}