using System.Linq;
using System.Numerics;
using Core.Helpers;
using Core.Workers;

namespace ProbabilisticEncryption.Workers
{
    public class BBSGenerator
    {
        public readonly BigInteger N;
        public readonly BigInteger M;
        public readonly BigInteger X0;

        public readonly BigInteger P;
        public readonly BigInteger Q;

        public BBSGenerator(BigInteger p, BigInteger q)
        {
            P = p; Q = q;
            N = p * q;
            M = (p - 1) * (q - 1);

            var x = IntFactory.GenerateMutuallySimple(N);
            X0 = BigInteger.ModPow(x, 2, N);
        }

        public BBSGenerator(BigInteger p, BigInteger q, int t, BigInteger xt)
        {
            P = p; Q = q;
            N = p * q;
            M = (p - 1) * (q - 1);

            var (_, a, b) = EuclideanHelper.GcdExtended(p, q);
            var u = BigInteger.ModPow((p + 1) / 4, t, p - 1);
            var v = BigInteger.ModPow((q + 1) / 4, t, q - 1);
            var w = BigInteger.ModPow(xt % p, u, p);
            var z = BigInteger.ModPow(xt % q, v, q);
            X0 = (b * q * w + a * p * z) % N;
        }

        public BigInteger GetX(int i)
            => BigInteger.ModPow(X0, (BigInteger.One << i) % M, N);

        public byte GetByte(int i)
            => Enumerable
                .Range(0, 8)
                .Select(k => (byte) (GetBit(i + k) << k))
                .Aggregate((s, next) => (byte) (s | next));

        private byte GetBit(int i)
            => (byte) (GetX(i).ToByteArray()[0] & 1);
    }
}