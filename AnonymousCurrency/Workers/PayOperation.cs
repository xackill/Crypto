using System;
using System.Linq;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Helpers;
using Core;
using Core.Cryptography;
using Core.Extensions;

namespace AnonymousCurrency.Workers
{
    public class PayOperation
    {
        public int SecretNumber { get; }
        public SellerCheckingEnvelope Envelope { get; }

        public PayOperation(SellerCheckingEnvelope envelope)
        {
            Envelope = envelope;
            SecretNumber = new Random().Next(Secret.SecretsCount);
        }

        public SignedEnvelope Exec(MissingByte missingByte, byte[] secretSign)
        {
            var cutSecretBytes = Envelope.EncryptedSecrets.Skip(SecretNumber * 127).Take(127).ToArray();
            var secretBytes = RestoreMissingByte(cutSecretBytes, missingByte);

            using (var bankSignChecker = new BankSignChecker())
            {
                if (!bankSignChecker.VerifySign(Envelope.EncryptedContent, Envelope.EncryptedContentSign))
                    throw new Exception("Подпись содержимого конверта подделана!");
                if (!bankSignChecker.VerifySign(secretBytes, secretSign))
                    throw new Exception("Подпись секрета подделана!");
            }

            using (var csp = new RSACryptography(Envelope.PublicPrivateKey))
            {
                var dContent = CryptoConverter.Decrypt<EnvelopeContent>(Envelope.EncryptedContent, csp);
                var dSecret = CryptoConverter.Decrypt<EnvelopeSecret>(secretBytes, csp);

                if (dContent.Id != dSecret.Id)
                    throw new Exception("Передан секрет другого конверта!");
            }

            return new SignedEnvelope
            {
                Id = Guid.NewGuid(),
                State = EnvelopeState.Opened,

                EncryptedContent = Envelope.EncryptedContent,
                EncryptedContentSign = Envelope.EncryptedContentSign,

                EncryptedSecrets = secretBytes,
                EncryptedSecretsSigns = secretSign,

                PublicPrivateKey = Envelope.PublicPrivateKey,
            };
        }

        private static byte[] RestoreMissingByte(byte[] cutSecretBytes, MissingByte missingByte)
        {
            var fpart = cutSecretBytes.Take(missingByte.Position).ToArray();
            var lpart = cutSecretBytes.Skip(missingByte.Position).ToArray();
            return fpart.ConcatBytes(missingByte.Element).ConcatBytes(lpart);
        }
    }
}