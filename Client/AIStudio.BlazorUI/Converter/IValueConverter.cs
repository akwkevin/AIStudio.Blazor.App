using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Converter
{
    //
    // 摘要:
    //     Provides a way to apply custom logic to a binding.
    public interface IValueConverter
    {
        //
        // 摘要:
        //     Converts a value.
        //
        // 参数:
        //   value:
        //     The value produced by the binding source.
        //
        //   targetType:
        //     The type of the binding target property.
        //
        //   parameter:
        //     The converter parameter to use.
        //
        //   culture:
        //     The culture to use in the converter.
        //
        // 返回结果:
        //     A converted value. If the method returns null, the valid null value is used.
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        //
        // 摘要:
        //     Converts a value.
        //
        // 参数:
        //   value:
        //     The value that is produced by the binding target.
        //
        //   targetType:
        //     The type to convert to.
        //
        //   parameter:
        //     The converter parameter to use.
        //
        //   culture:
        //     The culture to use in the converter.
        //
        // 返回结果:
        //     A converted value. If the method returns null, the valid null value is used.
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
