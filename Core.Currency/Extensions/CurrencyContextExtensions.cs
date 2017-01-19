using System;
using System.Linq;
using Core.Currency.DataBaseModels;
using Core.Currency.Workers;

namespace Core.Currency.Extensions
{
    public static class CurrencyContextExtensions
    {
        public static T Find<T>(this CurrencyContext context, Guid id) where T : DataBaseModel
            => context.Set<T>().FirstOrDefault(t => t.Id == id);

        public static T Read<T>(this CurrencyContext context, Guid id) where T : DataBaseModel
        {
            var value = context.Find<T>(id);
            if (value != null)
                return value;

            throw new Exception($"Сущности <{nameof(T)}> с Id = {id} не существует");
        }
    }
}
