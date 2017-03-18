using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VisualAuthentication.Helpers
{
    public static class BmpHelper
    {
        public static string ConvertToBase64(Bitmap bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Bmp);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}