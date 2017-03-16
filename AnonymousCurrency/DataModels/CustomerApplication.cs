using System;

namespace AnonymousCurrency.DataModels
{
    public class CustomerApplication
    {
        public Guid CustomerId { get; set; }
        public int Balance { get; set; }
        public BankCheckingEnvelope[] Envelopes { get; set; }
    }
}