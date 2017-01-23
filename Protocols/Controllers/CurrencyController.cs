using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Core.Currency.DataBaseModels;
using Core.Currency.DataModels;
using Core.Currency.Factories;
using Core.Currency.Workers;
using Core.Currency.Extensions;
using Newtonsoft.Json;

namespace Currency.Controllers
{
    public class CurrencyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewWallet()
        {
            return View();
        }

        public Guid CreateWallet(string surname, string name)
        {
            var wallet = WalletFactory.Create();
            var contact = ContactsFactory.Create(wallet, $"{name} {surname}");

            using (var context = new CurrencyContext())
            {
                context.Wallets.Add(wallet);
                context.Contacts.Add(contact);
                context.SaveChanges();
            }

            return wallet.Id;
        }

        public ActionResult Work(Guid? id)
        {
            using (var context = new CurrencyContext())
            {
                var contact = id.HasValue ? context.Read<Contact>(id.Value) : context.Contacts.GetRandomElement();
                ViewBag.ContactId = contact.Id.ToString("D").ToUpper();
                ViewBag.ContactName = contact.Name;

                var publicKey = contact.PublicKey;
                var base64PublicKey = Convert.ToBase64String(publicKey);
                ViewBag.ContactPublicKey = Regex
                                            .Matches(base64PublicKey, ".{5}")
                                            .Select(m => m.Value)
                                            .JoinStrings(" ");

                ViewBag.TransactionsIds = context
                                            .Transactions
                                            .Where(t => (t.SenderPublicKey == publicKey && t.SurplusCoins > 0) || 
                                                        (t.ReciverPublicKey == publicKey && t.Coins > 0) ||
                                                        (t.MinerPublicKey == publicKey && t.MinerReward > 0))
                                            .Select(t => t.Id)
                                            .ToArray();
            }

            return View();
        }

        public string GetTransaction(Guid id)
        {
            using (var context = new CurrencyContext())
                return JsonConvert.SerializeObject(context.Read<Transaction>(id));
        }

        public string CreateTransfer(string senderPublicKey, string destPublicKey, Guid sourceId, int coins)
        {
            TransactCreationResult result;

            try
            {
                var senderKey = Convert.FromBase64String(senderPublicKey.Replace(" ", ""));
                var distKey = Convert.FromBase64String(destPublicKey.Replace(" ", ""));

                var trans = TransactionFactory.CreateTransfer(senderKey, distKey, sourceId, coins);
                var randomPublicPrivateKey = GetRandomPublicPrivateKeyExcept(senderKey);

                TransactionVerifier.Verify(trans, randomPublicPrivateKey);
                Miner.CloseTransaction(trans, randomPublicPrivateKey);

                using (var context = new CurrencyContext())
                {
                    context.Transactions.Add(trans);
                    context.SaveChanges();
                }

                result = new TransactCreationResult(trans);
            }
            catch (Exception ex)
            {
                result = new TransactCreationResult(ex);
            }

            return JsonConvert.SerializeObject(result);
        }

        public string CreateUnion(string senderPublicKey, Guid sourceId, Guid extraSourceId)
        {
            TransactCreationResult result;

            try
            {
                var senderKey = Convert.FromBase64String(senderPublicKey.Replace(" ", ""));

                var trans = TransactionFactory.CreateUnion(senderKey, sourceId, extraSourceId);
                var randomPublicPrivateKey = GetRandomPublicPrivateKeyExcept(senderKey);

                TransactionVerifier.Verify(trans, randomPublicPrivateKey);
                Miner.CloseTransaction(trans, randomPublicPrivateKey);

                using (var context = new CurrencyContext())
                {
                    context.Transactions.Add(trans);
                    context.SaveChanges();
                }

                result = new TransactCreationResult(trans);
            }
            catch (Exception ex)
            {
                result = new TransactCreationResult(ex);
            }

            return JsonConvert.SerializeObject(result);
        }

        private byte[] GetRandomPublicPrivateKeyExcept(byte[] senderPublicKey)
        {
            using (var context = new CurrencyContext())
            {
                var allPublicPrivateKeysExceptSenderQuery =
                    from c in context.Contacts
                    join w in context.Wallets on c.Id equals w.Id
                    where c.PublicKey != senderPublicKey
                    select w.PublicPrivateKey;

                return allPublicPrivateKeysExceptSenderQuery.GetRandomElement();
            }
        }
    }
}