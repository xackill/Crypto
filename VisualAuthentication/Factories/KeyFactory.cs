using System.Collections.Generic;
using System.Drawing;
using Core.Extensions;
using VisualAuthentication.DataModels;

namespace VisualAuthentication.Factories
{
    public static class KeyFactory
    {
        public static Key CreateKey()
        {
            var elements = new List<Element>(VASecret.KeySize);
            while (elements.Count != VASecret.KeySize)
            {
                var element = CreateElement();
                if (!elements.Contains(element))
                    elements.Add(element);
            }

            return new Key {Elements = elements.ToArray()};
        }

        private static Element CreateElement()
            => new Element {Colors = CreateElementColors()};

        private static Color[][] CreateElementColors()
            => TableFactory.CreateTable(VASecret.ElementRows, VASecret.ElementCols, CreateColor);

        private static Color CreateColor()
            => VASecret.AvailableColors.GetRandomElement();
    }
}