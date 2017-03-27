using System.Numerics;
using ProbabilisticEncryption.DataBaseModels;

namespace ProbabilisticEncryption.Workers
{
    public static class BBSGeneratorFactory
    {
        public static BBSGenerator Initialize(KeyContainer keyContainer)
        {
            var p = new BigInteger(keyContainer.P);
            var q = new BigInteger(keyContainer.Q);
            
            return new BBSGenerator(p, q);
        }

        public static BBSGenerator Initialize(KeyContainer keyContainer, EncryptedMessageContainer encryptedMessageContainer)
        {
            var p = new BigInteger(keyContainer.P);
            var q = new BigInteger(keyContainer.Q);

            var t = encryptedMessageContainer.Message.Length * 8 + 1;
            var xt = new BigInteger(encryptedMessageContainer.Xt);

            return new BBSGenerator(p, q, t, xt);
        }
    }
}