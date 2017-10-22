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

        public override bool Equals(object obj)
        {
            if (obj is EllipticCurvePoint point)
                return X == point.X && Y == point.Y;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"[ECP: x=0x{X.ToString("X")}, y=0x{Y.ToString("X")}]";
        }
    }
}