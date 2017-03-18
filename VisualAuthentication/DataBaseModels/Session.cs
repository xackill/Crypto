using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace VisualAuthentication.DataBaseModels
{
    [Table("VA_Session")]
    public class Session : DataBaseModel
    {
        public string SerializedKeys { get; set; }
        public int SecretKeyNumber { get; set; }
        public int CurrentIteration { get; set; }
        public int CurrentCorrectNumber { get; set; }
        public int FirstErrorIteration { get; set; }
    }
}