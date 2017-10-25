using System;
using Core;
using Core.Helpers;
using EllipticCurves.DataModels;
using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using SmartParser = Core.Helpers.BigIntegerSmartParser;

namespace EllipticCurves.Helpers
{
    public static class EllipticParser
    {
        public static FiniteField ParseField(string p)
        {
            var modulus = SmartParser.Parse(p);
            if (RabinMillerHelper.IsPrime(modulus, Secret.PrimeCertainty))
                return new PrimeField(modulus);
            
            throw new Exception("Неизвестный тип конечного поля");
        }
        
        public static EllipticCurve ParseCurve(string a, string b, FiniteField field)
        {
            var aValue = CreateFiniteFieldValue(a, field);
            var bValue = CreateFiniteFieldValue(b, field);
            
            return new EllipticCurve(aValue, bValue);
        }

        public static EllipticCurvePoint ParsePoint(string x, string y, FiniteField field)
        {
            var xValue = CreateFiniteFieldValue(x, field);
            var yValue = CreateFiniteFieldValue(y, field);
            
            return new EllipticCurvePoint(xValue, yValue);
        }

        private static FiniteFieldValue CreateFiniteFieldValue(string a, FiniteField finiteField)
        {
            var value = SmartParser.Parse(a);
            return new FiniteFieldValue(value, finiteField);
        }
    }
}