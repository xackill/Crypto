using System;
using System.Linq;
using Core;
using Core.Cryptography;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Extensions;
using DistributedCurrency.Workers;

namespace DistributedCurrency.Factories
{
    public static class TransactionFactory
    {
        public static Transaction CreateFirst(byte[] userPublicKey)
            => CreateTransaction(userPublicKey, userPublicKey, Guid.Empty, null, null);

        public static Transaction CreateTransfer(byte[] senderPublicKey, byte[] reciverPublicKey, Guid sourceId, int coinsForTransfer)
            => CreateTransaction(senderPublicKey, reciverPublicKey, sourceId, null, coinsForTransfer);

        public static Transaction CreateUnion(byte[] senderPublicKey, Guid sourceId, Guid extraSourceId)
            => CreateTransaction(senderPublicKey, senderPublicKey, sourceId, extraSourceId, null);

        private static Transaction CreateTransaction(byte[] senderPublicKey, byte[] reciverPublicKey, 
            Guid sourceId, Guid? extraSourceId, int? coinsForTransfer)
        {
            if (extraSourceId.HasValue && coinsForTransfer.HasValue)
                throw new Exception("Не определен тип операции");

            using (var context = new DistributedCurrencyContext())
            {
                var publicPrivateKeyQuery =
                    from c in context.Contacts
                    join w in context.Wallets on c.Id equals w.Id
                    where c.PublicKey == senderPublicKey
                    select w.PublicPrivateKey;

                var senderPublicPrivateKey = publicPrivateKeyQuery.First();

                using (var csp = new RSACryptography(senderPublicPrivateKey))
                {
                    var solvency = SolvencyCounter.Count(csp.PublicKey, sourceId, extraSourceId);
                    var coins = coinsForTransfer ?? solvency.Coins;

                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        MinerReward = DCSecret.MinerReward,

                        SourceId = sourceId,
                        ExtraSourceId = extraSourceId,
                        ReciverPublicKey = reciverPublicKey,

                        Coins = coins,
                        SenderPublicKey = csp.PublicKey,

                        SurplusCoins = solvency.Coins - coins
                    };

                    transaction.SenderSign = csp.Sign(transaction.GetInitialBytes());
                    return transaction;
                }
            }
        }
    }
}
