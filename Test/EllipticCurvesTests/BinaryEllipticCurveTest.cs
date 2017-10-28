using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using EllipticCurves.Helpers;
using NUnit.Framework;

namespace Test.EllipticCurvesTests
{
    public class BinaryEllipticCurveTest : EllipticCurveTestBase
    {
        private string ca, cb, f, p;
        
        [TestCase("2", "15", "12", "12", "1", "1", "8", "9", "19", "16")]
        public void Summarize_Success(string ax, string ay, string bx, string by, string cx, string cy, string ca, string cb, string f, string p)
        {
            (this.ca, this.cb, this.f, this.p) = (ca, cb, f, p);
            AssertSummarize(ax, ay, bx, by, cx, cy);
        }
        
        [TestCase("2", "2", "15", "11", "2", "8", "9", "19", "16")]
        // Example 3.14
        [TestCase( "1", "8", "1",  "8",  "1", "8", "9", "19", "16")]
        [TestCase( "2", "8", "1",  "9", "15", "8", "9", "19", "16")]
        [TestCase( "3", "8", "1", "12",  "0", "8", "9", "19", "16")]
        [TestCase( "4", "8", "1", "15", "11", "8", "9", "19", "16")]
        [TestCase( "5", "8", "1", "11",  "2", "8", "9", "19", "16")]
        [TestCase( "6", "8", "1", "11",  "9", "8", "9", "19", "16")]
        [TestCase( "7", "8", "1", "15",  "4", "8", "9", "19", "16")]
        [TestCase( "8", "8", "1", "12", "12", "8", "9", "19", "16")]
        [TestCase( "9", "8", "1",  "9",  "6", "8", "9", "19", "16")]
        [TestCase("10", "8", "1",  "8",  "9", "8", "9", "19", "16")]
        [TestCase("12", "8", "1",  "8",  "1", "8", "9", "19", "16")]
        public void Multiply_Success(string factor, string ax, string ay, string cx, string cy, string ca, string cb, string f, string p)
        {
            (this.ca, this.cb, this.f, this.p) = (ca, cb, f, p);
            AssertMultiply(factor, ax, ay, cx, cy);
        }
        
        protected override FiniteField Field 
            => EllipticParser.ParseBinaryField(f, p);
      
        protected override EllipticCurve Curve
            => EllipticParser.ParseBinaryEllipticCurve(ca, cb, Field);
    }
}