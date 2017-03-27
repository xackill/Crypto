using System;
using System.Linq;
using KeyDeposit.Workers;
using System.Web.Mvc;
using Core.Helpers;
using KeyDeposit.DataBaseModels;
using KeyDeposit.DataModels;
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

        public string CreateKey()
        {
            try
            {
                using (var timeMeasurer = new TimeMeasurer())
                {
                    KeySource keySource;

                    using (timeMeasurer.StartOperation("1. Генерация P(простое) и G(случайное)"))
                        keySource = KeySourceFactory.Generate();

                    KeyContainer[] trustedKeys;
                    using (timeMeasurer.StartOperation("2. Генерация ключей для доверенных центров"))
                        trustedKeys = KeyContainersFactory.CreateForTrustedCenters(keySource);

                    KeyContainer creatorKey;
                    using (timeMeasurer.StartOperation("3. Генерация ключей для клиента"))
                        creatorKey = KeyContainersFactory.CreateForCreator(trustedKeys);

                    KeyContainer depositKey;
                    using (timeMeasurer.StartOperation("4. Генерация ключей для центра депонирования"))
                        depositKey = KeyContainersFactory.CreateForDepositCenter(creatorKey);

                    var keys = new[] { creatorKey, depositKey }.Concat(trustedKeys);
                    DataBase.Write(keys);

                    return $"Успех! Ключ {creatorKey.Id} сгенерирован!\n{timeMeasurer.Results}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string LearnTheKey(Guid id)
        {
            try
            {
                using (var timeMeasurer = new TimeMeasurer())
                using (var context = new KeyDepositContext())
                {
                    KeyContainer[] trustedKeys;
                    using (timeMeasurer.StartOperation("1. Чтение ключей доверенных центров"))
                        trustedKeys = context
                                        .Keys
                                        .Where(c => c.KeyId == id && c.Keeper == KeyKeeper.TrustedCenter)
                                        .ToArray();

                    KeyContainer stateKey;
                    using (timeMeasurer.StartOperation("2. Генерация секретного ключа на базе частей из доверенных центров"))
                        stateKey = KeyContainersFactory.CreateForState(trustedKeys);

                    context.Keys.Add(stateKey);
                    context.SaveChanges();

                    return $"Успех! Ключ {stateKey.Id} передан в соответствующие органы!\n{timeMeasurer.Results}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }
        
    }
}