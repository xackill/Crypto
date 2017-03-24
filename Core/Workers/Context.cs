using System;
using System.Collections.Generic;
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

        public void Write<T>(IEnumerable<T> entities) where T : DataBaseModel
        {
            Set<T>().AddRange(entities);
            SaveChanges();
        }

        public void Write<T>(T entity) where T : DataBaseModel
            => Write<T>(new[] { entity });

        public void Update<T>(IEnumerable<T> entities) where T : DataBaseModel
        {
            foreach (var entity in entities)
            {
                Set<T>().Attach(entity);
                Entry(entity).State = EntityState.Modified;
            }
            SaveChanges();
        }

        public void Update<T>(T entity) where T : DataBaseModel
            => Update<T>(new[] { entity });
    }
}