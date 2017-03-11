using System;
using AnonymousCurrency.DataBaseModels;
using Core;
using Core.Cryptography;

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
            using (var context = new AnonymousCurrencyContext())
                context.Write(bankCustomer);

            return bankCustomer.Id;
        }


    }
}
