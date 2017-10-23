using System;
using System.Linq;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Extensions;
using AnonymousCurrency.Helpers;
using Core.Cryptography;

namespace AnonymousCurrency.Workers
{
    public class PayOperation
    {
        public int SecretNumber { get; }
        public SellerCheckingEnvelope Envelope { get; }

        public PayOperation(SellerCheckingEnvelope envelope)
        {
            Envelope = envelope;
            SecretNumber = new Random().Next(ACSecret.SecretsCount);
        }

        public SignedEnvelope Exec(byte[] encryptedSecret)
        {
            var secretSign = Envelope.EnumerateSecretsSigns().ElementAt(SecretNumber);

            using (var bankSignChecker = new BankSignChecker())
            {
                if (!bankSignChecker.VerifySign(Envelope.EncryptedContent, Envelope.EncryptedContentSign))
                    throw new Exception("Подпись содержимого конверта подделана!");
                if (!bankSignChecker.VerifySign(encryptedSecret, secretSign))
                    throw new Exception("Подпись секрета подделана!");
            }

            using (var csp = new RSACryptography(Envelope.PublicPrivateKey))
            {
                var dContent = CryptoConverter.Decrypt<EnvelopeContent>(Envelope.EncryptedContent, csp);
                var dSecret = CryptoConverter.Decrypt<EnvelopeSecret>(encryptedSecret, csp);

                if (dContent.Id != dSecret.Id)
                    throw new Exception("Передан секрет другого конверта!");
            }

            return new SignedEnvelope
            {
                Id = Guid.NewGuid(),
                State = EnvelopeState.Opened,

                EncryptedContent = Envelope.EncryptedContent,
                EncryptedContentSign = Envelope.EncryptedContentSign,

                EncryptedSecrets = encryptedSecret,
                EncryptedSecretsSigns = secretSign,

                PublicPrivateKey = Envelope.PublicPrivateKey,
            };
        }
    }
}