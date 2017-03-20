using System.Collections.Generic;
using System.Drawing;

namespace VisualAuthentication.Factories
{
    public static class BrushFactory
    {
        private static readonly Dictionary<Color, Pen> Pens = new Dictionary<Color, Pen>();

        public static Brush GetBrush(Color color)
        {
            if (Pens.TryGetValue(color, out var pen))
                return pen.Brush;

            var cPen = Pens[color] = new Pen(color);
            return cPen.Brush;
        }
    }
}