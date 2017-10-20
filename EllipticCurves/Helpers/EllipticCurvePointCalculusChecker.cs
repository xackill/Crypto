using System;
using System.Linq;
using System.Numerics;
using Core.Extensions;
using EllipticCurves.DataModels;

namespace EllipticCurves.Helpers
{
    public class EllipticCurvePointCalculusChecker
    {
        private readonly EllipticCurve curve;

        public EllipticCurvePointCalculusChecker(EllipticCurve curve)
        {
            this.curve = curve;
        }

        public void ThrowIfCurveDoesNotContainAllPoints(params EllipticCurvePoint[] points)
        {
            var badPoints = points.Where(CurveDoesNotContainsPoint).ToArray();
            if (badPoints.Length > 0)
                throw new Exception($"{curve} не содержит {badPoints.JoinStrings(", ")}");
        }

        public void ThrowIfFactorLessThanZero(BigInteger factor)
        {
            if (factor <= BigInteger.Zero)
                throw new Exception("Множитель должен быть больше 0");
        }

        private bool CurveDoesNotContainsPoint(EllipticCurvePoint point)
        {
            var x3 = point.X * point.X * point.X;
            var y2 = point.Y * point.Y;

            return (y2 - x3 - curve.A * point.X - curve.B) % curve.Modulus != BigInteger.Zero;
        }
    }
}