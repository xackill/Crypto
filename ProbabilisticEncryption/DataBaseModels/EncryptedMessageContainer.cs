using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace ProbabilisticEncryption.DataBaseModels
{
    [Table("PE_EncryptedMessages")]
    public class EncryptedMessageContainer : DataBaseModel
    {
        public byte[] Message { get; set; }
        public byte[] Xt { get; set; }
    }
}