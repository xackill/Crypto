using System.ComponentModel.DataAnnotations.Schema;
using Core.DataBaseModels;

namespace AnonymousCurrency.DataBaseModels
{
    [Table("AC_BankCustomers")]
    public class BankCustomer : DataBaseModel
    {
        public string NickName { get; set; }
        public int Balance { get; set; }
    }
}
