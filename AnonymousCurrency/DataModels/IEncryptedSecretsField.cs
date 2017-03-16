namespace AnonymousCurrency.DataModels
{
    public interface IEncryptedSecretsField
    {
        byte[] EncryptedSecrets { get; }
    }
}