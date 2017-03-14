using System;
using System.Linq;
using Core.Extensions;

namespace AnonymousCurrency.DataModels
{
    public class EnvelopeSecret : IExtremelySerializable
    {
        public Guid Id { get; set; }
        public long K { get; set; }
        public long B { get; set; }
        
        public byte[] ExtremelySerialize()
        {
            var idAsBytes = Id.ToByteArray();
            var kAsBytes = BitConverter.GetBytes(K);
            var bAsBytes = BitConverter.GetBytes(B);
            return idAsBytes.ConcatBytes(kAsBytes).ConcatBytes(bAsBytes);
        }

        public void InitByDeserializing(byte[] bytes)
        {
            if (bytes.Length != 32)
                throw new Exception($"Байтовый массив поврежден: Необходимо 32 байта, а сейчас {bytes.Length}");

            Id = new Guid(bytes.Take(16).ToArray());
            K = BitConverter.ToInt64(bytes, startIndex: 16);
            B = BitConverter.ToInt64(bytes, startIndex: 24);
        }

        public bool Equals(EnvelopeSecret other)
        {
            return Id == other.Id && K == other.K && B == other.B;
        }
    }
}