using System;
using System.Linq;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Helpers;
using Core;
using Core.Cryptography;
using Core.Extensions;

namespace AnonymousCurrency.Factories
{
    public static class EnvelopeFactory
    {
        public static Envelope CreateEnvelope(RSACryptography csp, Guid ownerId, int balance)
        {
            var content = new EnvelopeContent {Id = Guid.NewGuid(), Balance = balance};
            var encryptedContent = CryptoConverter.Encrypt(content, csp);

            var encryptedSecrets = Enumerable
                                    .Range(0, Secret.SecretsCount)
                                    .Select(_ => EnvelopeSecretHelper.GenerateSecret(ownerId))
                                    .Select(s => CryptoConverter.Encrypt(s, csp))
                                    .Aggregate((secrets, nextSecret) => secrets.ConcatBytes(nextSecret));

            return new Envelope
            {
                OwnerId = ownerId,
                EncryptedContent = encryptedContent,
                EncryptedSecrets = encryptedSecrets
            };
        }
    }
}