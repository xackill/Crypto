using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;

namespace Core.Currency.Factories
{
    public static class WalletFactory
    {
        public static Wallet Create()
        {
            using (var cryptography = new RSACryptography())
            {
                return new Wallet
                {
                    Id = Guid.NewGuid(),
                    PublicPrivateKey = cryptography.PublicPrivateKey
                };
            }
        }
    }
}
