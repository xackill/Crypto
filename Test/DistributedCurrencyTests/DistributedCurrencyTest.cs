using System;
using System.Collections.Generic;
using DistributedCurrency;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Factories;
using DistributedCurrency.Workers;
using NUnit.Framework;
using Test.Helpers;
using DCDataBase = Core.Workers.DataBase<DistributedCurrency.Workers.DistributedCurrencyContext>;

namespace Test.DistributedCurrencyTests
{
    public class DistributedCurrencyTest
    {
        [Test]
        [Explicit]
        public void InitDistributedCurrency()
        {
            CreateCharactersWallets();
            CreateFirstTransaction();
        }
        
        private readonly Dictionary<string, Guid> characters = new Dictionary <string, Guid>
        {
            { "Шерлок Холмс", DCSecret.SherlockHolmesWalletId },
            { "Профессор Мориарти", Guid.NewGuid() },
            { "Миссис Хадсон", Guid.NewGuid() },
            { "Джон Ватсон", DCSecret.JohnWatsonWalletId },
            { "Майкрофт Холмс", DCSecret.MycroftHolmesWalletId }
        };

        private void CreateCharactersWallets()
        {
            var charactersWallets = new List<Wallet>(characters.Count);
            var charactersContacts = new List<Contact>(characters.Count);
            foreach (var character in characters)
            {
                var wallet = WalletFactory.Create();
                wallet.Id = character.Value;

                var contact = ContactsFactory.Create(wallet, character.Key);

                charactersWallets.Add(wallet);
                charactersContacts.Add(contact);
            }

            DCDataBase.WriteAll(charactersWallets);
            DCDataBase.WriteAll(charactersContacts);
        }

        private void CreateFirstTransaction()
        {
            var sender = DCDataBase.Read<Contact>(DCSecret.JohnWatsonWalletId);
            var verifier = DCDataBase.Read<Wallet>(DCSecret.MycroftHolmesWalletId);
            var miner = DCDataBase.Read<Wallet>(DCSecret.SherlockHolmesWalletId);

            using (new ConsoleMonitoring("Полный цикл"))
            {
                Transaction transact;
                using (new ConsoleMonitoring("Создание транзакции"))
                    transact = TransactionFactory.CreateFirst(sender.PublicKey);

                using (new ConsoleMonitoring("Верификация"))
                    TransactionVerifier.Verify(transact, verifier.PublicPrivateKey);

                using (new ConsoleMonitoring("Закрытие"))
                    Miner.CloseTransaction(transact, miner.PublicPrivateKey);

                using (new ConsoleMonitoring("Запись в базу"))
                    DCDataBase.Write(transact);
            }
        }
    }
}
