using System;
using System.Threading;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;
using Core.Currency.Extensions;

namespace Core.Currency.Workers
{
    public static class Miner
    {
        private static readonly Random Rand = new Random();

        public static void CloseTransaction(Transaction transact, byte[] minerPublicPrivateKey)
        {
            TransactionValidator.ValidateVerified(transact);

            using (var csp = new RSACryptography(minerPublicPrivateKey))
            {
                transact.MinerPublicKey = csp.PublicKey;

                while (!TransactionValidator.IsClosed(transact))
                {
                    ++transact.ClosingByte;
                    Thread.Sleep(Rand.Next(70));
                }

                transact.MinerSign = csp.Sign(transact.GetFinalBytes());
            }
        }
    }
}