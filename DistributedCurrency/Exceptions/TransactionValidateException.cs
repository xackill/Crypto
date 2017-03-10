using System;

namespace DistributedCurrency.Exceptions
{
    public class TransactionValidateException : Exception
    {
        public TransactionValidateException(string message)
            : base($"Ошибка целостности: {message}")
        {
        }
    }
}