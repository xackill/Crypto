using System;
using System.Security.Cryptography;

namespace Core.Cryptography
{
    public class RSACryptography : IDisposable
    {
        private readonly RSACryptoServiceProvider csp;

        public RSACryptography()
        {
            csp = new RSACryptoServiceProvider(1024);
        }

        public RSACryptography(byte [] key)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportCspBlob(key);
        }

        public void Dispose()
        {
            csp.Dispose();
        }

        public byte[] PublicKey => csp.ExportCspBlob(false);
        public byte[] PublicPrivateKey => csp.ExportCspBlob(true);

        public byte[] Sign(byte[] data) => csp.SignData(data, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        public bool VerifySign(byte[] data, byte[] sign) => csp.VerifyData(data, sign, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
    }
}
