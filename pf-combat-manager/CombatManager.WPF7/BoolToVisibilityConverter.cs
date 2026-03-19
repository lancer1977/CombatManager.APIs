using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(bool), typeof(Visibility))]
class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
			
        if (targetType != typeof(Visibility))
        {
            return null;
        }
			
        var visible = (bool)value;
        if (parameter != null && parameter.GetType() == typeof(bool))
        {
            if ((bool)parameter)
            {
                visible = !visible;
            }
        }
        else if (parameter != null && parameter.GetType() == typeof(string))
        {
            if (String.Compare((string)parameter, "true", true) == 0)
            {
                visible = !visible;
            }
        }
			
        return ((bool)visible) ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            return null;

        }
        var vis = (Visibility)value;
			
        var invert = false;
        if (parameter != null && parameter.GetType() == typeof(bool))
        {
            invert = (bool)parameter;
        }

        return invert?(vis != Visibility.Visible):(vis==Visibility.Visible);
    }
}