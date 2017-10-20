using System;
using System.Numerics;

namespace EllipticCurves.DataModels
{
    public sealed class EllipticCurve
    {
        public readonly BigInteger A;
        public readonly BigInteger B;
        public readonly BigInteger Modulus;

        public EllipticCurve(BigInteger a, BigInteger b, BigInteger modulus)
        {
            A = a;
            B = b;
            Modulus = modulus;
            
            if (IsSingular())
                throw new Exception($"{ToString()} сингулярна");
        }

        public override string ToString()
        {
            return $"[EC: a=0x{A.ToString("X")}, b=0x{B.ToString("X")}, p=0x{Modulus.ToString("X")}]";
        }

        private bool IsSingular()
        {
            var a3 = A * A * A;
            var b2 = B * B;
            
            return (4 * a3 + 27 * b2) % Modulus == BigInteger.Zero;
        }
    }
}