using System.Numerics;

namespace EllipticCurves.DataModels.FiniteFields
{
    public abstract class FiniteField
    {
        protected readonly BigInteger Modulus;

        protected FiniteField(BigInteger modulus)
        {
            Modulus = modulus;
        }

        public abstract FiniteFieldValue Negative(BigInteger a);
        public abstract FiniteFieldValue Inverse(BigInteger a);

        public abstract FiniteFieldValue Add(BigInteger a, BigInteger b);
        public abstract FiniteFieldValue Multiply(BigInteger a, BigInteger b);

        public abstract string ToString(BigInteger a);

        public override bool Equals(object obj)
        {
            return obj != null && GetType() == obj.GetType() && Modulus == ((FiniteField) obj).Modulus;
        }

        public override int GetHashCode()
        {
            return Modulus.GetHashCode();
        }

        public static bool operator ==(FiniteField a, FiniteField b)
        {
            return a?.Equals(b) ?? b?.Equals(null) ?? false;
        }

        public static bool operator !=(FiniteField a, FiniteField b)
        {
            return !(a == b);
        }
        
        protected FiniteFieldValue CreateFiniteFieldValue(BigInteger a)
        {
            return new FiniteFieldValue(a, this);
        }
    }
}