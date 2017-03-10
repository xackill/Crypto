using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace DistributedCurrency.DataBaseModels
{
    [Table("DC_Transactions")]
    public class Transaction : DataBaseModel
    {
        public short MinerReward { get; set; }

        public Guid SourceId { get; set; }
        public Guid? ExtraSourceId { get; set; }

        public byte[] ReciverPublicKey { get; set; }
        public int Coins { get; set; }

        public byte[] SenderPublicKey { get; set; }
        public int SurplusCoins { get; set; }
        public byte[] SenderSign { get; set; }

        public byte[] VerifierPublicKey { get; set; }
        public byte[] VerifierSign { get; set; }

        public byte[] MinerPublicKey { get; set; }
        public byte ClosingByte { get; set; }

        public byte[] MinerSign { get; set; }
    }
}
