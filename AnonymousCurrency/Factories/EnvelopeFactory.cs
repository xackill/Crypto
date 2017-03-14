using System;
using System.Linq;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Extensions;
using AnonymousCurrency.Helpers;
using Core;
using Core.Cryptography;
using Core.Extensions;

namespace AnonymousCurrency.Factories
{
    public static class EnvelopeFactory
    {
        public static BankCheckingEnvelope CreateBankCheckingEnvelope(RSACryptography csp, Guid ownerId, int balance)
        {
            var content = new EnvelopeContent {Id = Guid.NewGuid(), Balance = balance};
            var encryptedContent = CryptoConverter.Encrypt(content, csp);

            var encryptedSecrets = Enumerable
                                    .Range(0, Secret.SecretsCount)
                                    .Select(_ => EnvelopeSecretHelper.GenerateSecret(ownerId, content.Id))
                                    .Select(s => CryptoConverter.Encrypt(s, csp))
                                    .Aggregate((secrets, nextSecret) => secrets.ConcatBytes(nextSecret));

            return new BankCheckingEnvelope
            {
                OwnerId = ownerId,
                EncryptedContent = encryptedContent,
                EncryptedSecrets = encryptedSecrets
            };
        }

        public static SellerCheckingEnvelope CreateSellerCheckingEnvelope(SignedEnvelope envelope, out MissingByte[] missingBytes)
        {
            var rand = new Random();
            missingBytes = Enumerable
                .Range(0, Secret.SecretsCount)
                .Select(i => new MissingByte {Position = rand.Next(128)})
                .ToArray();
                
            var encryptedSecrets = envelope.EnumerateSecrets().ToArray();
            var encryptedSecretsWithMissingBytes = new byte[Secret.SecretsCount * 127];
            for (int i = 0, j = 0; i < Secret.SecretsCount; ++i)
                for (var k = 0; k < 128; ++k)
                {
                    if (k == missingBytes[i].Position)
                        missingBytes[i].Element = encryptedSecrets[i][k];
                    else
                        encryptedSecretsWithMissingBytes[j++] = encryptedSecrets[i][k];
                }

            return new SellerCheckingEnvelope
            {
                EncryptedContent = envelope.EncryptedContent,
                EncryptedContentSign = envelope.EncryptedContentSign,
                EncryptedSecrets = encryptedSecretsWithMissingBytes,
                PublicPrivateKey = envelope.PublicPrivateKey,
            };
        }
    }
}