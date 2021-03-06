﻿using System;
using System.Collections.Generic;
using Core.DataBaseModels;

namespace Core.Workers
{
    public static class DataBase<TC> where TC : Context, new()
    {
        public static T Find<T>(Guid id) where T : DataBaseModel
        {
            using (var context = new TC())
                return context.Find<T>(id);
        }

        public static T Read<T>(Guid id) where T : DataBaseModel
        {
            using (var context = new TC())
                return context.Read<T>(id);
        }

        public static void WriteAll<T>(IEnumerable<T> entities) where T : DataBaseModel
        {
            using (var context = new TC())
                context.Write(entities);
        }

        public static void UpdateAll<T>(IEnumerable<T> entities) where T : DataBaseModel
        {
            using (var context = new TC())
                context.Update(entities);
        }
        
        public static void Write<T>(T entity) where T : DataBaseModel
             => WriteAll(new[] { entity });

        public static void Update<T>(T entity) where T : DataBaseModel
            => UpdateAll(new[] { entity });
    }
}