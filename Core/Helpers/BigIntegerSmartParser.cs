using System;
using System.Globalization;
using System.Numerics;

namespace Core.Helpers
{
    public static class BigIntegerSmartParser
    {
        public static BigInteger Parse(string data)
        {
            return !data.StartsWith("0x")
                ? Parse(data, NumberStyles.AllowLeadingSign)
                : Parse(data.Substring(2), NumberStyles.HexNumber);
        }

        private static BigInteger Parse(string data, NumberStyles numberStyles)
        {
            if (BigInteger.TryParse(data, numberStyles, null, out var result))
                return result;
            
            throw new Exception($"'{data}' не распознано как число");
        }
    }
}