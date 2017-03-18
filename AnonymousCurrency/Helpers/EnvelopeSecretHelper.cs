using System;
using System.Numerics;
using AnonymousCurrency.DataModels;
using Core.Extensions;

namespace AnonymousCurrency.Helpers
{
    public static class EnvelopeSecretHelper
    {
        private static readonly Random Rand = new Random();

        public static bool IsSecretValid(EnvelopeSecret secret, Guid ownerId, Guid contentId)
        {
            var linear = secret;
            var point = GuidToPoint(ownerId);

            return secret.Id == contentId && point.Y == linear.K * point.X + linear.B;
        }

        public static Guid RevealThePerson(EnvelopeSecret first, EnvelopeSecret second)
        {
            var personX = (second.B - first.B) / (first.K - second.K);
            var personY = first.K * personX + first.B;

            var personPoint = new Point { X = personX, Y = personY };
            return PointToGuid(personPoint);
        }

        public static EnvelopeSecret GenerateSecret(Guid ownerId, Guid envelopeId)
        {
            var point = GuidToPoint(ownerId);

            var k = new BigInteger(Rand.Next());
            var b = point.Y - (k * point.X);

            return new EnvelopeSecret {Id = envelopeId, K = k, B = b};
        }

        private class Point
        {
            public BigInteger X;
            public BigInteger Y;
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
            var xAsBytes = point.X.ToByteArray();
            var yAsBytes = point.Y.ToByteArray();

            return new Guid(xAsBytes.ConcatBytes(yAsBytes));
        }
    }
}