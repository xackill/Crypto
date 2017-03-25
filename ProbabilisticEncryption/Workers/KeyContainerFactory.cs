using System;
using Core.Workers;
using ProbabilisticEncryption.DataBaseModels;

namespace ProbabilisticEncryption.Workers
{
    public static class KeyContainerFactory
    {
        public static KeyContainer CreateNew()
        {
            var p = IntFactory.GenerateBlumPrime(PESecret.BytesCountP);
            var q = IntFactory.GenerateBlumPrime(PESecret.BytesCountQ);

            return new KeyContainer
            {
                Id = Guid.NewGuid(),
                P = p.ToByteArray(),
                Q = q.ToByteArray()
            };
        }
    }
}