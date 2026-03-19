using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(string))]
class ParenthesesPropertyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var strVal = (string)value;


        if (strVal != null)
        {

            strVal = strVal.Trim(new char[] {'(', ')', ' '});
        }

        return strVal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var strVal = (string)value;

        if (strVal != null && strVal.Trim().Length > 0)
        {
            strVal = "(" + strVal + ")";
        }
        else
        {
            strVal = null;
        }

        return strVal;
    }
}