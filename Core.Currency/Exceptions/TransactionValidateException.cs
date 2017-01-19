using System;

namespace Core.Currency.Exceptions
{
    public class TransactionValidateException : Exception
    {
        public TransactionValidateException(string message)
            : base($"Ошибка целостности: {message}")
        {
        }
    }
}