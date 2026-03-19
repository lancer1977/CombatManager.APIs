using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(string))]
class CRValidatingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        return value;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
            
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        var cr = (string)value;

        var xpVal = Monster.TryGetCrValue(cr);

        if (xpVal == null)
        {
            return DependencyProperty.UnsetValue;
        }
            

        return cr.Trim();

    }
}