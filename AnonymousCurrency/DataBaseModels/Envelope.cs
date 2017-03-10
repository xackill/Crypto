using System;
using System.ComponentModel.DataAnnotations.Schema;
using AnonymousCurrency.Enums;
using Core.DataBaseModels;

namespace AnonymousCurrency.DataBaseModels
{
    [Table("AC_Envelopes")]
    public class Envelope : DataBaseModel
    {
        public Guid OwnerId { get; set; }
        public EnvelopeState State { get; set; }

        public byte[] EncryptedEnvelopeData { get; set; }
        public byte[] EncryptedEnvelopeSign { get; set; }

        public byte[] SecretData { get; set; }
        public byte[] SecretSigns { get; set; }

        public byte[] PublicPrivateKey { get; set; }
    }
}
