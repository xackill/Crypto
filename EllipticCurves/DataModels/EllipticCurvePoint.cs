using System.Numerics;

namespace EllipticCurves.DataModels
{
    public class EllipticCurvePoint
    {
        public readonly BigInteger X;
        public readonly BigInteger Y;

        public EllipticCurvePoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[ECP: x=0x{X.ToString("X")}, y=0x{Y.ToString("X")}]";
        }
    }
}