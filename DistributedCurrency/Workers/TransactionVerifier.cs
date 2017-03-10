using Core.Cryptography;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Extensions;

namespace DistributedCurrency.Workers
{
    public static class TransactionVerifier
    {
        public static void Verify(Transaction transact, byte[] verifierPublicPrivateKey)
        {
            TransactionValidator.ValidateInitial(transact);
            using (var csp = new RSACryptography(verifierPublicPrivateKey))
            {
                transact.VerifierPublicKey = csp.PublicKey;
                transact.VerifierSign = csp.Sign(transact.GetVerifyBytes());
            }
        }
    }
}