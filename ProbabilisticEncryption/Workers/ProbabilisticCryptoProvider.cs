using System;
using Core.Extensions;
using ProbabilisticEncryption.DataBaseModels;

namespace ProbabilisticEncryption.Workers
{
    public static class ProbabilisticCryptoProvider
    {
        public static EncryptedMessageContainer Encrypt(KeyContainer keyContainer, string message)
        {
            var ibytes = message.ToBytes();
            var bbsGenerator = BBSGeneratorFactory.Initialize(keyContainer);

            var obytes = new byte[ibytes.Length];
            for (var i = 0; i < ibytes.Length; ++i)
            {
                obytes[i] = (byte) (ibytes[i] ^ bbsGenerator.GetByte(i * 8 + 1));
            }

            return new EncryptedMessageContainer
            {
                Id = Guid.NewGuid(),
                Message = obytes,
                Xt = bbsGenerator.GetX(ibytes.Length).ToByteArray()
            };
        }

        public static string Decrypt(KeyContainer keyContainer, EncryptedMessageContainer encryptedMessageContainer)
        {
            var obytes = encryptedMessageContainer.Message;
            var bbsGenerator = BBSGeneratorFactory.Initialize(keyContainer, encryptedMessageContainer);

            var ibytes = new byte[obytes.Length];
            for (var i = 0; i < obytes.Length; ++i)
            {
                ibytes[i] = (byte)(obytes[i] ^ bbsGenerator.GetByte(i * 8 + 1));
            }

            return ibytes.ConvertToString();
        }
    }
}