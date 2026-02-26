using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(int))]
class SpellSchoolIndexConverter : IValueConverter
{
    private static List<string> _Schools;

    static SpellSchoolIndexConverter()
    {
        try
        {
            _Schools = new List<string>();
            foreach (var r in Rule.Rules.FindAll(a => a.Type == "Magic" && a.Subtype == "School"))
            {
                _Schools.Add(r.Name.ToLower());
            }
            _Schools.Sort();
        }
        catch (Exception)
        {
            //should only happen at design time
        }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(int))
        {
            return DependencyProperty.UnsetValue;
        }

        if (value == null)
        {
            return 0;
        }


        return _Schools.IndexOf(((string)value).ToLower());

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        var index = (int)value;

        if (index < _Schools.Count)
        {
            return _Schools[(int)value];
        }

        return null;

    }

    public static IEnumerable<string> Schools
    {
        get
        {
            return _Schools;
        }
    }
}