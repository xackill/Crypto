using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Core.Extensions
{
    public static class BigIntegerExtensions
    {
        public static int DegreeOfBinaryPolynomial(this BigInteger value)
        {
            var bytes = BigInteger.Abs(value).ToByteArray();
            var lastPart = Convert.ToString(bytes.Last(), 2).TrimStart('0').Length;
            return (bytes.Length - 1) * 8 + lastPart - 1;
        }

        public static string ToBinaryString(this BigInteger value)
        {
            var bytes = BigInteger.Abs(value).ToByteArray();
            return bytes
                .Aggregate(InitBuilder(bytes), (sb, b) => sb.Append(ByteToString(b)))
                .ToString()
                .TrimEnd('0');
        }

        private static StringBuilder InitBuilder(byte[] bytes)
        {
            return new StringBuilder(bytes.Length * 8);
        }

        private static string ByteToString(byte @byte)
        {
            return Convert.ToString(@byte, 2).Reverse().PadRight(8, '0');
        }
    }
}