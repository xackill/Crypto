using System.Linq;

namespace VisualAuthentication.DataModels
{
    public class Key
    {
        public Element[] Elements { get; set; }

        public bool Contains(Element el)
            => Elements.Contains(el);
    }
}