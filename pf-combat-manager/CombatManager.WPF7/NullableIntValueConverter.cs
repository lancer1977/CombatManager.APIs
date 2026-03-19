using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class NullableIntValueConverter : IValueConverter
{
    int minVal;
    int maxVal;

    public NullableIntValueConverter(int minVal, int maxVal)
    {
        this.minVal = minVal;
        this.maxVal = maxVal;
    }


    public NullableIntValueConverter() : this(int.MinValue, int.MaxValue)
    {

    }

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
            if (outVal < minVal || outVal > maxVal)
            {
                return DependencyProperty.UnsetValue;
            }

            return outVal;
        }

        return DependencyProperty.UnsetValue;

    }
}