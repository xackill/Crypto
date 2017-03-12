using System;
using AnonymousCurrency.DataBaseModels;
using AnonymousCurrency.DataModels;
using Core.Extensions;

namespace AnonymousCurrency.Helpers
{
    public static class EnvelopeSecretHelper
    {
        private static readonly Random Rand = new Random((int) (DateTime.Now.Ticks & int.MaxValue));

        public static bool IsSecretBelongsToOwner(EnvelopeSecret secret, BankCustomer owner)
        {
            var linear = secret;
            var point = GuidToPoint(owner.Id);

            return point.Y == linear.K * point.X + linear.B;
        }

        public static Guid RevealThePerson(EnvelopeSecret first, EnvelopeSecret second)
        {
            if (first.Equals(second))
                throw new Exception("Секреты одинаковы!");

            var personX = (second.B - first.B) / (first.K - second.K);
            var personY = first.K * personX + first.B;

            var personPoint = new Point { X = personX, Y = personY };
            return PointToGuid(personPoint);
        }

        public static EnvelopeSecret GenerateSecret(Guid guid)
        {
            var point = GuidToPoint(guid);

            var k = Rand.Next() * ((Rand.Next() & 1) == 1 ? -1 : 1);
            var b = point.Y - k * point.X;

            return new EnvelopeSecret {K = k, B = b};
        }

        private class Point
        {
            public long X;
            public long Y;
        }

        private static Point GuidToPoint(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var x = BitConverter.ToInt64(bytes, startIndex: 0);
            var y = BitConverter.ToInt64(bytes, startIndex: 8);

            return new Point { X = x, Y = y };
        }

        private static Guid PointToGuid(Point point)
        {
            var xAsBytes = BitConverter.GetBytes(point.X);
            var yAsBytes = BitConverter.GetBytes(point.Y);

            return new Guid(xAsBytes.ConcatBytes(yAsBytes));
        }
    }
}