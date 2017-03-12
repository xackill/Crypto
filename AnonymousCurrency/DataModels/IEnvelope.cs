namespace AnonymousCurrency.DataModels
{
    public interface IEnvelope
    {
        byte[] EncryptedSecrets { get; }
    }
}