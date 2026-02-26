using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(Visibility))]
class StringBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (targetType != typeof(Visibility))
        {
            return null;
        }

        var visible = ((string)value) == "1";
        if (parameter != null && parameter.GetType() == typeof(bool))
        {
            if ((bool)parameter)
            {
                visible = !visible;
            }
        }
        else if (parameter != null && parameter.GetType() == typeof(string))
        {
            if (string.Compare((string)parameter, "true", true) == 0)
            {
                visible = !visible;
            }
        }

        return ((bool)visible) ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return null;

        }
        var vis = (Visibility)value;

        var invert = false;
        if (parameter != null && parameter.GetType() == typeof(bool))
        {
            invert = (bool)parameter;
        }

        var val =  invert ? (vis != Visibility.Visible) : (vis == Visibility.Visible);
        return val ? "1" : "0";
    }
}