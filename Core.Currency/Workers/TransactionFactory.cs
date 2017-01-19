using System;
using Core.Currency.Cryptography;
using Core.Currency.DataBaseModels;
using Core.Currency.Extensions;

namespace Core.Currency.Workers
{
    public static class TransactionFactory
    {
        public static Transaction CreateFirst(Guid userId)
            => CreateTransaction(userId, userId, Guid.Empty, null, null);

        public static Transaction CreateTransfer(Guid senderId, Guid reciverId, Guid sourceId, int coinsForTransfer)
            => CreateTransaction(senderId, reciverId, sourceId, null, coinsForTransfer);

        public static Transaction CreateUnion(Guid senderId, Guid sourceId, Guid extraSourceId)
            => CreateTransaction(senderId, senderId, sourceId, extraSourceId, null);

        private static Transaction CreateTransaction(Guid senderId, Guid reciverId, Guid sourceId, Guid? extraSourceId, int? coinsForTransfer)
        {
            if (extraSourceId.HasValue && coinsForTransfer.HasValue)
                throw new Exception("Не определен тип операции");

            using (var context = new CurrencyContext())
            {
                var senderWallet = context.Read<Wallet>(senderId);
                var reciverContact = context.Read<Contact>(reciverId);

                using (var csp = new RSACryptography(senderWallet.PublicPrivateKey))
                {
                    var solvency = SolvencyCounter.Count(csp.PublicKey, sourceId, extraSourceId);
                    var coins = coinsForTransfer ?? solvency.Coins;

                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        MinerReward = Secret.MinerReward,

                        SourceId = sourceId,
                        ExtraSourceId = extraSourceId,
                        ReciverPublicKey = reciverContact.PublicKey,

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
