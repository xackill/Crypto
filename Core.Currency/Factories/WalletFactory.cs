using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;

namespace Core.Currency.Factories
{
    public static class WalletFactory
    {
        public static Wallet Create(string ownerName)
        {
            using (var cryptography = new RSACryptography())
            {
                return new Wallet
                {
                    Id = Guid.NewGuid(),
                    OwnerName = ownerName,
                    PublicPrivateKey = cryptography.PublicPrivateKey
                };
            }
        }
    }
}
