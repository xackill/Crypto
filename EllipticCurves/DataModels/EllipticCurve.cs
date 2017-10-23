using System;

namespace EllipticCurves.DataModels
{
    public sealed class EllipticCurve
    {
        public readonly FiniteFieldValue A;
        public readonly FiniteFieldValue B;

        public EllipticCurve(FiniteFieldValue a, FiniteFieldValue b)
        {
            A = a;
            B = b;
            
            if (IsSingular())
                throw new Exception($"{this} сингулярна");
        }

        public override string ToString()
        {
            return $"[EC: a={A}, b={B}]";
        }

        private bool IsSingular()
        {
            var a3 = A * A * A;
            var b2 = B * B;
            
            return (4 * a3 + 27 * b2) == 0;
        }
    }
}