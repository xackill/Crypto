using System.Numerics;

namespace Core.Helpers
{
    public static class RabinMillerHelper
    {
        public static bool IsPrime(BigInteger src, int certainty)
        {
            if (src == 2 || src == 3)
                return true;
            if (src < 2 || (src & 1) == 0)
                return false;

            var d = src - 1;
            var s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                ++s;
            }

            var len = src.ToByteArray().LongLength;

            for (var i = 0; i < certainty; i++)
            {
                var a = BigInteger.Zero;

                while (a < 2 || a >= src - 2) 
                    a = BigIntegerGenerator.Generate(len);

                var x = BigInteger.ModPow(a, d, src);
                if (x == 1 || x == src - 1)
                    continue;

                for (var r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, src);
                    if (x == 1)
                        return false;
                    if (x == src - 1)
                        break;
                }

                if (x != src - 1)
                    return false;
            }

            return true;
        }
    }
}