using System.Numerics;

namespace Core.Helpers
{
    public static class EuclideanHelper
    {
        public static (BigInteger ar, BigInteger br) GetExtendedGCD(BigInteger a, BigInteger b)
        {
            while (!a.IsZero && !b.IsZero)
            {
                if (a > b)
                    a = a % b;
                else
                    b = b % a;
            }

            return (a, b);
        }

        public static BigInteger GetGCD(BigInteger a, BigInteger b)
        {
            var (ar, br) = GetExtendedGCD(a, b);
            return ar + br;
        }
    }
}