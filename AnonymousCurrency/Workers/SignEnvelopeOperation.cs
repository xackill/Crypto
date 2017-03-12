using System;
using System.Collections.Immutable;
using System.Linq;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using Core;
using Core.Cryptography;
using Core.Extensions;
using DataBase = Core.Workers.DataBase<AnonymousCurrency.Workers.AnonymousCurrencyContext>;

namespace AnonymousCurrency.Workers
{
    public class SignEnvelopeOperation
    {
        private readonly BankCustomer Creator;
        private readonly int EnvelopeToSign;
        private readonly ImmutableArray<int> EnvelopesToOpen;
        private readonly Envelope[] Envelopes;
        private readonly RSACryptography BankCryptography;

        public SignEnvelopeOperation(RSACryptography bankCryptography, Guid creatorId, Envelope[] envelopesForCheck)
        {
            if (envelopesForCheck.Length != Secret.EnvelopeSignCount)
                throw new Exception($"Конвертов должно быть {Secret.EnvelopeSignCount}");

            Creator = DataBase.Read<BankCustomer>(creatorId);
            BankCryptography = bankCryptography;
            Envelopes = envelopesForCheck;
            EnvelopesToOpen = GenerateEnvelopesNumbers(out EnvelopeToSign);
        }

        public ImmutableArray<int> EnvelopesToCheck => EnvelopesToOpen;

        public SignedEnvelope CheckAndSignEnvelope(byte[][] publicPrivateKeys)
        {
            if (publicPrivateKeys.Length != Secret.EnvelopeSignCount - 1)
                throw new Exception($"Закрытых ключей должно быть {Secret.EnvelopeSignCount - 1}");

            for (var i = 0; i < Secret.EnvelopeSignCount - 1; ++i)
            {
                var envelope = Envelopes[EnvelopesToOpen[i]];
                var publicPrivateKey = publicPrivateKeys[i];
                ThrowIfEnvelopeCorrupted(envelope, publicPrivateKey);
            }

            return SignEnvelope(Envelopes[EnvelopeToSign]);
        }

        private static ImmutableArray<int> GenerateEnvelopesNumbers(out int exclude)
        {
            var rand = new Random();
            exclude = rand.Next(Secret.EnvelopeSignCount);

            return Enumerable
                .Range(0, Secret.EnvelopeSignCount)
                .Except(exclude)
                .Shuffle()
                .ToImmutableArray();
        }

        private static void ThrowIfEnvelopeCorrupted(Envelope envelope, byte[] publicPrivateKey)
        {
        }

        private SignedEnvelope SignEnvelope(Envelope envelope) =>
            new SignedEnvelope
            {
                Id = Guid.NewGuid(),
                OwnerId = envelope.OwnerId,
                State = EnvelopeState.Sealed,

                EncryptedEnvelopeData = envelope.EncryptedEnvelopeData,
                EncryptedEnvelopeSign = BankCryptography.Sign(envelope.EncryptedEnvelopeData),

                SecretData = envelope.SecretData,
                SecretSign = BankCryptography.Sign(envelope.SecretData)
            };
    }
}
