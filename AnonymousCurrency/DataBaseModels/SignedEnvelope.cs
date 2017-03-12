using System;
using System.ComponentModel.DataAnnotations.Schema;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using Core.DataBaseModels;

namespace AnonymousCurrency.DataBaseModels
{
    [Table("AC_Envelopes")]
    public class SignedEnvelope : DataBaseModel, IEnvelope
    {
        public Guid OwnerId { get; set; }
        public EnvelopeState State { get; set; }

        public byte[] EncryptedContent { get; set; }
        public byte[] EncryptedContentSign { get; set; }

        public byte[] EncryptedSecrets { get; set; }
        public byte[] EncryptedSecretsSigns { get; set; }

        public byte[] PublicPrivateKey { get; set; }
    }
}
