using System;
using System.Web.Mvc;
using AnonymousCurrency.DataBaseModels;
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
            var customer = new BankCustomer {Id = Guid.NewGuid(), NickName = nickname};
            DataBase.Write(customer);

            return customer.Id;
        }
    }
}