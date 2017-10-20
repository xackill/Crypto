using System.Numerics;

namespace Core.Extensions
{
    public static class BigIntegerExtensions
    {
        public static BigInteger ModInverse(this BigInteger x, BigInteger modulus) 
            => BigInteger.ModPow(x, modulus - 2, modulus);
    }
}