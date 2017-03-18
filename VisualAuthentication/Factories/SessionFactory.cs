using System;
using System.Linq;
using Core.Extensions;
using Newtonsoft.Json;
using VisualAuthentication.DataBaseModels;

namespace VisualAuthentication.Factories
{
    public static class SessionFactory
    {
        private static readonly Random Random = new Random();

        public static Session CreateSession()
        {
            var keys = Enumerable
                .Range(0, VASecret.KeysCount)
                .SelectToArray(_ => KeyFactory.CreateKey());

            var serializedKeys = JsonConvert.SerializeObject(keys);
            var secretKeyNumber = Random.Next(keys.Length);

            return new Session
            {
                Id = Guid.NewGuid(),
                CurrentIteration = 1,
                FirstErrorIteration = -1,
                SecretKeyNumber =  secretKeyNumber,
                SerializedKeys = serializedKeys
            };
        }
    }
}