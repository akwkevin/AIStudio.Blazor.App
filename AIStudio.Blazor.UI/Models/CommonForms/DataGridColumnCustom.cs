namespace AIStudio.Blazor.UI.Models.CommonForms
{
    public class DataGridColumnCustom
    {
        public int DisplayIndex
        {
            get; set;
        } = int.MaxValue;

        public string PropertyName
        {
            get; set;
        }

        public string CellStyle
        {
            get; set;
        }

        public bool CanUserSort
        {
            get; set;
        } = true;

        public bool CanUserResize
        {
            get; set;
        } = true;

        public bool CanUserReorder
        {
            get; set;
        } = true;

        public string HeaderStyle
        {
            get; set;
        }

        public string Width
        {
            get; set;
        }

        public double MaxWidth
        {
            get; set;
        }

        public double MinWidth
        {
            get; set;
        }


        public string SortMemberPath
        {
            get; set;
        }

        public int Visibility
        {
            get; set;
        }

        public object Header
        {
            get; set;
        }

        public string StringFormat
        {
            get; set;
        }

        public string Converter
        {
            get; set;
        }

        public object ConverterParameter
        {
            get; set;
        }

        public string ForegroundExpression
        {
            get; set;
        }

        public string BackgroundExpression
        {
            get; set;
        }

        public int HorizontalAlignment
        {
            get; set;
        }
    }
}
