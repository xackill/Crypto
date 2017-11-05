using System;
using System.Numerics;

namespace EllipticCurves.DataModels.FiniteFields
{
    public class FiniteFieldValue
    {
        private readonly BigInteger value;
        private readonly FiniteField finiteField;

        public FiniteFieldValue(BigInteger value, FiniteField finiteField)
        {
            this.value = finiteField.Normalize(value);
            this.finiteField = finiteField;
        }

        public static FiniteFieldValue operator +(FiniteFieldValue a, FiniteFieldValue b)
        {
            ThrowIfFiniteFieldsAreDifferent(a, b);
            return a.finiteField.Add(a.value, b.value);
        }
        
        public static FiniteFieldValue operator *(FiniteFieldValue a, FiniteFieldValue b)
        {
            ThrowIfFiniteFieldsAreDifferent(a, b);
            return a.finiteField.Multiply(a.value, b.value);
        }

        public static FiniteFieldValue operator -(FiniteFieldValue a)
        {
            return a.finiteField.Negative(a.value);
        }

        public static FiniteFieldValue operator -(FiniteFieldValue a, FiniteFieldValue b)
        {
            ThrowIfFiniteFieldsAreDifferent(a, b);
            return Subtract(a.value, b.value, a.finiteField);
        }

        public static FiniteFieldValue operator /(FiniteFieldValue a, FiniteFieldValue b)
        {
            ThrowIfFiniteFieldsAreDifferent(a, b);
            return Divide(a.value, b.value, a.finiteField);
        }
        
        public static bool operator ==(FiniteFieldValue a, object b) 
            => a?.Equals(b) ?? b?.Equals(null) ?? false;

        public static bool operator !=(FiniteFieldValue a, object b) 
            => !(a == b);

        public static FiniteFieldValue operator +(BigInteger a, FiniteFieldValue b) 
            => b.finiteField.Add(a, b.value);

        public static FiniteFieldValue operator +(FiniteFieldValue a, BigInteger b) 
            => a.finiteField.Add(a.value, b);
        
        public static FiniteFieldValue operator *(BigInteger a, FiniteFieldValue b) 
            => b.finiteField.Multiply(a, b.value);

        public static FiniteFieldValue operator *(FiniteFieldValue a, BigInteger b)
            => a.finiteField.Multiply(a.value, b);

        public static FiniteFieldValue operator -(BigInteger a, FiniteFieldValue b)
            => Subtract(a, b.value, b.finiteField);

        public static FiniteFieldValue operator -(FiniteFieldValue a, BigInteger b)
            => Subtract(a.value, b, a.finiteField);
        
        public static FiniteFieldValue operator /(BigInteger a, FiniteFieldValue b)
            => Divide(a, b.value, b.finiteField);

        public static FiniteFieldValue operator /(FiniteFieldValue a, BigInteger b) 
            => Divide(a.value, b, a.finiteField);

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case int intValue:
                    return value == intValue;
                case BigInteger bigIntValue:
                    return value == bigIntValue;
                case FiniteFieldValue finiteFieldValue:
                    return value == finiteFieldValue.value && finiteField == finiteFieldValue.finiteField;
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (value.GetHashCode() * 397) ^ (finiteField != null ? finiteField.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return finiteField.ToString(value);
        }

        private static void ThrowIfFiniteFieldsAreDifferent(FiniteFieldValue a, FiniteFieldValue b)
        {
            if (a.finiteField != b.finiteField)
                throw new Exception("Действия с разными полями неопределены");
        }

        private static FiniteFieldValue Subtract(BigInteger a, BigInteger b, FiniteField field)
        {
            return field.Add(a, field.Negative(b).value);
        }
        
        private static FiniteFieldValue Divide(BigInteger a, BigInteger b, FiniteField field)
        {
            var inversed = field.Inverse(b);
            return a == BigInteger.One ? inversed : field.Multiply(a, inversed.value);
        }
    }
}