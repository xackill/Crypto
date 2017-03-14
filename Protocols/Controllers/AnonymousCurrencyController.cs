using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Factories;
using AnonymousCurrency.Workers;
using Core;
using Core.Cryptography;
using Core.Extensions;
using Newtonsoft.Json;
using DataBase = Core.Workers.DataBase<AnonymousCurrency.Workers.AnonymousCurrencyContext>;

namespace Currency.Controllers
{
    public class AnonymousCurrencyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewAccount()
        {
            return View();
        }

//        public ActionResult Help()
//        {
//            return View();
//        }

        public Guid CreateAccount(string nickname)
        {
            using (var bank = new Bank())
                return bank.RegisterCustomer(nickname);
        }

        public ActionResult Work(Guid? id)
        {
            using (var context = new AnonymousCurrencyContext())
            {
                var account = id.HasValue ? context.Read<BankCustomer>(id.Value) : context.BankCustomers.GetRandomElement();
                ViewBag.AccountId = account.Id;
                ViewBag.AccountName = account.NickName;
                ViewBag.AccountBalance = account.Balance;

                ViewBag.OpenedEnvelopesIds = context
                                                .Envelopes
                                                .Where(e => e.OwnerId == account.Id && e.State == EnvelopeState.Opened)
                                                .Select(t => t.Id)
                                                .ToArray();

                ViewBag.SealedEnvelopesIds = context
                                                .Envelopes
                                                .Where(e => e.OwnerId == account.Id && e.State == EnvelopeState.Sealed)
                                                .Select(t => t.Id)
                                                .ToArray();
            }

            return View();
        }

        public string GetEnvelope(Guid id)
        {
            return JsonConvert.SerializeObject(DataBase.Read<SignedEnvelope>(id));
        }

        public string CreateSealedEnvelope(Guid userId, int amount, int cheatAmount)
        {
            try
            {
                using (var bank = new Bank())
                {
                    var range = Enumerable.Range(0, Secret.EnvelopeSignCount).ToArray();
                    var cheatIdx = new Random().Next(Secret.EnvelopeSignCount); 
                    var csps = range.Select(i => new {i, csp = new RSACryptography()}).ToArray();
                    var envelopes = csps
                        .Select(csp => EnvelopeFactory.CreateBankCheckingEnvelope(csp.csp, userId, amount + (csp.i == cheatIdx? cheatAmount : 0)))
                        .ToArray();

                    var application = new CustomerApplication
                    {
                        CustomerId = userId,
                        Balance = amount,
                        Envelopes = envelopes
                    };

                    var operation = bank.StartSignEnvelopeOperation(application);
                    var publicPrivateKeys = operation.EnvelopesToCheck.Select(i => csps[i].csp).ToArray();
                    var signedEnvelope = operation.CheckAndSignEnvelope(publicPrivateKeys);

                    signedEnvelope.PublicPrivateKey = csps[operation.EnvelopeToSign].csp.PublicPrivateKey;
                    signedEnvelope.State = EnvelopeState.Sealed;
                    DataBase.Write(signedEnvelope);

                    return $"Успех! Создан конверт {signedEnvelope.Id} с {amount + cheatAmount}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string Pay(Guid userId, Guid envelopeId, Guid reciverId)
        {
            try
            {
                var envelope = DataBase.Read<SignedEnvelope>(envelopeId);
                if (envelope.OwnerId != userId)
                    throw new Exception("Этот конверт Вам не принадлежит!");
                if (envelope.State != EnvelopeState.Sealed)
                    throw new Exception("Тратить можно только закрытые конверты!");

                MissingByte[] missingBytes;
                var envelopeToCheck = EnvelopeFactory.CreateSellerCheckingEnvelope(envelope, out missingBytes);
                var payOperation = new PayOperation(envelopeToCheck);
                var sidx = payOperation.SecretNumber;
                var secretSign = envelope.EncryptedSecretsSigns.Skip(128 * sidx).Take(128).ToArray();
                var newSignedEnvelope = payOperation.Exec(missingBytes[sidx], secretSign);
                newSignedEnvelope.OwnerId = reciverId;
                DataBase.Write(newSignedEnvelope);

                return $"Успех! Конверт потрачен - в {newSignedEnvelope.Id}; узнан секрет {sidx}";
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string Deposite(Guid userId, Guid envelopeId)
        {
            try
            {
                var envelope = DataBase.Read<SignedEnvelope>(envelopeId);
                if (envelope.OwnerId != userId)
                    throw new Exception("Этот конверт Вам не принадлежит!");

                using (var bank = new Bank())
                    return $"Успех! На ваш счет добавлено: {bank.AddDeposite(envelope)}";
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }
    }
}