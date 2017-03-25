using Core;
using Core.Workers;

namespace ProbabilisticEncryption.Workers
{
    public static class BBSGeneratorFactory
    {
        public static BBSGenerator CreateNew()
        {
            var p = IntFactory.GeneratePrimeCongruent3Modulo4(PESecret.BytesCountP);
            var q = IntFactory.GeneratePrimeCongruent3Modulo4(PESecret.BytesCountQ);

            return new BBSGenerator(p, q);
        }
    }
}