using System;
using System.Linq;
using System.Numerics;
using Core.Extensions;

namespace AnonymousCurrency.DataModels
{
    public class EnvelopeSecret : IExtremelySerializable
    {
        public Guid Id { get; set; }
        public BigInteger K { get; set; }
        public BigInteger B { get; set; }
        
        public byte[] ExtremelySerialize()
        {
            var idAsBytes = Id.ToByteArray();
            var kAsBytes = K.ToByteArray();
            var bAsBytes = B.ToByteArray();

            return idAsBytes
                .ConcatBytes((byte)kAsBytes.Length)
                .ConcatBytes(kAsBytes)
                .ConcatBytes(bAsBytes);
        }

        public void InitByDeserializing(byte[] bytes)
        {
            try
            {
                Id = new Guid(bytes.Take(16).ToArray());

                var kSize = (int)bytes[16];
                K = new BigInteger(bytes.Skip(17).Take(kSize).ToArray());
                B = new BigInteger(bytes.Skip(17+kSize).ToArray());
            }
            catch (Exception)
            {
                throw new Exception("Байтовый массив поврежден");
            }
        }

        public bool Equals(EnvelopeSecret other)
        {
            return Id == other.Id && K == other.K && B == other.B;
        }
    }
}