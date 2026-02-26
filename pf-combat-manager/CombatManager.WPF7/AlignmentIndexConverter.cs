using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(int))]
class AlignmentIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(int))
        {
            return DependencyProperty.UnsetValue;
        }

        var al = Monster.ParseAlignment((string)value);

        var val = 0;

        val += (int)al.Order;
        val += 3 * (int)al.Moral;

        return val;
            
			
			
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        var type = new Monster.AlignmentType();
        type.Moral = (Monster.MoralAxis)((int)value / 3);
        type.Order = (Monster.OrderAxis)((int)value % 3);

        return Monster.AlignmentText(type) ;
			
    }
}