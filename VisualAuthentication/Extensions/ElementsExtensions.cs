using System.Linq;
using VisualAuthentication.DataModels;
using VisualAuthentication.Factories;
using VisualAuthentication.Helpers;

namespace VisualAuthentication.Extensions
{
    public static class ElementsExtensions
    {
        public static string[] ToBase64Pics(this Element[] elements)
            => elements.Select(PictureDrawer.Draw).Select(BmpHelper.ConvertToBase64).ToArray();
    }
}