using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;
using KeyDeposit.Enums;

namespace KeyDeposit.DataBaseModels
{
    [Table("KD_KeyContainer")]
    public class KeyContainer : DataBaseModel
    {
        public Guid KeyId { get; set; }

        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }
        public byte[] Modulus { get; set; }

        public KeyKeeper Keeper { get; set; }
    }
}