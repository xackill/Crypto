using System;
using System.Linq;
using Core.Extensions;

namespace AnonymousCurrency.DataModels
{
    public class EnvelopeContent : IExtremelySerializable
    {
        public Guid Id { get; set; }
        public int Balance { get; set; }

        public byte[] ExtremelySerialize()
        {
            var idAsBytes = Id.ToByteArray();
            var coinsAsBytes = BitConverter.GetBytes(Balance);
            return idAsBytes.ConcatBytes(coinsAsBytes);
        }

        public void InitByDeserializing(byte[] bytes)
        {
            if (bytes.Length != 20)
                throw new Exception($"Байтовый массив поврежден: Необходимо 20 байт, а сейчас {bytes.Length}");

            Id = new Guid(bytes.Take(16).ToArray());
            Balance = BitConverter.ToInt32(bytes, startIndex: 16);
        }
    }
}