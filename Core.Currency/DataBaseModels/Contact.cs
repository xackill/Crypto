using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Currency.DataBaseModels
{
    [Table("Contacts")]
    public class Contact : DataBaseModel
    {
        public string Name { get; set; }
        public byte[] PublicKey { get; set; }
    }
}
