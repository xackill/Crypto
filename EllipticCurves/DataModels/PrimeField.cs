using System.Numerics;

namespace EllipticCurves.DataModels
{    
    public class PrimeField : FiniteField
    {
        public PrimeField(BigInteger modulus) : base(modulus)
        {
        }

        public override FiniteFieldValue Negative(BigInteger a)
        {
            return CreateFiniteFieldValue(-a);
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

        public override string ToString(BigInteger a)
        {
            return $"0x{a.ToString("X")}";
        }

        private FiniteFieldValue CreateFiniteFieldValue(BigInteger a)
        {
            return new FiniteFieldValue(a, this);
        }
        
        private BigInteger Normalize(BigInteger a)
        {
            a %= Modulus;
            return a >= 0 ? a : a + Modulus;
        }
    }
}