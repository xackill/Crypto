using System;
using Core.Cryptography;
using DistributedCurrency.DataBaseModels;

namespace DistributedCurrency.Factories
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
