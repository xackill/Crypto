using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Currency.DataBaseModels;
using Core.Currency.Exceptions;
using Core.Currency.Factories;
using Core.Currency.Workers;
using WebGrease.Css.Extensions;
using Core.Currency.Extensions;

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
                ViewBag.ContactId = contact.Id;
                ViewBag.ContactName = contact.Name;
            }

            return View();
        }
    }
}