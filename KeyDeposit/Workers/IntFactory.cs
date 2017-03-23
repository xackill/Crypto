using System.Numerics;
using KeyDeposit.Helpers;

namespace KeyDeposit.Workers
{
    public static class IntFactory
    {
        public static BigInteger GenerateRandom()
            => BigIntegerGenerator.Generate(KDSecret.BytesCount);

        public static BigInteger GeneratePrime()
            => BigIntegerGenerator.GeneratePrime(KDSecret.BytesCount, KDSecret.IntervalLength, KDSecret.PrimeCertainty);
    }
}