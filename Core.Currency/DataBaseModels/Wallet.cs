using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Currency.DataBaseModels
{
    [Table("Wallets")]
    public class Wallet : DataBaseModel
    {
        public byte[] PublicPrivateKey { get; set; }
    }
}