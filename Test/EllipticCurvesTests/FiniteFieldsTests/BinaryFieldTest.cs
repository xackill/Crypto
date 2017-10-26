using EllipticCurves.DataModels.FiniteFields;
using NUnit.Framework;

namespace Test.EllipticCurvesTests.FiniteFieldsTests
{
    public class BinaryFieldTest : FiniteFieldTestBase
    {
        [TestCase(13, 7, 10, 19, 16)]
        public void Addition(int a, int b, int c, int reductionPolynomial, int modulus)
        {
            var field = new BinaryField(reductionPolynomial, modulus);
            AssertAddition(a, b, c, field);
        }
        
        [TestCase(13, 7, 10, 19, 16)]
        public void Subtraction(int a, int b, int c, int reductionPolynomial, int modulus)
        {
            var field = new BinaryField(reductionPolynomial, modulus);
            AssertSubtraction(a, b, c, field);
        }
        
        [TestCase(13, 7, 5, 19, 16)]
        [TestCase(83, 202, 1, 283, 256)]
        public void Multiplication(int a, int b, int c, int reductionPolynomial, int modulus)
        {
            var field = new BinaryField(reductionPolynomial, modulus);
            AssertMultiplication(a, b, c, field);
        }
        
        [TestCase(13, 4, 19, 16)]
        [TestCase(83, 202, 283, 256)]
        [TestCase(202, 83, 283, 256)]
        public void Inversion(int a, int c, int reductionPolynomial, int modulus)
        {
            var field = new BinaryField(reductionPolynomial, modulus);
            AssertInversion(a, c, field);
        }   
    }
}