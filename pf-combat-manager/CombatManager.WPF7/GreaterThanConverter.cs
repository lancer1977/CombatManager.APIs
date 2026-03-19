using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int), typeof(bool))]
class GreaterThanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
          
        if (parameter == null)
        {
            return ((int)value) > 0;
        }
        else
        {
            return ((int)value) > (int)parameter;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}