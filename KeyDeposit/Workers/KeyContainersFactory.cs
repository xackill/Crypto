using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using KeyDeposit.DataBaseModels;
using KeyDeposit.DataModels;
using KeyDeposit.Enums;

namespace KeyDeposit.Workers
{
    public static class KeyContainersFactory
    {
        public static KeyContainer[] CreateForTrustedCenters(KeySource source)
        {
            var keyId = Guid.NewGuid();

            var trustedContainers = new List<KeyContainer>(KDSecret.TrustedCenterCount);
            for (var i = 0; i < KDSecret.TrustedCenterCount; ++i)
            {
                var s = IntFactory.GenerateRandom();
                var t = BigInteger.ModPow(source.G, s, source.P);

                var container = new KeyContainer
                {
                    Id = Guid.NewGuid(),
                    KeyId = keyId,
                    Keeper = KeyKeeper.TrustedCenter,
                    Modulus = source.P.ToByteArray(),

                    PublicKey = t.ToByteArray(),
                    PrivateKey = s.ToByteArray()
                };

                trustedContainers.Add(container);
            }

            return trustedContainers.ToArray();
        }

        public static KeyContainer CreateForCreator(KeyContainer[] trusted)
        {
            var keyId = trusted[0].KeyId;
            var modulus = new BigInteger(trusted[0].Modulus);

            var (t, s) = ConstructPublicPrivateKeys(trusted, modulus);

            return new KeyContainer
            {
                Id = Guid.NewGuid(),
                KeyId = keyId,
                Keeper = KeyKeeper.Creator,
                Modulus = modulus.ToByteArray(),

                PublicKey = t.ToByteArray(),
                PrivateKey = s.ToByteArray()
            };
        }

        public static KeyContainer CreateForDepositCenter(KeyContainer creatorContainer)
        {
            return new KeyContainer
            {
                Id = Guid.NewGuid(),
                KeyId = creatorContainer.KeyId,
                Keeper = KeyKeeper.DepositCenter,
                Modulus = creatorContainer.Modulus,

                PublicKey = creatorContainer.PublicKey
            };
        }

        public static KeyContainer CreateForState(KeyContainer[] trusted)
        {
            var keyId = trusted[0].KeyId;
            var modulus = new BigInteger(trusted[0].Modulus);

            var (t, s) = ConstructPublicPrivateKeys(trusted, modulus);
            
            return new KeyContainer
            {
                Id = Guid.NewGuid(),
                KeyId = keyId,
                Keeper = KeyKeeper.State,
                Modulus = modulus.ToByteArray(),

                PublicKey = t.ToByteArray(),
                PrivateKey = s.ToByteArray()
            };
        }

        private static (BigInteger t, BigInteger s) ConstructPublicPrivateKeys(KeyContainer[] trusted, BigInteger modulus)
            => (trusted.Select(c => new BigInteger(c.PublicKey)).Aggregate((sum, next) => (sum + next) % modulus),
                trusted.Select(c => new BigInteger(c.PrivateKey)).Aggregate((mul, next) => (mul * next) % modulus));
    }
}