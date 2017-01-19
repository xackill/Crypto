using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;
using Core.Currency.Extensions;

namespace Core.Currency.Workers
{
    public static class TransactionVerifier
    {
        public static void Verify(Transaction transact, Guid walletId)
        {
            TransactionValidator.ValidateInitial(transact);
            using (var context = new CurrencyContext())
            {
                var walletPublicPrivateKey = context.Read<Wallet>(walletId).PublicPrivateKey;
                using (var csp = new RSACryptography(walletPublicPrivateKey))
                {
                    transact.VerifierPublicKey = csp.PublicKey;
                    transact.VerifierSign = csp.Sign(transact.GetVerifyBytes());
                }
            }
        }
    }
}