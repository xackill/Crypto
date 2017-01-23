using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Currency.Extensions
{
    public static class MatchCollectionExtensions
    {
        public static IEnumerable<T> Select<T>(this MatchCollection collection, Func<Match, T> func)
            => from Match m in collection select func(m);
    }
}