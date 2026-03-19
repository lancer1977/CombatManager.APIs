using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CombatManager.WPF7;

[ValueConversion(typeof(bool), typeof(Brush))]
class ChangedToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(Brush) )
        {
            return null;
        }

        var brushes = (BrushCollection)parameter;


        return (bool)value ? brushes[1] :
            brushes[0];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}