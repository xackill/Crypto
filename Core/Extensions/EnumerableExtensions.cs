using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
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

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T element)
            => enumerable.Except(new[] {element});

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
            => enumerable.OrderBy(_ => Guid.NewGuid());
    }
}
