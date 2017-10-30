using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using EllipticCurves.Helpers;
using NUnit.Framework;

namespace Test.EllipticCurvesTests
{
    public class PrimeEllipticCurveTest : EllipticCurveTestBase
    {
        private string ca, cb, p;
        
        [TestCase("0",  "1",  "0",  "1",  "8", "10", "3", "1", "23")]
        [TestCase("0",  "1", "11", "10", "14", "21", "3", "1", "23")]
        [TestCase("5", "22", "16", "27", "13", "6", "4", "20", "29")]
        public void Summarize_Success(string ax, string ay, string bx, string by, string cx, string cy, string ca, string cb, string p)
        {
            (this.ca, this.cb, this.p) = (ca, cb, p);
            AssertSummarize(ax, ay, bx, by, cx, cy);
        }
        
        [TestCase( "2", "5", "22", "14",  "6", "4", "20", "29")]
        // Example 3.13
        [TestCase( "1", "1",  "5",  "1",  "5", "4", "20", "29")]
        [TestCase( "2", "1",  "5",  "4", "19", "4", "20", "29")]
        [TestCase( "3", "1",  "5", "20",  "3", "4", "20", "29")]
        [TestCase( "4", "1",  "5", "15", "27", "4", "20", "29")]
        [TestCase( "5", "1",  "5",  "6", "12", "4", "20", "29")]
        [TestCase( "6", "1",  "5", "17", "19", "4", "20", "29")]
        [TestCase( "7", "1",  "5", "24", "22", "4", "20", "29")]
        [TestCase( "8", "1",  "5",  "8", "10", "4", "20", "29")]
        [TestCase( "9", "1",  "5", "14", "23", "4", "20", "29")]
        [TestCase("10", "1",  "5", "13", "23", "4", "20", "29")]
        [TestCase("11", "1",  "5", "10", "25", "4", "20", "29")]
        [TestCase("12", "1",  "5", "19", "13", "4", "20", "29")]
        [TestCase("13", "1",  "5", "16", "27", "4", "20", "29")]
        [TestCase("14", "1",  "5",  "5", "22", "4", "20", "29")]
        [TestCase("15", "1",  "5",  "3",  "1", "4", "20", "29")]
        [TestCase("16", "1",  "5",  "0", "22", "4", "20", "29")]
        [TestCase("17", "1",  "5", "27",  "2", "4", "20", "29")]
        [TestCase("18", "1",  "5",  "2", "23", "4", "20", "29")]
        [TestCase("19", "1",  "5",  "2",  "6", "4", "20", "29")]
        [TestCase("20", "1",  "5", "27", "27", "4", "20", "29")]
        [TestCase("21", "1",  "5",  "0",  "7", "4", "20", "29")]
        [TestCase("22", "1",  "5",  "3", "28", "4", "20", "29")]
        [TestCase("23", "1",  "5",  "5",  "7", "4", "20", "29")]
        [TestCase("24", "1",  "5", "16",  "2", "4", "20", "29")]
        [TestCase("25", "1",  "5", "19", "16", "4", "20", "29")]
        [TestCase("26", "1",  "5", "10",  "4", "4", "20", "29")]
        [TestCase("27", "1",  "5", "13",  "6", "4", "20", "29")]
        [TestCase("28", "1",  "5", "14",  "6", "4", "20", "29")]
        [TestCase("29", "1",  "5",  "8", "19", "4", "20", "29")]
        [TestCase("30", "1",  "5", "24",  "7", "4", "20", "29")]
        [TestCase("31", "1",  "5", "17", "10", "4", "20", "29")]
        [TestCase("32", "1",  "5",  "6", "17", "4", "20", "29")]
        [TestCase("33", "1",  "5", "15",  "2", "4", "20", "29")]
        [TestCase("34", "1",  "5", "20", "26", "4", "20", "29")]
        [TestCase("35", "1",  "5",  "4", "10", "4", "20", "29")]
        [TestCase("36", "1",  "5",  "1", "24", "4", "20", "29")]
        [TestCase("38", "1",  "5",  "1",  "5", "4", "20", "29")]
        public void Multiply_Success(string factor, string ax, string ay, string cx, string cy, string ca, string cb, string p)
        {
            (this.ca, this.cb, this.p) = (ca, cb, p);
            AssertMultiply(factor, ax, ay, cx, cy);
        }
        
        protected override FiniteField Field 
            => EllipticParser.ParsePrimeField(p);
      
        protected override EllipticCurve Curve
            => EllipticParser.ParsePrimeEllipticCurve(ca, cb, Field);
    }
}