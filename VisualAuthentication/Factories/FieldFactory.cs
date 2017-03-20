using System;
using System.Linq;
using Core.Extensions;
using VisualAuthentication.DataBaseModels;
using VisualAuthentication.DataModels;
using VisualAuthentication.Extensions;

namespace VisualAuthentication.Factories
{
    public static class FieldFactory
    {
        public static Field CreateField(Session session)
        {
            var keys = session.Keys();
            var generator = new Func<Element>(() => keys.GetRandomElement().Elements.GetRandomElement());
            var elements = TableFactory.CreateTable(VASecret.FieldRows, VASecret.FieldCols, generator);

            var rowAnswers = GenerateRandomSequence(len: VASecret.FieldRows);
            var colAnswers = GenerateRandomSequence(len: VASecret.FieldRows);
            return new Field
            {
                Elements = elements,
                RowAnswers = rowAnswers,
                ColumnAnswers = colAnswers
            };
        }

        private static int[] GenerateRandomSequence(int len)
            => Enumerable.Range(1, len).Shuffle().ToArray();
    }
}