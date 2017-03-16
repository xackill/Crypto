using System;
using System.Linq;
using System.Text;

namespace Core.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        public static string ConvertToString(this byte[] bytes)
            => Encoding.Unicode.GetString(bytes);

        public static byte[] ConcatBytes(this byte[] first, byte second)
            => first.ConcatBytes(new [] {second});

        public static byte[] ConcatBytes(this byte[] first, byte[] second)
            => first.Concat(second).ToArray();
    }
}
