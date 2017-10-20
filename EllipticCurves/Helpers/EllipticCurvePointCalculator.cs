using System.Numerics;
using Core.Extensions;
using EllipticCurves.DataModels;

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
            checker.ThrowIfFactorLessThanZero(factor);
            checker.ThrowIfCurveDoesNotContainAllPoints(point);

            var result = point;
            for (factor--; factor != BigInteger.Zero; factor /= 2, point = Double(point))
            {
                if (factor % 2 == 0) 
                    continue;
                
                if (result.X == point.X || result.Y == point.Y)
                    result = Double(result);
                else
                    result = SummarizeWithoutChecking(result, point);
                factor--;
            }

            return result;
        }
        
        private EllipticCurvePoint SummarizeWithoutChecking(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var (dx, dy) = Normalize(second.X - first.X, second.Y - first.Y);
            var lambda = dy * dx.ModInverse(curve.Modulus);
            
            return SummarizeInternal(lambda, first, second);
        }
        
        private EllipticCurvePoint Double(EllipticCurvePoint point)
        {
            var k1 = 3 * point.X * point.X + curve.A;
            var k2 = (2 * point.Y).ModInverse(curve.Modulus);

            return SummarizeInternal(k1 * k2, point, point);
        }
        
        private EllipticCurvePoint SummarizeInternal(BigInteger lambda, EllipticCurvePoint first, EllipticCurvePoint second)
        {
            lambda = Normalize(lambda);

            var x = Normalize(lambda * lambda - first.X - second.X);
            var y = Normalize(lambda * (first.X - x) - first.Y);
            
            return new EllipticCurvePoint(x, y);
        }
        
        private BigInteger Normalize(BigInteger coord)
        {
            coord %= curve.Modulus;
            return coord >= 0 ? coord : coord + curve.Modulus;
        }

        private (BigInteger, BigInteger) Normalize(BigInteger dx, BigInteger dy)
        {
            return (Normalize(dx), Normalize(dy));
        }
    }
}