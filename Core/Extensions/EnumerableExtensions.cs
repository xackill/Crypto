using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        private static readonly Random Rand = new Random();

        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            var arr = enumerable.ToArray();
            return arr.Length == 0 ? default(T) : arr[Rand.Next(arr.Length)];
        }

        public static string JoinStrings<T>(this IEnumerable<T> enumerable, string separator)
            => string.Join(separator, enumerable);

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T element)
            => enumerable.Except(new[] {element});

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
            => enumerable.OrderBy(_ => Guid.NewGuid());

        public static T[] SelectToArray<T, TK>(this IEnumerable<TK> enumerable, Func<TK, T> selector)
            => enumerable.Select(selector).ToArray();
    }
}
