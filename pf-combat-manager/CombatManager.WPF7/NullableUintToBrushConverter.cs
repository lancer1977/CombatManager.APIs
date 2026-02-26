using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CombatManager.WPF7;

[ValueConversion(typeof(uint?), typeof(Brush))]
class NullableUintToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(Brush))
        {
            return null;
        }

        if (value == null)
        {
            return null;
        }
        else
        {
            var ui = ((uint?)value).Value;
            var a = (byte)(ui >> 24);
            var r = (byte)(ui >> 16);
            var g = (byte)(ui >> 8);
            var b = (byte)ui;
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}