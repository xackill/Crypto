using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Currency.Extensions
{
    public static class EnumerableExtensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            var rand = new Random();
            var arr = enumerable.ToArray();

            return arr.Length == 0 ? default(T) : arr[rand.Next(arr.Length)];
        }

        public static string JoinStrings<T>(this IEnumerable<T> enumerable, string separator)
            => string.Join(separator, enumerable);
    }
}
