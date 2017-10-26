using System.Numerics;
using Core.Extensions;

namespace EllipticCurves.DataModels.FiniteFields
{
    public class BinaryField : FiniteField
    {
        private readonly BigInteger reductionPolynomial;
        
        public BinaryField(BigInteger reductionPolynomial, BigInteger modulus) 
            : base(modulus.DegreeOfBinaryPolynomial())
        {
            this.reductionPolynomial = reductionPolynomial;
        }

        public override FiniteFieldValue Negative(BigInteger a)
        {
            return CreateFiniteFieldValue(a);
        }

        public override FiniteFieldValue Inverse(BigInteger a)
        {
            // Algorithm 2.48
            var (u, v) = (a, reductionPolynomial);
            var (g1, g2) = (BigInteger.One, BigInteger.Zero);

            while (u != BigInteger.One)
            {
                var j = u.DegreeOfBinaryPolynomial() - v.DegreeOfBinaryPolynomial();
                if (j < 0)
                {
                    (u, v, g1, g2, j) = (v, u, g2, g1, -j);
                }

                u ^= v << j;
                g1 ^= g2 << j;
            }

            return CreateFiniteFieldValue(g1);
        }

        public override FiniteFieldValue Add(BigInteger a, BigInteger b)
        {
            return CreateFiniteFieldValue(a ^ b);
        }

        public override FiniteFieldValue Multiply(BigInteger a, BigInteger b)
        {
            var c = MultiplyInternal(a, b);
            return CreateFiniteFieldValue(c);
        }

        public override string ToString(BigInteger a)
        {
            return $"0x{a.ToString("X")}";
        }

        private BigInteger MultiplyInternal(BigInteger a, BigInteger b)
        {
            // Algorithm 2.33
            var binaryString = a.ToBinaryString();
            var c = binaryString[0] == '0' ? 0 : b;

            for (var i = 1; i < binaryString.Length; ++i)
            {
                b *= 2;
                if (b.DegreeOfBinaryPolynomial() == Modulus)
                    b ^= reductionPolynomial;

                if (binaryString[i] == '1')
                    c ^= b;
            }
            return c;
        }
    }
}