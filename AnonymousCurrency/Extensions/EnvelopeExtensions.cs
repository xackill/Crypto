using System;
using System.Collections.Generic;
using System.Linq;
using AnonymousCurrency.DataModels;

namespace AnonymousCurrency.Extensions
{
    public static class EnvelopeExtensions
    {
        public static IEnumerable<byte[]> EnumerateSecrets(this IEnvelope envelope)
        {
            var bytes = envelope.EncryptedSecrets;
            if (bytes.Length % 128 != 0)
                throw new Exception($"Байтовый массив поврежден: Необходима длина кратная 128 байт, а сейчас {bytes.Length}");

            while (bytes.Length > 0)
            {
                yield return bytes.Take(128).ToArray();
                bytes = bytes.Skip(128).ToArray();
            }
        }
    }
}
