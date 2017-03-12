namespace AnonymousCurrency.DataModels
{
    public interface IExtremelySerializable
    {
        byte[] ExtremelySerialize();
        void InitByDeserializing(byte[] bytes);
    }
}
