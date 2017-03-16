using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace AnonymousCurrency.DataBaseModels
{
    [Table("AC_UsedChecks")]
    public class UsedCheck : DataBaseModel
    {
        public byte[] KnownSecret { get; set; }
    }
}