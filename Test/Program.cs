using System;
using System.Collections.Generic;
using System.Linq;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Factories;
using DistributedCurrency.Workers;

namespace Test
{
    class Program
    {
        public static string[] forceSensitives =
        {
            "Мейс Винду",
            "Дарт Вейдер",
            "Люк Скайуокер",
            "Дарт Сидиус",
            "Йода",
            "Граф Дуку",
            "Оби-Ван Кеноби",
        };

        public static void CreateMajorWallets()
        {
            var forceSensitivesWallets = new List<Wallet>(forceSensitives.Length);
            var forceSensitivesContacts = new List<Contact>(forceSensitives.Length);
            foreach (var name in forceSensitives)
            {
                var wallet = WalletFactory.Create();
                var contact = ContactsFactory.Create(wallet, name);

                forceSensitivesWallets.Add(wallet);
                forceSensitivesContacts.Add(contact);
            }

            using (var context = new DistributedCurrencyContext())
            {
                context.Wallets.AddRange(forceSensitivesWallets);
                context.Contacts.AddRange(forceSensitivesContacts);
                context.SaveChanges();
            }
        }

        public static byte[] GetPublicPrivateKeyByName(DistributedCurrencyContext context, string name)
        {
            var query = from c in context.Contacts
                        join w in context.Wallets on c.Id equals w.Id
                        where c.Name == name
                        select w.PublicPrivateKey;
            return query.First();
        }

        public static void CreateFirstTransaction()
        {
            using (var context = new DistributedCurrencyContext())
            {
                var Luke = forceSensitives[2];
                var senderPublicKey = context.Contacts.First(c => c.Name == Luke).PublicKey;
                var verifierPublicPrivateKey = GetPublicPrivateKeyByName(context, forceSensitives[1]);
                var minerPublicPrivateKey = GetPublicPrivateKeyByName(context, forceSensitives[4]);

                using (new ConsoleMonitoring("Полный цикл"))
                {
                    Transaction transact;
                    using (new ConsoleMonitoring("Создание транзакции"))
                        transact = TransactionFactory.CreateFirst(senderPublicKey);

                    using (new ConsoleMonitoring("Верификация"))
                        TransactionVerifier.Verify(transact, verifierPublicPrivateKey);

                    using (new ConsoleMonitoring("Закрытие"))
                        Miner.CloseTransaction(transact, minerPublicPrivateKey);

                    using (new ConsoleMonitoring("Запись в базу"))
                    {
                        context.Transactions.Add(transact);
                        context.SaveChanges();
                    }
                }
            }
        }

//        public static void SendCoins()
//        {
//            var sourceId = Guid.Parse("5342BB56-4CF1-4750-922E-B78140F87AED");
//            var JodaWalletId = Guid.Parse("D2969F71-9C1D-4112-9DF4-CD8706DC9F4C");
//            var SidiousWalletId = Guid.Parse("5E703F39-7C22-42E2-8AB6-D59CCDCD15B0");
//            var VaderWalletId = Guid.Parse("9D9580CF-DF75-44F2-BDC0-E5E6CA21800F");
//            var LukeWalletId = Guid.Parse("54583A87-6E67-4C40-A295-0EF181C1F68B");
//
//            using (new ConsoleMonitoring("Полный цикл"))
//            {
//                Transaction transact;
//                using (new ConsoleMonitoring("Создание транзакции"))
//                    transact = TransactionFactory.CreateTransfer(JodaWalletId, SidiousWalletId, sourceId, 40);
//
//                using (new ConsoleMonitoring("Верификация"))
//                    TransactionVerifier.Verify(transact, VaderWalletId);
//
//                using (new ConsoleMonitoring("Закрытие"))
//                    Miner.CloseTransaction(transact, LukeWalletId);
//
//                using (new ConsoleMonitoring("Запись в базу"))
//                using (var context = new DistributedCurrencyContext())
//                {
//                    context.Transactions.Add(transact);
//                    context.SaveChanges();
//                }
//            }
//        }
//
//        public static void UnionFirstCoins()
//        {
//            var sourceId = Guid.Parse("314D2511-4A99-45B0-8010-000599C59F8D");
//            var extraSourceId = Guid.Parse("3152DD69-2D70-4C64-95B5-C4CC06D3E105");
//            var JodaWalletId = Guid.Parse("D2969F71-9C1D-4112-9DF4-CD8706DC9F4C");
//            var SidiousWalletId = Guid.Parse("5E703F39-7C22-42E2-8AB6-D59CCDCD15B0");
//            var VaderWalletId = Guid.Parse("9D9580CF-DF75-44F2-BDC0-E5E6CA21800F");
//
//            using (new ConsoleMonitoring("Полный цикл"))
//            {
//                Transaction transact;
//                using (new ConsoleMonitoring("Создание транзакции"))
//                    transact = TransactionFactory.CreateUnion(SidiousWalletId, sourceId, extraSourceId);
//
//                using (new ConsoleMonitoring("Верификация"))
//                    TransactionVerifier.Verify(transact, JodaWalletId);
//
//                using (new ConsoleMonitoring("Закрытие"))
//                    Miner.CloseTransaction(transact, VaderWalletId);
//
//                using (new ConsoleMonitoring("Запись в базу"))
//                using (var context = new DistributedCurrencyContext())
//                {
//                    context.Transactions.Add(transact);
//                    context.SaveChanges();
//                }
//            }
//        }

        public static void Main()
        {
            CreateMajorWallets();
            CreateFirstTransaction();
        }
    }
}
