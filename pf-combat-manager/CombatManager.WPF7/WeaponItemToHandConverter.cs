using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(WeaponItem), typeof(string))]
class WeaponItemToHandConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        var item = (WeaponItem)value;

        if (item == null)
        {
            return DependencyProperty.UnsetValue;
        }
			
        if (item.MainHand)
        {

            return "Main";
        }
        else
        {

            return "Off";
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
           
    }
}