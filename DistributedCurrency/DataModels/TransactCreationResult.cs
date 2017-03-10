using System;
using DistributedCurrency.DataBaseModels;

namespace DistributedCurrency.DataModels
{
    public class TransactCreationResult
    {
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }

        public Guid TransactId { get; set; }

        public TransactCreationResult(Exception ex)
        {
            Success = false;
            ErrorMsg = ex.Message;
        }

        public TransactCreationResult(Transaction trans)
        {
            Success = true;
            TransactId = trans.Id;
        }
    }
}