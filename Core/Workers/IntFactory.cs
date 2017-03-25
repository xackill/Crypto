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
    }
}