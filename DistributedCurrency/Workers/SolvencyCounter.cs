using System;
using System.Collections.Generic;
using System.Linq;
using DistributedCurrency.DataBaseModels;
using DistributedCurrency.DataModels;
using DistributedCurrency.Extensions;

namespace DistributedCurrency.Workers
{
    public static class SolvencyCounter
    {
        public static Solvency Count(byte[] userPublicKey, Guid fId, Guid? sId = null)
        {
            using (var context = new DistributedCurrencyContext())
            {
                var userId = context.Contacts.First(c => c.PublicKey == userPublicKey).Id;

                var wave = !sId.HasValue ? Wave.First : ResearchTransactChain(context, userId, fId, sId.Value);
                var solvency = new Solvency();

                if (wave.HasFlag(Wave.First))
                    solvency.Coins += GetSolvencyValueOfTransaction(context, fId, userPublicKey);
                if (wave.HasFlag(Wave.Second))
                    solvency.Coins += GetSolvencyValueOfTransaction(context, sId.Value, userPublicKey);

                if (sId.HasValue && wave != Wave.Both)
                    solvency.Message = $"Обнаружено пересечение: выбрана {wave:D}я транзакция";

                return solvency;
            }
        }

        private static int GetSolvencyValueOfTransaction(DistributedCurrencyContext context, Guid transactId, byte[] userPublicKey)
            => transactId == Guid.Empty ? 0 : context.Read<Transaction>(transactId).CountUserCoins(userPublicKey);

        [Flags]
        private enum Wave
        {
            First = 1,
            Second = 2,
            Both = 3
        }

        private class VisitedTransact
        {
            public Guid Id { get; set; }
            public Guid CreatorId { get; set; }            
            public Guid MinerId { get; set; }            
            public Guid[] SourcesIds { get; set; }

            public Guid VisitedByUserId { get; set; }
            public Wave VisitedInWave { get; set; }
        }

        private class Visiters
        {
            public Dictionary<Guid, Wave> Visits { get; set; }
        }

        private static VisitedTransact CreateVisitedTransact(DistributedCurrencyContext context, Guid sourseId, Wave wave, Guid? visitedByUserId = null)
        {
            var transact = context.Read<Transaction>(sourseId);
            TransactionValidator.ValidateClosed(transact);

            var creatorId = context.Contacts.First(c => c.PublicKey == transact.SenderPublicKey).Id;
            var minerId = context.Contacts.First(c => c.PublicKey == transact.MinerPublicKey).Id;

            return new VisitedTransact
            {
                Id = sourseId,
                CreatorId = creatorId,
                MinerId = minerId,
                SourcesIds = new [] {transact.SourceId, transact.ExtraSourceId ?? Guid.Empty },

                VisitedByUserId = visitedByUserId ?? Guid.Empty,
                VisitedInWave = wave
            };
        }

        private static Visiters CreateVisiters(VisitedTransact transact)
            => new Visiters { Visits = new Dictionary<Guid, Wave> { { transact.VisitedByUserId, transact.VisitedInWave} } };

        private static Wave ResearchTransactChain(DistributedCurrencyContext context, Guid userId, Guid fId, Guid sId)
        {
            var researchedTransactChain = new Dictionary<Guid, Visiters>();
            var bfsOrder = new Queue<VisitedTransact>(new[]
            {
                CreateVisitedTransact(context, fId, Wave.First, userId),
                CreateVisitedTransact(context, sId, Wave.Second, userId)
            });

            while (bfsOrder.Count > 0)
            {
                var newTransact = bfsOrder.Dequeue();

                if (researchedTransactChain.TryGetValue(newTransact.Id, out Visiters visiters))
                {
                    if (visiters.Visits.TryGetValue(newTransact.VisitedByUserId, out Wave visitedInWave))
                        return newTransact.VisitedInWave;

                    visiters.Visits[newTransact.VisitedByUserId] = newTransact.VisitedInWave;
                    continue;
                }
                researchedTransactChain[newTransact.Id] = CreateVisiters(newTransact);

                if (newTransact.MinerId == newTransact.VisitedByUserId)
                    continue;

                foreach (var sourceId in newTransact.SourcesIds)
                    if (sourceId != Guid.Empty)
                        bfsOrder.Enqueue(CreateVisitedTransact(context, sourceId, newTransact.VisitedInWave, newTransact.CreatorId));
            }

            return Wave.Both;
        }
    }
}
