using System.Numerics;
using Core.Helpers;

namespace Core.Workers
{
    public static class IntFactory
    {
        public static BigInteger GenerateRandom()
            => BigIntegerGenerator.Generate(Secret.BytesCount);

        public static BigInteger GeneratePrime()
            => BigIntegerGenerator.GeneratePrime(Secret.BytesCount, Secret.IntervalLength, Secret.PrimeCertainty);

        public static BigInteger GenerateMutuallySimple(BigInteger n)
        {
            while (true)
            {
                var x = GenerateRandom();

                var gcd = EuclideanHelper.GetGCD(x, n);
                if (gcd == 1)
                    return x;
            };
        }
    }
}