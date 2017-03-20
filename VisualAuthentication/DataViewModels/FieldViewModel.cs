using System.Collections.Generic;

namespace VisualAuthentication.DataViewModels
{
    public class FieldViewModel
    {
        public List<List<string>> Pics { get; set; }
        public int[] RowAnswers { get; set; }
        public int[] ColumnAnswers { get; set; }
    }
}