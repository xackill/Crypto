using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;

namespace Core.Currency.Factories
{
    public static class ContactsFactory
    {
        public static Contact Create(Wallet wallet)
        {
            using (var csp = new RSACryptography(wallet.PublicPrivateKey))
                return Create(wallet.Id, wallet.OwnerName, csp.PublicKey);
        }

        public static Contact Create(Guid id, string name, byte[] publicKey)
            => new Contact {Id = id, Name = name, PublicKey = publicKey};
    }
}
