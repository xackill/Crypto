using System;
using Newtonsoft.Json;
using VisualAuthentication.DataBaseModels;
using VisualAuthentication.DataModels;
using VisualAuthentication.DataViewModels;

namespace VisualAuthentication.Extensions
{
    public static class SessionExtensions
    {
        public static Key[] Keys(this Session session) 
            => JsonConvert.DeserializeObject<Key[]>(session.SerializedKeys);

        public static Key SecretKey(this Session session) 
            => session.Keys()[session.SecretKeyNumber];

        public static bool IsClose(this Session session)
            => session.CurrentIteration == VASecret.IterationsCount;

        public static bool IsSuccess(this Session session)
            => session.IsClose() && session.FirstErrorIteration == -1;

        public static SessionResultViewModel GetFinResult(this Session session)
        {
            if (!session.IsClose())
                throw new Exception("Сессия не завершена!");

            return new SessionResultViewModel
            {
                ResultText = session.IsSuccess()
                                ? "Успех! Авторизация завершена!" : 
                                $"Ошибка! Один или несколько шагов были неверными! (первая ошибка на {session.FirstErrorIteration} шаге)"
            };
        }
    }
}