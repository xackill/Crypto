using System.Drawing;

namespace VisualAuthentication
{
    public static class VASecret
    {
        public const int KeySize = 10;
        public const int KeysCount = 10;

        public const int FieldCols = 10;
        public const int FieldRows = 10;

        public const int ElementCols = 3;
        public const int ElementRows = 3;

        public const int IterationsCount = 10;

        public static Color[] AvailableColors =
        {
            Color.White,
            Color.DeepSkyBlue,
            Color.Red,
            Color.Green,
            Color.Yellow
        };
    }
}