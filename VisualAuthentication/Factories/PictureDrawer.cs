using System.Drawing;
using System.Drawing.Drawing2D;
using VisualAuthentication.DataModels;

namespace VisualAuthentication.Factories
{
    public static class PictureDrawer
    {
        private const int Cell = 20;
        private const int Width = Cell * VASecret.ElementCols + 1;
        private const int Height = Cell * VASecret.ElementRows + 1;

        private static readonly Pen BorderPen = new Pen(Color.Black);

        public static Bitmap Draw(Element el)
        {
            var bmp = new Bitmap(Width, Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                for (var i=0; i<el.Colors.Length; ++i)
                for (var j=0; j<el.Colors[i].Length; ++j)
                {
                    var colorBrush = BrushFactory.GetBrush(el.Colors[i][j]);
                    gr.DrawRectangle(BorderPen, i * Cell, j * Cell, Cell, Cell);
                    gr.FillRectangle(colorBrush, i * Cell + 1, j * Cell + 1, Cell - 1, Cell - 1);
                }
            }
            return bmp;
        }
    }
}