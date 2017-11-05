using System.Numerics;

namespace EllipticCurves.DataModels.FiniteFields
{    
    public class PrimeField : FiniteField
    {
        public PrimeField(BigInteger modulus) : base(modulus)
        {
        }

        public override FiniteFieldValue Negative(BigInteger a)
        {
            return CreateFiniteFieldValue(Normalize(-a));
        }

        public override FiniteFieldValue Inverse(BigInteger a)
        {
            var inversed = BigInteger.ModPow(a, Modulus - 2, Modulus);
            return CreateFiniteFieldValue(inversed);
        }

        public override FiniteFieldValue Add(BigInteger a, BigInteger b)
        {
            return CreateFiniteFieldValue(Normalize(a + b));
        }

        public override FiniteFieldValue Multiply(BigInteger a, BigInteger b)
        {
            return CreateFiniteFieldValue(Normalize(a * b));
        }

        public override BigInteger Normalize(BigInteger a)
        {
            a %= Modulus;
            return a >= 0 ? a : a + Modulus;
        }

        public override string ToString(BigInteger a)
        {
            return $"0x{a.ToString("X")}";
        }
    }
}