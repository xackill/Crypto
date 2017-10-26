using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using EllipticCurves.Helpers;
using NUnit.Framework;
using SmartParser = Core.Helpers.BigIntegerSmartParser;

namespace Test.EllipticCurvesTests
{
    public abstract class EllipticCurveTestBase
    {
        protected void AssertSummarize(string ax, string ay, string bx, string by, string cx, string cy)
        {
            var (a, b, expected) = (GetPoint(ax, ay), GetPoint(bx, by), GetPoint(cx, cy));
            
            var actual = Curve.Summarize(a, b);
            Assert.AreEqual(expected, actual);            
        }

        protected void AssertMultiply(string factor, string ax, string ay, string cx, string cy)
        {
            var (f, a, expected) = (SmartParser.Parse(factor), GetPoint(ax, ay), GetPoint(cx, cy));

            var actual = Curve.Multiply(f, a);
            Assert.AreEqual(expected, actual);        
        }
        
        private EllipticCurvePoint GetPoint(string a, string b)
        {
            return EllipticParser.ParsePoint(a, b, Field);
        }

        protected abstract EllipticCurve Curve { get; }
        protected abstract FiniteField Field { get; }
    }
}