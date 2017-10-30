using System;
using System.Collections.Immutable;
using System.Linq;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Extensions;
using AnonymousCurrency.Helpers;
using Core.Cryptography;
using Core.Extensions;
using DataBase = Core.Workers.DataBase<AnonymousCurrency.Workers.AnonymousCurrencyContext>;

namespace AnonymousCurrency.Workers
{
    public class SignEnvelopeOperation
    {
        private readonly Guid CustomerId;
        private readonly int ApplicationBalance;
        private readonly int EnvelopeToSignIdx;
        private readonly ImmutableArray<int> EnvelopesToOpen;
        private readonly BankCheckingEnvelope[] Envelopes;
        private readonly RSACryptography BankCryptography;

        public SignEnvelopeOperation(RSACryptography bankCryptography, CustomerApplication application)
        {
            BankCryptography = bankCryptography;
            CustomerId = application.CustomerId;
            ApplicationBalance = application.Balance;
            Envelopes = application.Envelopes;

            if (ApplicationBalance == 0)
                throw new Exception("Не может быть создан конверт с 0!");

            if (Envelopes.Length != ACSecret.EnvelopeSignCount)
                throw new Exception($"Конвертов должно быть {ACSecret.EnvelopeSignCount}");

            var customer = DataBase.Read<BankCustomer>(CustomerId);
            if (customer.Balance < application.Balance)
                throw new Exception("Недостаточно средств на счете!");

            EnvelopesToOpen = GenerateEnvelopesNumbers(out EnvelopeToSignIdx);
        }

        public int EnvelopeToSign => EnvelopeToSignIdx;
        public ImmutableArray<int> EnvelopesToCheck => EnvelopesToOpen;

        public SignedEnvelope CheckAndSignEnvelope(RSACryptography[] csps)
        {
            if (csps.Length != ACSecret.EnvelopeSignCount - 1)
                throw new Exception($"Закрытых ключей должно быть {ACSecret.EnvelopeSignCount - 1}");

            for (var i = 0; i < ACSecret.EnvelopeSignCount - 1; ++i)
            {
                var envelope = Envelopes[EnvelopesToOpen[i]];
                ThrowIfEnvelopeCorrupted(envelope, csps[i]);
            }

            UpdateCustomerBalance();
            return SignEnvelope(Envelopes[EnvelopeToSignIdx]);
        }

        private void UpdateCustomerBalance()
        {
            var customer = DataBase.Read<BankCustomer>(CustomerId);
            customer.Balance -= ApplicationBalance;
            DataBase.Update(customer);
        }

        private static ImmutableArray<int> GenerateEnvelopesNumbers(out int exclude)
        {
            var rand = new Random();
            exclude = rand.Next(ACSecret.EnvelopeSignCount);

            return Enumerable
                .Range(0, ACSecret.EnvelopeSignCount)
                .Except(exclude)
                .Shuffle()
                .ToImmutableArray();
        }

        private void ThrowIfEnvelopeCorrupted(BankCheckingEnvelope envelope, RSACryptography csp)
        {
            var content = CryptoConverter.Decrypt<EnvelopeContent>(envelope.EncryptedContent, csp);
            ThrowIfEnvelopeContentCorrupted(content);

            var encryptedSecrets = envelope.EnumerateSecrets().ToArray();
            if (encryptedSecrets.Length != ACSecret.SecretsCount)
                throw new Exception("Обнаружен конверт с недостаточным числом секретов!");
                 
            foreach (var encryptedSecret in encryptedSecrets)
            {
                var secret = CryptoConverter.Decrypt<EnvelopeSecret>(encryptedSecret, csp);
                ThrowIfEnvelopeSecretCorrupted(secret, content.Id);
            }
        }

        private void ThrowIfEnvelopeContentCorrupted(EnvelopeContent content)
        {
            if (content.Balance != ApplicationBalance)
                throw new Exception("Обнаружен конверт с поддельной суммой!");
        }

        private void ThrowIfEnvelopeSecretCorrupted(EnvelopeSecret secret, Guid contentId)
        {
            if (!EnvelopeSecretHelper.IsSecretValid(secret, CustomerId, contentId))
                throw new Exception("Обнаружен конверт с поддельным секретом!");
        }

        private SignedEnvelope SignEnvelope(BankCheckingEnvelope envelope)
        {
            var encryptedContentSign = BankCryptography.Sign(envelope.EncryptedContent);

            var encryptedSecretsSigns = envelope
                .EnumerateSecrets()
                .Select(secret => BankCryptography.Sign(secret))
                .Aggregate((signs, nextSign) => signs.ConcatBytes(nextSign));

            return new SignedEnvelope
            {
                Id = Guid.NewGuid(),
                OwnerId = envelope.OwnerId,
                State = EnvelopeState.Sealed,

                EncryptedContent = envelope.EncryptedContent,
                EncryptedContentSign = encryptedContentSign,

                EncryptedSecrets = envelope.EncryptedSecrets,
                EncryptedSecretsSigns = encryptedSecretsSigns
            };
        }
    }
}
