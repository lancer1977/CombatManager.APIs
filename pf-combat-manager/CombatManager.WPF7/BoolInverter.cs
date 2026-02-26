using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(bool), typeof(bool))]
class BoolInverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            return null;
        }

        return !((bool)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            return null;
        }

        return !((bool)value);
    }


}