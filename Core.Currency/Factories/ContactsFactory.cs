using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;

namespace Core.Currency.Factories
{
    public static class ContactsFactory
    {
        public static Contact Create(Wallet wallet, string ownerName)
        {
            using (var csp = new RSACryptography(wallet.PublicPrivateKey))
                return Create(wallet.Id, ownerName, csp.PublicKey);
        }

        public static Contact Create(Guid id, string name, byte[] publicKey)
            => new Contact {Id = id, Name = name, PublicKey = publicKey};
    }
}
