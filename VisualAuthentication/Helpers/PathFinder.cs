using VisualAuthentication.DataModels;

namespace VisualAuthentication.Helpers
{
    public static class PathFinder
    {
        public static int GetCorrectAnswer(Field field, Key key)
        {
            int x = 0, y = 0;
            while (x != VASecret.ElementCols && y != VASecret.ElementRows)
            {
                if (key.Contains(field.Elements[y][x]))
                    ++y;
                else
                    ++x;
            }

            if (y == VASecret.ElementCols)
                return field.ColumnAnswers[x];
            return field.RowAnswers[y];
        }
    }
}