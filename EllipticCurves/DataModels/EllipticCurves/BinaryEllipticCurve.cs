using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.DataModels.EllipticCurves
{
    public class BinaryEllipticCurve : EllipticCurve
    {
        public BinaryEllipticCurve(FiniteFieldValue a, FiniteFieldValue b) : base(a, b)
        {
        }

        protected override EllipticCurvePoint SummarizeWithoutChecking(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (first.IsInfinity)
                return second;
            if (second.IsInfinity)
                return first;
            
            if (first.X == second.X && first.Y == first.X + second.Y)
                return EllipticCurvePoint.Infinity;
            
            if (first.X == second.X)
                return DoubleWithoutChecking(first);
            
            var lambda = (first.Y + second.Y) / (first.X + second.X);
            
            var x = lambda * lambda + lambda + first.X + second.X + A;
            var y = lambda * (first.X + x) + x + first.Y;
            
            return new EllipticCurvePoint(x, y);
        }

        protected override EllipticCurvePoint DoubleWithoutChecking(EllipticCurvePoint point)
        {
            if (point.IsInfinity)
                return point;

            var fx2 = point.X * point.X;
            var x = fx2 + B / fx2;

            var y = fx2 + (point.X + point.Y / point.X) * x + x;
            
            return new EllipticCurvePoint(x, y);
        }

        protected override bool CurveDoesNotContainsPoint(EllipticCurvePoint point)
        {
            if (point.IsInfinity)
                return true;

            var y2 = point.Y * point.Y;
            var x2 = point.X * point.X;
            
            return (y2 + point.X * point.Y - x2 * point.X - A * x2 - B) != 0;
        }
    }
}