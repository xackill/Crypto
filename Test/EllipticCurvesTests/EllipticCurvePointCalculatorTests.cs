using EllipticCurves.DataModels;
using EllipticCurves.Helpers;
using NUnit.Framework;

namespace Test.EllipticCurvesTests
{
    public class EllipticCurvePointCalculatorTests
    {
        private EllipticCurvePointCalculator sut;

        [SetUp]
        public void SetUp()
        {
            sut = new EllipticCurvePointCalculator(SimpleEllipticCurve);
        }

        [Test]
        public void Summarize_DifferentPoints_Success()
        {
            var firstPoint = EllipticParser.ParsePoint(x: "0", y: "1", field: SimplePrimeField);
            var secondPoint = EllipticParser.ParsePoint(x: "11", y: "10", field: SimplePrimeField);
            var actual = sut.Summarize(firstPoint, secondPoint);

            var expected = EllipticParser.ParsePoint(x: "14", y: "21", field: SimplePrimeField);
            Assert.AreEqual(expected, actual);            
        }
        
        [Test]
        public void Summarize_SamePoint_Success()
        {
            var point = EllipticParser.ParsePoint(x: "0", y: "1", field: SimplePrimeField);
            var actual = sut.Summarize(point, point);

            var expected = EllipticParser.ParsePoint(x: "8", y: "10", field: SimplePrimeField);
            Assert.AreEqual(expected, actual);            
        }

        private static readonly FiniteField SimplePrimeField = EllipticParser.ParseField(p: "23");
        private static readonly EllipticCurve SimpleEllipticCurve = EllipticParser.ParseCurve(a: "3", b: "1", field: SimplePrimeField);
    }
}