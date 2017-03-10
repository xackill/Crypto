using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace DistributedCurrency.DataBaseModels
{
    [Table("DC_Wallets")]
    public class Wallet : DataBaseModel
    {
        public byte[] PublicPrivateKey { get; set; }
    }
}