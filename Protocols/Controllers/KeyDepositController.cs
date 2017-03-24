using System;
using System.Linq;
using KeyDeposit.Workers;
using System.Web.Mvc;
using KeyDeposit.DataBaseModels;
using KeyDeposit.Enums;
using Newtonsoft.Json;
using DataBase = Core.Workers.DataBase<KeyDeposit.Workers.KeyDepositContext>;

namespace Currency.Controllers
{
    public class KeyDepositController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string LoadKeys(KeyKeeper keeper)
        {
            try
            {
                using (var context = new KeyDepositContext())
                {
                    var keys = context
                                    .Keys
                                    .Where(k => k.Keeper == keeper)
                                    .Select(k => k.Id)
                                    .ToArray();

                    return JsonConvert.SerializeObject(keys);
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string GetKeyContainer(Guid id)
        {
            try
            {
                var keyContainer = DataBase.Read<KeyContainer>(id);
                return JsonConvert.SerializeObject(keyContainer);
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }
    }
}