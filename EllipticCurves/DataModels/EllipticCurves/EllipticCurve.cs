using System;
using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.DataModels.EllipticCurves
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
            return 4 * (A * A * A) + 27 * (B * B) == 0;
        }
    }
}