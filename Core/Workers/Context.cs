using System;
using System.Data.Entity;
using System.Linq;
using Core.DataBaseModels;

namespace Core.Workers
{
    public abstract class Context : DbContext
    {
        protected Context() : base(Secret.ConnectionString)
        {
        }

        public T Find<T>(Guid id) where T : DataBaseModel
            => Set<T>().FirstOrDefault(t => t.Id == id);

        public T Read<T>(Guid id) where T : DataBaseModel
        {
            var value = Find<T>(id);
            if (value != null)
                return value;

            throw new Exception($"Сущности <{nameof(T)}> с Id = {id} не существует");
        }

        public void Write<T>(T entity) where T : DataBaseModel
        {
            Set<T>().Add(entity);
            SaveChanges();
        }
    }
}