using System;

namespace AnonymousCurrency.DataModels
{
    public class CustomerApplication
    {
        public Guid CustomerId { get; set; }
        public int Balance { get; set; }
        public Envelope[] Envelopes { get; set; }
    }
}