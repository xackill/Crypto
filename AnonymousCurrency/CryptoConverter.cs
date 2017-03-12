using AnonymousCurrency.DataModels;
using Core.Cryptography;

namespace AnonymousCurrency.Helpers
{
    public static class CryptoConverter
    {
        public static byte[] Encrypt(IExtremelySerializable obj, RSACryptography csp)
        {
            var bytes = obj.ExtremelySerialize();
            return csp.Encrypt(bytes);
        }

        public static T Decrypt<T>(byte[] encryptedBytes, RSACryptography csp) 
            where T : IExtremelySerializable, new()
        {
            var decryptedBytes = csp.Decrypt(encryptedBytes);
            var obj = new T();
            obj.InitByDeserializing(decryptedBytes);
            return obj;
        }
    }
}