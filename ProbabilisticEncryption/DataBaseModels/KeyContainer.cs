using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace ProbabilisticEncryption.DataBaseModels
{
    [Table("PE_KeyContainers")]
    public class KeyContainer : DataBaseModel
    {
        public byte[] P { get; set; }
        public byte[] Q { get; set; }
    }
}