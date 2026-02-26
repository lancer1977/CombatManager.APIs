using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(bool), typeof(Visibility))]
class BoolToVisibilityCollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(Visibility))
        {
            return null;
        }


        var falseVis = Visibility.Collapsed;
        var trueVis = Visibility.Visible;

        if (parameter != null)
        {

            falseVis = Visibility.Visible;
            trueVis = Visibility.Collapsed;
                    
                
        }

        return ((bool)value) ? trueVis : falseVis;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool) || targetType == typeof(bool?))
        {
            return null;

        }
        var vis = (Visibility)value;

        return (vis == Visibility.Visible);
    }
}