using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int), typeof(Brush))]
class HPToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var brushes = (BrushCollection)parameter;

        if (brushes == null)
        {
            brushes = new BrushCollection();
            brushes.Add((Brush)Application.Current.Resources["ThemeTextForegroundBrush"]);
            brushes.Add(new SolidColorBrush(Colors.Blue));
            brushes.Add(new SolidColorBrush(Colors.Red));
        }

        if (targetType != typeof(Brush) )
        {
            return null;
        }
			
        if ((int)value == 0)
        {
            return brushes[1];
        }

        return (int)value < 0 ? brushes[2] : brushes[0];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}