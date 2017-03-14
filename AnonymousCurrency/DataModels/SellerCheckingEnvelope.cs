namespace AnonymousCurrency.DataModels
{
    public class SellerCheckingEnvelope
    {
        public byte[] EncryptedContent { get; set; }
        public byte[] EncryptedContentSign { get; set; }

        public byte[] EncryptedSecrets { get; set; }

        public byte[] PublicPrivateKey { get; set; }
    }
}