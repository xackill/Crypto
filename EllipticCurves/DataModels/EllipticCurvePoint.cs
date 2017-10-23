namespace EllipticCurves.DataModels
{
    public class EllipticCurvePoint
    {
        public readonly FiniteFieldValue X;
        public readonly FiniteFieldValue Y;

        public EllipticCurvePoint(FiniteFieldValue x, FiniteFieldValue y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is EllipticCurvePoint point)
                return X == point.X && Y == point.Y;

            return false;
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
            return $"[ECP: x={X}, y={Y}]";
        }
    }
}