using System;

namespace Core.Currency.Exceptions
{
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>(this T[] arr)
            => arr[new Random().Next(arr.Length)];
    }
}
