using System;

namespace Core.Currency.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64(this byte[] data)
            => Convert.ToBase64String(data);
    }
}
