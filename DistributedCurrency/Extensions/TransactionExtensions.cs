using System.Linq;
using Core.Extensions;
using DistributedCurrency.DataBaseModels;

namespace DistributedCurrency.Extensions
{
    public static class TransactionExtensions
    {
        public static bool IsUserSender(this Transaction transaction, byte[] userPublicKey)
            => transaction.SenderPublicKey?.SequenceEqual(userPublicKey) ?? false;

        public static bool IsUserReciver(this Transaction transaction, byte[] userPublicKey)
            => transaction.ReciverPublicKey?.SequenceEqual(userPublicKey) ?? false;

        public static bool IsUserMiner(this Transaction transaction, byte[] userPublicKey)
            => transaction.MinerPublicKey?.SequenceEqual(userPublicKey) ?? false;

        public static int CountUserCoins(this Transaction transaction, byte[] userPublicKey)
        {
            var coinCount = 0;

            if (transaction.IsUserReciver(userPublicKey))
                coinCount += transaction.Coins;

            if (transaction.IsUserSender(userPublicKey))
                coinCount += transaction.SurplusCoins;

            if (transaction.IsUserMiner(userPublicKey))
                coinCount += transaction.MinerReward;       

            return coinCount;
        }

        public static int CountFullUsedCoins(this Transaction transaction)
            => transaction.Coins + transaction.SurplusCoins;

        public static byte[] GetInitialBytes(this Transaction transaction)
            => GetInitialData(transaction).ToBytes();

        public static byte[] GetVerifyBytes(this Transaction transaction)
            => GetVerifyData(transaction).ToBytes();

        public static byte[] GetFinalBytes(this Transaction transaction)
            => GetFinalData(transaction).ToBytes();

        private static string GetInitialData(Transaction t)
            => $"{t.Id}{t.MinerReward}{t.SourceId}{t.ExtraSourceId}{t.ReciverPublicKey.ToBase64()}{t.Coins}{t.SenderPublicKey.ToBase64()}{t.SurplusCoins}";

        private static string GetVerifyData(Transaction t)
            => $"{GetInitialData(t)}{t.SenderSign.ToBase64()}{t.VerifierPublicKey.ToBase64()}";

        private static string GetFinalData(Transaction t)
            => $"{GetVerifyData(t)}{t.VerifierSign.ToBase64()}{t.MinerPublicKey.ToBase64()}{(char)t.ClosingByte}";
    }
}
