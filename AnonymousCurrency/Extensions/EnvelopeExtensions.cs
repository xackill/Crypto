using System;
using System.Collections.Generic;
using System.Linq;
using AnonymousCurrency.DataModels;

namespace AnonymousCurrency.Extensions
{
    public static class EnvelopeExtensions
    {
        public static IEnumerable<byte[]> EncryptedSecrets(this IEnvelope envelope)
        {
            var bytes = envelope.EncryptedSecrets;
            if (bytes.Length % 16 != 0)
                throw new Exception($"Байтовый массив поврежден: Необходима длина кратная 16 байт, а сейчас {bytes.Length}");

            while (bytes.Length > 0)
            {
                yield return bytes.Take(16).ToArray();
                bytes = bytes.Skip(16).ToArray();
            }
        }
    }
}
