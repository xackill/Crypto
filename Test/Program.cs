using System;
using System.Collections.Generic;
using System.Linq;
using DistributedCurrency;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Factories;
using DistributedCurrency.Workers;
using DCDataBase = Core.Workers.DataBase<DistributedCurrency.Workers.DistributedCurrencyContext>;

namespace Test
{
    public class Program
    {
        public static Dictionary<string, Guid> Characters = new Dictionary <string, Guid>
        {
            { "Шерлок Холмс", DCSecret.SherlockHolmesWalletId },
            { "Профессор Мориарти", Guid.NewGuid() },
            { "Миссис Хадсон", Guid.NewGuid() },
            { "Джон Ватсон", DCSecret.JohnWatsonWalletId },
            { "Майкрофт Холмс", DCSecret.MycroftHolmesWalletId }
        };

        public static void CreateMajorWallets()
        {
            var charactersWallets = new List<Wallet>(Characters.Count);
            var charactersContacts = new List<Contact>(Characters.Count);
            foreach (var character in Characters)
            {
                var wallet = WalletFactory.Create();
                wallet.Id = character.Value;

                var contact = ContactsFactory.Create(wallet, character.Key);

                charactersWallets.Add(wallet);
                charactersContacts.Add(contact);
            }

            DCDataBase.Write(charactersWallets.AsEnumerable());
            DCDataBase.Write(charactersContacts.AsEnumerable());
        }

        public static void CreateFirstTransaction()
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

        public static void Main()
        {
            CreateMajorWallets();
            CreateFirstTransaction();
        }
    }
}
