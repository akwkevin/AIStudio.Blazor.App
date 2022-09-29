
using AIStudio.Client.Business;
using AIStudio.Util.Common;


namespace AIStudio.Blazor.UI.Converter
{
    public class ObjectToStringConverter: IValueConverter
    {
        protected IUserData _userData { get; }
        public ObjectToStringConverter(IUserData userData)
        {
            _userData = userData;
        }

        public object Convert(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            List<SelectOption> itemSource = null;
            if (param != null && _userData.ItemSource.ContainsKey(param?.ToString()))
            {
                itemSource = _userData.ItemSource[param?.ToString()];
            }

            if (value is IEnumerable<object> list)
            {
                if (itemSource != null)
                {
                    List<string> displays = new List<string>();
                    foreach (var item in list)
                    {
                        displays.Add(itemSource.FirstOrDefault(p => item?.ToString() == p.Value)?.Text);
                    }
                    return string.Join(",", displays.Select(p => p?.ToString()));
                }
                return string.Join(",", list.Select(p => p?.ToString()));
            }
            else
            {
                if (itemSource != null)
                {
                    return itemSource.FirstOrDefault(p => value?.ToString() == p.Value)?.Text;
                }
            }

            return value?.ToString();
        }

        public object ConvertBack(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
