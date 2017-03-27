 using System;
 using System.Linq;
 using System.Web.Mvc;
 using Core.Helpers;
 using Newtonsoft.Json;
 using ProbabilisticEncryption.DataBaseModels;
 using ProbabilisticEncryption.Workers;
using DataBase = Core.Workers.DataBase<ProbabilisticEncryption.Workers.ProbabilisticEncryptionContext>;

namespace Currency.Controllers
{
    public class ProbabilisticEncryptionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Work()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public string LoadKeyContainersIds()
        {
            try
            {
                using (var context = new ProbabilisticEncryptionContext())
                {
                    var keys = context
                                    .KeyContainers
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

        public string LoadEncryptedMessagesIds()
        {
            try
            {
                using (var context = new ProbabilisticEncryptionContext())
                {
                    var keys = context
                                    .EncryptedMessages
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

        public string LoadKeyContainer(Guid id)
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

        public string LoadEncryptedMessage(Guid id)
        {
            try
            {
                var encryptedMessage = DataBase.Read<EncryptedMessageContainer>(id);
                return JsonConvert.SerializeObject(encryptedMessage);
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string CreateKeyContainer()
        {
            try
            {
                using (var timeMeasurer = new TimeMeasurer())
                {
                    KeyContainer key;
                    using (timeMeasurer.StartOperation("1. Генерация контейнера"))
                        key = KeyContainerFactory.CreateNew();

                    using (timeMeasurer.StartOperation("2. Запись контейнера в бд"))
                        DataBase.Write(key);

                    return $"Успех! Контейнер {key.Id} сгенерирован!\n{timeMeasurer.Results}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string CreateEncryptedMessage(Guid keyId, string message)
        {
            try
            {
                using (var timeMeasurer = new TimeMeasurer())
                {
                    KeyContainer key;
                    using (timeMeasurer.StartOperation("1. Чтение ключа из бд"))
                        key = DataBase.Read<KeyContainer>(keyId);

                    EncryptedMessageContainer emsg;
                    using (timeMeasurer.StartOperation("2. Шифрование"))
                        emsg = ProbabilisticCryptoProvider.Encrypt(key, message);

                    using (timeMeasurer.StartOperation("3. Запись зашифрованного сообщения в бд"))
                        DataBase.Write(emsg);

                    return $"Успех! Сообщение {emsg.Id} зашифровано!\n{timeMeasurer.Results}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }

        public string DecryptMessage(Guid keyId, Guid messageId)
        {
            try
            {
                using (var timeMeasurer = new TimeMeasurer())
                {
                    KeyContainer key;
                    using (timeMeasurer.StartOperation("1. Чтение контейнера ключа из бд"))
                        key = DataBase.Read<KeyContainer>(keyId);

                    EncryptedMessageContainer emsg;
                    using (timeMeasurer.StartOperation("2. Чтение зашифрованного сообщения из бд"))
                        emsg = DataBase.Read<EncryptedMessageContainer>(messageId);

                    string msg;
                    using (timeMeasurer.StartOperation("3. Расшифрование"))
                        msg = ProbabilisticCryptoProvider.Decrypt(key, emsg);

                    return $"Успех! Сообщение {emsg.Id} зашифровано!\n{timeMeasurer.Results}\n\nСодержание:\n{msg}";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }
    }
}