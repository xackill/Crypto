using System;
using System.Linq;
using System.Numerics;
using Core.Extensions;
using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.DataModels.EllipticCurves
{
    public abstract class EllipticCurve
    {
        protected FiniteFieldValue A { get; }
        protected FiniteFieldValue B { get; }

        protected EllipticCurve(FiniteFieldValue a, FiniteFieldValue b)
        {
            A = a;
            B = b;
        }

        public EllipticCurvePoint Summarize(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            ThrowIfCurveDoesNotContainAllPoints(first, second);
            return SummarizeWithoutChecking(first, second);
        }

        public EllipticCurvePoint Multiply(BigInteger factor, EllipticCurvePoint point)
        {
            ThrowIfFactorLessThanZero(factor);
            ThrowIfCurveDoesNotContainAllPoints(point);

            if (point.IsInfinity || factor == 0)
                return EllipticCurvePoint.Infinity;
            
            // Algorithm 3.26
            var result = EllipticCurvePoint.Infinity;
            var binaryString = factor.ToBinaryString();

            foreach (var b in binaryString)
            {
                if (b == '1')
                    result = SummarizeWithoutChecking(result, point);
                point = DoubleWithoutChecking(point);
            }
            return result;
        }

        protected abstract EllipticCurvePoint SummarizeWithoutChecking(EllipticCurvePoint first, EllipticCurvePoint second);
        protected abstract EllipticCurvePoint DoubleWithoutChecking(EllipticCurvePoint point);
        protected abstract bool CurveDoesNotContainsPoint(EllipticCurvePoint point);
        
        private void ThrowIfCurveDoesNotContainAllPoints(params EllipticCurvePoint[] points)
        {
            var badPoints = points.Where(CurveDoesNotContainsPoint).ToArray();
            if (badPoints.Length > 0)
                throw new Exception($"{this} не содержит {badPoints.JoinStrings(", ")}");
        }

        private static void ThrowIfFactorLessThanZero(BigInteger factor)
        {
            if (factor < BigInteger.Zero)
                throw new Exception("Множитель должен быть не меньше 0");
        }
    }
}