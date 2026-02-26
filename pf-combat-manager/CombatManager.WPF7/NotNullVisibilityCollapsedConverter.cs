using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(object), typeof(Visibility))]
class NotNullVisibilityCollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var nullVis = Visibility.Collapsed;
        var notNullVis = Visibility.Visible;

        if (parameter != null)
        {
            nullVis = Visibility.Visible;
            notNullVis = Visibility.Collapsed;
        }


        if (value != null &&  value.GetType() == typeof(String))
        {
            var text = (String)value;

            return (text.Length == 0) ? nullVis : notNullVis;
        }

        return (value != null) ? notNullVis : nullVis;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}