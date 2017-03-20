using System.Drawing;
using VisualAuthentication.DataModels;

namespace VisualAuthentication.Factories
{
    public static class PictureDrawer
    {
        private const int Cell = 20;
        private const int Width = Cell * VASecret.ElementCols + 1;
        private const int Height = Cell * VASecret.ElementRows + 1;

        private static readonly Font DigitFont = new Font("Arial", 30);
        private static readonly Rectangle Rectangle = new Rectangle(0, 0, Width, Height);
        private static readonly StringFormat StringFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public static Bitmap Draw(Element el)
        {
            var bmp = new Bitmap(Width, Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                for (var i=0; i<el.Colors.Length; ++i)
                for (var j=0; j<el.Colors[i].Length; ++j)
                {
                    var colorBrush = BrushFactory.GetBrush(el.Colors[i][j]);
                    gr.DrawRectangle(Pens.Black, i * Cell, j * Cell, Cell, Cell);
                    gr.FillRectangle(colorBrush, i * Cell + 1, j * Cell + 1, Cell - 1, Cell - 1);
                }
            }
            return bmp;
        }

        public static Bitmap Draw(int value)
        {
            var bmp = new Bitmap(Width, Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                var text = value.ToString();
                gr.FillRectangle(Brushes.White, Rectangle);
                gr.DrawString(text, DigitFont, Brushes.Black, Rectangle, StringFormat);
            }
            return bmp;
        }

        public static Bitmap DrawEmptyImg()
        {
            var bmp = new Bitmap(Width, Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.FillRectangle(Brushes.White, Rectangle);
            }
            return bmp;
        }
    }
}