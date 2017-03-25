using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Drawing;
using System.Linq;
using System.Numerics;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Factories;
using AnonymousCurrency.Helpers;
using AnonymousCurrency.Workers;
using Core;
using Core.Cryptography;
using Core.Extensions;
using Core.Workers;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Factories;
using DistributedCurrency.Workers;
using KeyDeposit.DataBaseModels;
using KeyDeposit.DataModels;
using KeyDeposit.Workers;
using Newtonsoft.Json;
using ProbabilisticEncryption.Workers;
using VisualAuthentication.DataModels;
using VisualAuthentication.Extensions;
using VisualAuthentication.Factories;

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

        public static void CreateEnvelope()
        {
            using (var bank = new Bank())
            {
                var id = Guid.Parse("B079B377-C4DB-431E-B9E2-341BA628FE66");

                var range = Enumerable.Range(0, Secret.EnvelopeSignCount).ToArray();
                var csps = range.Select(_ => new RSACryptography()).ToArray();
                var envelopes = csps.Select(csp => EnvelopeFactory.CreateBankCheckingEnvelope(csp, id, 10)).ToArray();

                var application = new CustomerApplication
                {
                    CustomerId = id,
                    Balance = 10,
                    Envelopes = envelopes
                };

                var operation = bank.StartSignEnvelopeOperation(application);
                var publicPrivateKeys = operation.EnvelopesToCheck.Select(i => csps[i]).ToArray();
                var signedEnvelope = operation.CheckAndSignEnvelope(publicPrivateKeys);

                signedEnvelope.PublicPrivateKey = csps[operation.EnvelopeToSign].PublicPrivateKey;
                signedEnvelope.State = EnvelopeState.Opened;
                DataBase<AnonymousCurrencyContext>.Write(signedEnvelope);
            }
        }

        public static void Main()
        {
//            BigInteger max;
//            using (new ConsoleMonitoring("Генерация числа"))
//                max = IntFactory.GenerateRandom();

            BBSGenerator g;
            using (new ConsoleMonitoring("Создание BBS генератора"))
                g = BBSGeneratorFactory.CreateNew();

            Console.WriteLine($"P == {g.P};  Q == {g.Q}");

            //KeySource keySource;
            //
            //using (new ConsoleMonitoring("Генерация исходников"))
            //keySource = KeySourceFactory.Generate();
            //
            //KeyContainer[] trustedKeys;
            //using (new ConsoleMonitoring("Генерация ключей для доверенных центров"))
            //trustedKeys = KeyContainersFactory.CreateForTrustedCenters(keySource);
            //
            //KeyContainer creatorKey;
            //using (new ConsoleMonitoring("Генерация ключей для создателя"))
            //creatorKey = KeyContainersFactory.CreateForCreator(trustedKeys);
            //
            //KeyContainer depositKey;
            //using (new ConsoleMonitoring("Генерация ключей для центра депонирования"))
            //depositKey = KeyContainersFactory.CreateForDepositCenter(creatorKey);
            //
            //KeyContainer stateKey;
            //using (new ConsoleMonitoring("Генерация ключей для государства"))
            //stateKey = KeyContainersFactory.CreateForState(trustedKeys);
            //
            //using (new ConsoleMonitoring("Запись в БД"))
            //{
            //var keys = new[] { creatorKey, depositKey, stateKey }.Concat(trustedKeys);
            //DataBase<KeyDepositContext>.Write(keys);
            //}

            //            CreateEnvelope();

            //            var bmp = PictureDrawer.Draw();
            //            bmp.Save(@"C:\work\a.bmp");

            //            for (;;)
            //            {
            //                var contentId = Guid.NewGuid();
            //                var ownerId = Guid.NewGuid();
            //
            //                var s = EnvelopeSecretHelper.GenerateSecret(ownerId, contentId);
            //                Console.WriteLine(EnvelopeSecretHelper.IsSecretValid(s, ownerId, contentId));
            //                var es = s.ExtremelySerialize();
            //                var ds = new EnvelopeSecret();
            //                ds.InitByDeserializing(es);
            //                Console.WriteLine(EnvelopeSecretHelper.IsSecretValid(ds, ownerId, contentId));
            //
            //                Console.ReadKey();
            //            }

            //            var b = new BigInteger(100);
            //            Console.WriteLine(b);
            //            var bytes = b.ToByteArray();
            //            var c = new BigInteger(bytes);
            //            Console.WriteLine(c);
            //var ent = new EnvelopeContent { Id = Guid.NewGuid(), Balance = int.MaxValue };
            //Console.WriteLine($"Id = {ent.Id}; Balance = {ent.Balance}");
            //
            //using (var csp = new RSACryptography())
            //{
            //    var enc = CryptoConverter.Encrypt(ent, csp);
            //    var dent = CryptoConverter.Decrypt<EnvelopeContent>(enc, csp);
            //    Console.WriteLine($"Id = {ent.Id}; Balance = {ent.Balance}");
            //}

            //CreateMajorWallets();
            //CreateFirstTransaction();
        }
    }
}
