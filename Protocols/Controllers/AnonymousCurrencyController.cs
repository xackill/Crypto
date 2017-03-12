using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.Enums;
using AnonymousCurrency.Workers;
using Core.Extensions;
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
    }
}