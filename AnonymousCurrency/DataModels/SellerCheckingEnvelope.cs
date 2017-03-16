namespace AnonymousCurrency.DataModels
{
    public class SellerCheckingEnvelope : IEncryptedSecretsSignsField
    {
        public byte[] EncryptedContent { get; set; }
        public byte[] EncryptedContentSign { get; set; }

        public byte[] EncryptedSecretsSigns { get; set; }

        public byte[] PublicPrivateKey { get; set; }
    }
}