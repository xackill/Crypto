using KeyDeposit.DataModels;

namespace KeyDeposit.Workers
{
    public static class KeySourceFactory
    {
        public static KeySource Generate()
            => new KeySource {G = IntFactory.GenerateRandom(), P = IntFactory.GeneratePrime()};
    }
}