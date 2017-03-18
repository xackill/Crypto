using System.Drawing;
using System.Linq;

namespace VisualAuthentication.DataModels
{
    public class Element
    {
        public Color[][] Colors { get; set; }

        public override bool Equals(object obj)
        {
            var element = obj as Element;
            if (element == null)
                return false;

            var a = Colors;
            var b = element.Colors;

            if (a.Length != b.Length)
                return false;

            return Enumerable
                .Range(0, a.Length)
                .All(i => a[i].SequenceEqual(b[i]));
        }

        public override int GetHashCode()
        {
            return Colors?.GetHashCode() ?? 0;
        }
    }
}