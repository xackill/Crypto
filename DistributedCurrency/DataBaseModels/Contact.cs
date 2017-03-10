using Core.DataBaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistributedCurrency.DataBaseModels
{
    [Table("DC_Contacts")]
    public class Contact : DataBaseModel
    {
        public string Name { get; set; }
        public byte[] PublicKey { get; set; }
    }
}
