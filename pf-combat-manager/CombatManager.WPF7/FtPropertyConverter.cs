using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(int))]
class FtPropertyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var strVal = (string)value;


        var retVal = 0;

        if (strVal != null)
        {

            var regFt = new Regex("(?<num>[0-9]+) +ft\\.");
            var m = regFt.Match(strVal);


            if (m.Success)
            {
                retVal = int.Parse(m.Groups["num"].Value);
            }
        }

        return retVal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int)
        {
            return (int)value + " ft.";
        }
        else
        {
            return ((string)value) + " ft.";
        }
    }
}