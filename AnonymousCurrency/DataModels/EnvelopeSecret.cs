using System;
using Core.Extensions;

namespace AnonymousCurrency.DataModels
{
    public class EnvelopeSecret : IExtremelySerializable
    {
        public long K { get; set; }
        public long B { get; set; }
        
        public byte[] ExtremelySerialize()
        {
            var kAsBytes = BitConverter.GetBytes(K);
            var bAsBytes = BitConverter.GetBytes(B);
            return kAsBytes.ConcatBytes(bAsBytes);
        }

        public void InitByDeserializing(byte[] bytes)
        {
            if (bytes.Length != 16)
                throw new Exception($"Байтовый массив поврежден: Необходимо 16 байт, а сейчас {bytes.Length}");

            K = BitConverter.ToInt64(bytes, startIndex: 0);
            B = BitConverter.ToInt64(bytes, startIndex: 8);
        }

        public bool Equals(EnvelopeSecret other)
        {
            return K == other.K && B == other.B;
        }
    }
}