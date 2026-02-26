using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class TurnsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        if (value == null)
        {
            return "-";
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(int?))
        {
            return DependencyProperty.UnsetValue;
        }

        if (((string)value) == "-")
        {
            return null;
        }

        int outVal;

        if (int.TryParse((string)value, out outVal))
        {
            if (outVal < 0)
            {
                return DependencyProperty.UnsetValue;
            }

            return outVal;
        }

        return DependencyProperty.UnsetValue;

    }
}