using System.Collections.Generic;
using System.Linq;
using VisualAuthentication.DataModels;
using VisualAuthentication.Factories;
using VisualAuthentication.Helpers;

namespace VisualAuthentication.Extensions
{
    public static class DrawsExtensions
    {
        public static List<string> ToBase64Images(this Element[] elements)
            => elements.Select(PictureDrawer.Draw).Select(BmpConvert.ToBase64).ToList();

        public static List<string> ToBase64Images(this int[] values)
            => values.Select(ToBase64Image).ToList();

        public static string ToBase64Image(this int value)
            => BmpConvert.ToBase64(PictureDrawer.Draw(value));
    }
}