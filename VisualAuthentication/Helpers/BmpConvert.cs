using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VisualAuthentication.Helpers
{
    public static class BmpConvert
    {
        public static string ToBase64(Image bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Bmp);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}