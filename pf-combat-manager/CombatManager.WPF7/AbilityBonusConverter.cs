using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CombatManager.Utilities;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class AbilityBonusConverter : IValueConverter
{



    public AbilityBonusConverter()
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
            return "0";
        }

        else
        {
            return CmStringUtilities.PlusFormatNumber(Monster.AbilityBonus((int)value));
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {

        return DependencyProperty.UnsetValue;

    }
}