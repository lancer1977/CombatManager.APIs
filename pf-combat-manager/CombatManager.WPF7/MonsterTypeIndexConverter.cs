using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(int))]
class MonsterTypeIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(int))
        {
            return DependencyProperty.UnsetValue;
        }

        return (int)Monster.ParseCreatureType((string)value);

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        return Monster.CreatureTypeText((CreatureType)value);

    }
}