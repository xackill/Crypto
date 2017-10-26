using System;
using Core;
using Core.Helpers;
using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using SmartParser = Core.Helpers.BigIntegerSmartParser;

namespace EllipticCurves.Helpers
{
    public static class EllipticParser
    {
        public static FiniteField ParsePrimeField(string p)
        {
            var modulus = SmartParser.Parse(p);
            if (!RabinMillerHelper.IsPrime(modulus, Secret.PrimeCertainty))
                throw new Exception("p - не простое");

            return new PrimeField(modulus);
        }
        
        public static FiniteField ParseBinaryField(string f, string p)
        {
            var modulus = SmartParser.Parse(p);
            if (!modulus.IsPowerOfTwo)
                throw new Exception("p - не степень числа 2");

            var reductionPolynomial = SmartParser.Parse(f);
            // check
            
            return new BinaryField(reductionPolynomial, modulus);
        }
        
        public static EllipticCurve ParsePrimeEllipticCurve(string a, string b, FiniteField field)
        {
            var (aValue, bValue) = Convert(a, b, field);
            return new PrimeEllipticCurve(aValue, bValue);
        }
        
        public static EllipticCurve ParseNonSupersingularEllipticCurve(string a, string b, FiniteField field)
        {
            var (aValue, bValue) = Convert(a, b, field);
            return new NonSupersingularEllipticCurve(aValue, bValue);
        }

        public static EllipticCurvePoint ParsePoint(string x, string y, FiniteField field)
        {
            var (xValue, yValue) = Convert(x, y, field);
            return new EllipticCurvePoint(xValue, yValue);
        }

        private static (FiniteFieldValue, FiniteFieldValue) Convert(string x, string y, FiniteField field)
        {
            return (CreateFiniteFieldValue(x, field), CreateFiniteFieldValue(y, field));
        }

        private static FiniteFieldValue CreateFiniteFieldValue(string a, FiniteField finiteField)
        {
            var value = SmartParser.Parse(a);
            return new FiniteFieldValue(value, finiteField);
        }
    }
}