using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using CombatManager.Utilities;

namespace CombatManager.WPF7;

class SensesConverter : IMultiValueConverter
{
    private static int _Perception;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        if (values.Length != 2 || !(values[0] is string) || !(values[1] is int))
        {
            return DependencyProperty.UnsetValue;
        }

        var senses = (string)values[0];
        _Perception = (int)values[1];

        var regEx = new Regex("(?<senses>.*?)(; )?Perception (\\+|\\-)[0-9]+");

        var m = regEx.Match(senses);

        if (m.Success)
        {
            return m.Groups["senses"].Value;
        }
        else
        {
            return senses;

        }
    }

    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {

        var objects = new object[targetTypes.Length];

        var senses = ((string)value).Trim();

        var sensesText = "";

        if (senses.Length > 0)
        {
            sensesText += senses + "; ";
        }

        objects[0] = sensesText + "Perception " + CmStringUtilities.PlusFormatNumber(_Perception);
        objects[1] = _Perception;
            

        return objects;
    }


}