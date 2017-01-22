using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Currency.Exceptions;
using Core.Currency.Factories;
using Core.Currency.Workers;

namespace Currency.Controllers
{
    public class CurrencyController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new CurrencyContext())
            {
                var walletsIds = context.Wallets.Select(w => w.Id).ToArray();
                ViewBag.RandomGuid = walletsIds.GetRandomElement();
            }

            return View();
        }

        public ActionResult NewWallet()
        {
            return View();
        }

        public Guid CreateWallet(string surname, string name)
        {
            var wallet = WalletFactory.Create($"{surname} {name}");
            var contact = ContactsFactory.Create(wallet);

            using (var context = new CurrencyContext())
            {
                context.Wallets.Add(wallet);
                context.Contacts.Add(contact);
                context.SaveChanges();
            }

            return wallet.Id;
        }

        public ActionResult Work(Guid id)
        {
            ViewBag.Guid = id;
            return View();
        }
    }
}