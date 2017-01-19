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

        public static void CloseTransaction(Transaction transact, Guid minerId)
        {
            TransactionValidator.ValidateVerified(transact);

            using (var context = new CurrencyContext())
            {
                var minerPublicPrivateKey = context.Read<Wallet>(minerId).PublicPrivateKey;
                using (var csp = new RSACryptography(minerPublicPrivateKey))
                {
                    transact.MinerPublicKey = csp.PublicKey;

                    while (!TransactionValidator.IsClosed(transact))
                    {
                        ++transact.ClousingByte;
                        Thread.Sleep(Rand.Next(50));
                    }

                    transact.MinerSign = csp.Sign(transact.GetFinalBytes());
                }
            }
        }
    }
}