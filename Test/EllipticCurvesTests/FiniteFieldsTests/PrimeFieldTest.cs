using EllipticCurves.DataModels.FiniteFields;
using NUnit.Framework;

namespace Test.EllipticCurvesTests.FiniteFieldsTests
{
    public class PrimeFieldTest : FiniteFieldTestBase
    {
        [TestCase(17, 20, 8, 29)]
        public void Addition(int a, int b, int c, int modulus)
        {
            var field = new PrimeField(modulus);
            AssertAddition(a, b, c, field);
        }
        
        [TestCase(17, 20, 26, 29)]
        public void Subtraction(int a, int b, int c, int modulus)
        {
            var field = new PrimeField(modulus);
            AssertSubtraction(a, b, c, field);
        }
        
        [TestCase(17, 20, 21, 29)]
        public void Multiplication(int a, int b, int c, int modulus)
        {
            var field = new PrimeField(modulus);
            AssertMultiplication(a, b, c, field);
        }
        
        [TestCase(17, 12, 29)]
        public void Inversion(int a, int c, int modulus)
        {
            var field = new PrimeField(modulus);
            AssertInversion(a, c, field);
        }
    }
}