using System;
using System.Linq;
using Core.Extensions;

namespace VisualAuthentication.Factories
{
    public static class TableFactory
    {
        public static T[][] CreateTable<T>(int rowsCount, int columnsCount, Func<T> generator)
            => Enumerable.Range(0, rowsCount).SelectToArray(_ => CreateRow(columnsCount, generator));

        public static T[] CreateRow<T>(int columnsCount, Func<T> generator)
            => Enumerable.Range(0, columnsCount).SelectToArray(_ => generator());
    }
}