using EllipticCurves.DataModels;
using SmartParser = Core.Helpers.BigIntegerSmartParser;

namespace EllipticCurves.Helpers
{
    public static class EllipticParser
    {
        public static EllipticCurve ParseCurve(string a, string b, string modulus)
        {
            return new EllipticCurve(SmartParser.Parse(a), SmartParser.Parse(b), SmartParser.Parse(modulus));
        }

        public static EllipticCurvePoint ParsePoint(string x, string y)
        {
            return new EllipticCurvePoint(SmartParser.Parse(x), SmartParser.Parse(y));
        }
    }
}