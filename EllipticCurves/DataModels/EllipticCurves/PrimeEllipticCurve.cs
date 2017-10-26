using System;
using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.DataModels.EllipticCurves
{
    public class PrimeEllipticCurve : EllipticCurve
    {
        public PrimeEllipticCurve(FiniteFieldValue a, FiniteFieldValue b) : base(a, b)
        {
            if (4 * (A * A * A) + 27 * (B * B) == 0)
                throw new Exception($"{this} сингулярна");
        }
        
        public override string ToString()
        {
            return $"[EC: a={A}, b={B}]";
        }

        protected override EllipticCurvePoint SummarizeWithoutChecking(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (first.IsInfinity)
                return second;
            if (second.IsInfinity)
                return first;
            
            if (first.X == second.X && first.Y == -second.Y)
                return EllipticCurvePoint.Infinity;
            
            if (first.X == second.X)
                return DoubleWithoutChecking(first);
            
            var lambda = (second.Y - first.Y) / (second.X - first.X);
            return SummarizeInternal(lambda, first, second);
        }

        protected override EllipticCurvePoint DoubleWithoutChecking(EllipticCurvePoint point)
        {
            if (point.IsInfinity)
                return point;
            
            var k1 = 3 * point.X * point.X + A;
            var k2 = 2 * point.Y;

            return SummarizeInternal(k1 / k2, point, point);
        }

        protected override bool CurveDoesNotContainsPoint(EllipticCurvePoint point)
        {
            if (point.IsInfinity)
                return true;
            
            var x3 = point.X * point.X * point.X;
            var y2 = point.Y * point.Y;

            return (y2 - x3 - A * point.X - B) != 0;
        }
              
        private static EllipticCurvePoint SummarizeInternal(FiniteFieldValue lambda, EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var x = lambda * lambda - first.X - second.X;
            var y = lambda * (first.X - x) - first.Y;
            
            return new EllipticCurvePoint(x, y);
        }
    }
}