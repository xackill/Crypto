using System.Numerics;
using Core.Helpers;

namespace Core.Workers
{
    public static class IntFactory
    {
        public static BigInteger GenerateRandom(int bytesCount = Secret.BytesCount)
            => BigIntegerGenerator.Generate(bytesCount);

        public static BigInteger GeneratePrime(int bytesCount = Secret.BytesCount)
            => BigIntegerGenerator.GeneratePrime(bytesCount, Secret.IntervalLength, Secret.PrimeCertainty);

        public static BigInteger GenerateMutuallySimple(BigInteger n, int bytesCount = Secret.BytesCount)
        {
            while (true)
            {
                var x = GenerateRandom(bytesCount);

                var gcd = EuclideanHelper.GetGCD(x, n);
                if (gcd == 1)
                    return x;
            };
        }

        public static BigInteger GenerateBlumPrime(int bytesCount = Secret.BytesCount)
        {
            while (true)
            {
                var x = GeneratePrime(bytesCount);

                if (x % 4 == 3)
                    return x;
            }
        }
    }
}