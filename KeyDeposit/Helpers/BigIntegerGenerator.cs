using System.Numerics;
using System.Security.Cryptography;

namespace KeyDeposit.Helpers
{
    public static class BigIntegerGenerator
    {
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();

        public static BigInteger Generate(long bytesCount)
        {
            var bytes = new byte[bytesCount];
            Random.GetBytes(bytes);

            var random = new BigInteger(bytes);
            return BigInteger.Abs(random);
        }

        public static BigInteger Generate(BigInteger maxVal)
        {
            var bytes = maxVal.ToByteArray();
            var maxValBytesLen = bytes.LongLength;
            return Generate(maxValBytesLen) % maxVal;
        }

        public static BigInteger Generate(BigInteger minVal, BigInteger maxVal)
        {
            var intervalLen = maxVal - minVal;
            return minVal + Generate(intervalLen);
        }

        public static BigInteger GeneratePrime(long bytesCount, int intervalLength, int certainty)
        {
            var stepsCount = intervalLength / 2;

            while (true)
            {
                var minVal = Generate(bytesCount);
                var maxVal = minVal + intervalLength;

                for (var i = 0; i < stepsCount; ++i)
                {
                    var tmp = Generate(minVal, maxVal);
                    if (RabinMillerHelper.IsPrime(tmp, certainty))
                        return tmp;
                }
            }
        }
    }
}