using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(object), typeof(bool))]
class ItemExistsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var res = (value != null);
        return res;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}