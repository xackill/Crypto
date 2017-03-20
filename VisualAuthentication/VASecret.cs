using System.Drawing;

namespace VisualAuthentication
{
    public static class VASecret
    {
        public const int KeySize = 10;
        public const int KeysCount = 5;

        public const int FieldCols = 5;
        public const int FieldRows = 5;

        public const int ElementCols = 3;
        public const int ElementRows = 3;

        public const int IterationsCount = 4;

        public static Color[] AvailableColors =
        {
            Color.White,
            Color.DeepSkyBlue,
            Color.GreenYellow
        };
    }
}