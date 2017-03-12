using System;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using Core;
using Core.Cryptography;
using DataBase = Core.Workers.DataBase<AnonymousCurrency.Workers.AnonymousCurrencyContext>;

namespace AnonymousCurrency.Workers
{
    public class Bank : IDisposable
    {
        private readonly RSACryptography csp;

        public Bank()
        {
            csp = new RSACryptography(Secret.BankPublicPrivateKey);
        }

        public void Dispose()
        {
            csp.Dispose();
        }

        public Guid RegisterCustomer(string nickname)
        {
            var bankCustomer = new BankCustomer {Id = Guid.NewGuid(), NickName = nickname};
            DataBase.Write(bankCustomer);
            return bankCustomer.Id;
        }

        public SignEnvelopeOperation StartSignEnvelopeOperation(CustomerApplication application)
        {
            return new SignEnvelopeOperation(csp, application);
        }
    }
}
