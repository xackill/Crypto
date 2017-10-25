using System.Numerics;
using EllipticCurves.DataModels;
using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.Helpers
{
    public class EllipticCurvePointCalculator
    {
        private readonly EllipticCurve curve;
        private readonly EllipticCurvePointCalculusChecker checker;
        
        public EllipticCurvePointCalculator(EllipticCurve curve)
        {
            this.curve = curve;
            checker = new EllipticCurvePointCalculusChecker(curve);
        }

        public EllipticCurvePoint Summarize(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            checker.ThrowIfCurveDoesNotContainAllPoints(first, second);
            return SummarizeWithoutChecking(first, second);
        }

        public EllipticCurvePoint Multiply(BigInteger factor, EllipticCurvePoint point)
        {
            checker.ThrowIfFactorLessThanOne(factor);
            checker.ThrowIfCurveDoesNotContainAllPoints(point);

            var result = point;
            for (factor--; factor != BigInteger.Zero; factor /= 2, point = Double(point))
            {
                if (factor % 2 == 0) 
                    continue;
                
                result = SummarizeWithoutChecking(result, point);
                factor--;
            }

            return result;
        }
        
        private EllipticCurvePoint SummarizeWithoutChecking(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (first.X == second.X)
                return Double(first);
            
            var lambda = (second.Y - first.Y) / (second.X - first.X);
            return SummarizeInternal(lambda, first, second);
        }
        
        private EllipticCurvePoint Double(EllipticCurvePoint point)
        {
            var k1 = 3 * point.X * point.X + curve.A;
            var k2 = 2 * point.Y;

            return SummarizeInternal(k1 / k2, point, point);
        }
        
        private static EllipticCurvePoint SummarizeInternal(FiniteFieldValue lambda, EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var x = lambda * lambda - first.X - second.X;
            var y = lambda * (first.X - x) - first.Y;
            
            return new EllipticCurvePoint(x, y);
        }
    }
}