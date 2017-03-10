using System.Linq;
using Core.Cryptography;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.Exceptions;
using DistributedCurrency.Extensions;

namespace DistributedCurrency.Workers
{
    public static class TransactionValidator
    {
        public static bool IsClosed(Transaction transaction)
            => transaction.GetFinalBytes().Aggregate(0, (pr, next) => (pr + next) % 256) == 0;

        public static void ValidateClosed(Transaction trans)
        {
            ValidateSign(trans.MinerPublicKey, trans.MinerSign, trans.GetFinalBytes(), "майнера");

            if (!IsClosed(trans))
                throw new TransactionValidateException("Транзакция не закрыта");

            if (trans.MinerPublicKey.SequenceEqual(trans.SenderPublicKey))
                throw new TransactionValidateException("Отправитель не может закрывать транзакцию");
        }

        public static void ValidateVerified(Transaction trans)
        {
            ValidateSign(trans.VerifierPublicKey, trans.VerifierSign, trans.GetVerifyBytes(), "подтверждающего");

            if (trans.VerifierPublicKey.SequenceEqual(trans.SenderPublicKey))
                throw new TransactionValidateException("Транзакция не была заверена сторонним лицом");
        }

        public static void ValidateInitial(Transaction trans)
        {
            ValidateSign(trans.SenderPublicKey, trans.SenderSign, trans.GetInitialBytes(), "отправителя");

            if (trans.MinerReward <= 0)
                throw new TransactionValidateException("Указана неверная награда для майнера");

            if (trans.ReciverPublicKey == null)
                throw new TransactionValidateException("Не указан получатель");

            if (trans.Coins < 0 || trans.SurplusCoins < 0)
                throw new TransactionValidateException("Кол-во монет на входе транзакции не соответствует кол-ву монет на выходе");

            var solvency = SolvencyCounter.Count(trans.SenderPublicKey, trans.SourceId, trans.ExtraSourceId);
            if (solvency.Coins != trans.CountFullUsedCoins())
                throw new TransactionValidateException("Кол-во монет на входе транзакции не соответствует кол-ву монет на выходе");
        }

        private static void ValidateSign(byte[] publicKey, byte[] sign, byte[] data, string whos)
        {
            if (publicKey == null || sign == null)
                throw new TransactionValidateException($"Нет ключа и\\или подписи {whos}");

            using (var csp = new RSACryptography(publicKey))
                if (!csp.VerifySign(data, sign))
                    throw new TransactionValidateException($"Подпись {whos} не соответствует транзакции");
        }
    }
}