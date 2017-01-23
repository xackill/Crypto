using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;
using Core.Currency.Extensions;

namespace Core.Currency.Workers
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