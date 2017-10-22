using System;
using System.Numerics;
using Core.Extensions;
using EllipticCurves.DataModels;
using EllipticCurves.Helpers;
using NUnit.Framework;

namespace Test.EllipticCurvesTests
{
    public class EllipticCurvePointCalculatorTests
    {
        private EllipticCurve curve;
        private EllipticCurvePointCalculator sut;

        [SetUp]
        public void SetUp()
        {
            curve = SimpleEllipticCurve;
            sut = new EllipticCurvePointCalculator(curve);
        }

        [Test]
        public void Summarize_DifferentPoints_Success()
        {
            var firstPoint = new EllipticCurvePoint(x:  0, y:  1);
            var secondPoint = new EllipticCurvePoint(x: 11, y: 10);
            var actual = sut.Summarize(firstPoint, secondPoint);

            var expected = new EllipticCurvePoint(x: 14, y: 21);
            Assert.AreEqual(expected, actual);            
        }
        
        [Test]
        public void Summarize_SamePoint_Success()
        {
            var point = new EllipticCurvePoint(x:  0, y:  1);
            var actual = sut.Summarize(point, point);

            var expected = new EllipticCurvePoint(x: 8, y: 10);
            Assert.AreEqual(expected, actual);            
        }

        private static EllipticCurve SimpleEllipticCurve 
            => new EllipticCurve(a : 3, b: 1, modulus: 23);
    }
}