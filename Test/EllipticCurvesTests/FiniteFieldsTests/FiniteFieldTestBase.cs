using EllipticCurves.DataModels.FiniteFields;
using NUnit.Framework;

namespace Test.EllipticCurvesTests.FiniteFieldsTests
{
    public abstract class FiniteFieldTestBase
    {
        protected void AssertAddition(int a, int b, int c, FiniteField field)
        {
            var (x1, x2, expected) = Convert(a, b, c, field);
            Assert.AreEqual(expected, x1 + x2);
        }
        
        protected void AssertSubtraction(int a, int b, int c, FiniteField field)
        {
            var (x1, x2, expected) = Convert(a, b, c, field);
            Assert.AreEqual(expected, x1 - x2);
        }
        
        protected void AssertMultiplication(int a, int b, int c, FiniteField field)
        {
            var (x1, x2, expected) = Convert(a, b, c, field);
            Assert.AreEqual(expected, x1 * x2);
        }
        
        protected void AssertInversion(int a, int c, FiniteField field)
        {
            var (x1, expected) = Convert(a, c, field);
            Assert.AreEqual(expected, 1/x1);
        }
        
        private (FiniteFieldValue, FiniteFieldValue) Convert(int a, int c, FiniteField f)
        {
            return (new FiniteFieldValue(a, f), new FiniteFieldValue(c, f));
        }

        private (FiniteFieldValue, FiniteFieldValue, FiniteFieldValue) Convert(int a, int b, int c, FiniteField f)
        {
            return (new FiniteFieldValue(a, f), new FiniteFieldValue(b, f), new FiniteFieldValue(c, f));
        }
    }
}