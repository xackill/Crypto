using System;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Helpers;
using Core;
using Core.Cryptography;
using DataBase = Core.Workers.DataBase<AnonymousCurrency.Workers.AnonymousCurrencyContext>;

namespace AnonymousCurrency.Workers
{
    public class Bank : IDisposable
    {
        private readonly RSACryptography csp;

        public Bank()
        {
            csp = new RSACryptography(ACSecret.BankPublicPrivateKey);
        }

        public void Dispose()
        {
            csp.Dispose();
        }

        public Guid RegisterCustomer(string nickname)
        {
            var bankCustomer = new BankCustomer {Id = Guid.NewGuid(), NickName = nickname};
            DataBase.Write(bankCustomer);
            return bankCustomer.Id;
        }

        public SignEnvelopeOperation StartSignEnvelopeOperation(CustomerApplication application)
        {
            return new SignEnvelopeOperation(csp, application);
        }

        public bool VerifySign(byte[] data, byte[] sign)
        {
            return csp.VerifySign(data, sign);
        }

        public int AddDeposite(SignedEnvelope envelope)
        {
            if (!VerifySign(envelope.EncryptedContent, envelope.EncryptedContentSign))
                throw new Exception("Подпись содержимого конверта подделана!");
            if (!VerifySign(envelope.EncryptedSecrets, envelope.EncryptedSecretsSigns))
                throw new Exception("Подпись секрета подделана!");

            using (var ccsp = new RSACryptography(envelope.PublicPrivateKey))
            {
                var dContent = CryptoConverter.Decrypt<EnvelopeContent>(envelope.EncryptedContent, ccsp);
                var dSecret = CryptoConverter.Decrypt<EnvelopeSecret>(envelope.EncryptedSecrets, ccsp);

                if (dContent.Id != dSecret.Id)
                    throw new Exception("Передан секрет другого конверта!");

                var prevUsings = DataBase.Find<UsedCheck>(dContent.Id);
                if (prevUsings == null)
                {
                    DataBase.Write(new UsedCheck {Id = dContent.Id, KnownSecret = dSecret.ExtremelySerialize()});
                    var customer = DataBase.Read<BankCustomer>(envelope.OwnerId);
                    customer.Balance += dContent.Balance;
                    DataBase.Update(customer);
                    return dContent.Balance;
                }

                var prevSecret = new EnvelopeSecret();
                prevSecret.InitByDeserializing(prevUsings.KnownSecret);

                if (prevSecret.Equals(dSecret))
                {
                    var greedy = DataBase.Read<BankCustomer>(envelope.OwnerId);
                    throw new Exception($"Пользователь {greedy.NickName} дважды принес один и тот же конверт!");
                }
                else
                {
                    var greedyId = EnvelopeSecretHelper.RevealThePerson(prevSecret, dSecret);
                    var greedy = DataBase.Read<BankCustomer>(greedyId);
                    throw new Exception($"Пользователь {greedy.NickName} дважды продал один и тот же конверт!");
                }
            }
        }
    }
}
