using System.Numerics;

namespace Core.Helpers
{
    public static class EuclideanHelper
    {
        public static BigInteger Gcd(BigInteger a, BigInteger b)
        {
            while (!a.IsZero && !b.IsZero)
            {
                if (a > b)
                    a = a % b;
                else
                    b = b % a;
            }

            return a + b;
        }


        public static (BigInteger gcd, BigInteger x, BigInteger y) GcdExtended(BigInteger a, BigInteger b)
        {
            if (a == 0)
                return (gcd: b, x: 0, y: 1);

            var (gcd, x, y) = GcdExtended(b % a, a);
            return (gcd: gcd, x: y - b / a * x, y: x);
        }
    }
}