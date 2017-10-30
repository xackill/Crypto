using System;
using Core.Cryptography;

namespace AnonymousCurrency.Helpers
{
    public class BankSignChecker : IDisposable
    {
        private readonly RSACryptography csp;

        public BankSignChecker()
        {
            csp = new RSACryptography(ACSecret.BankPublicKey);
        }

        public void Dispose()
        {
            csp.Dispose();
        }

        public bool VerifySign(byte[] data, byte[] sign)
        {
            return csp.VerifySign(data, sign);
        }
    }
}