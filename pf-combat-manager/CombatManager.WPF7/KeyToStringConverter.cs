using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace CombatManager.WPF7;

[ValueConversion(typeof(Key), typeof(String))]
class KeyToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(String))
        {
            return null;
        }

        var k = (Key)value;

        return k.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(Key))
        {
            return null;

        }

        var s = (String)value;

        Key returnKey;
        if (KeysList.TryGetValue(s, out returnKey))
        {
            return returnKey;
        }

        return null;
    }


    static SortedDictionary<String, Key> keysList;

    public static readonly HashSet<Key> IgnoreKeys = new HashSet<Key>()
    {
        Key.LeftCtrl,
        Key.RightCtrl,
        Key.LeftShift,
        Key.RightShift,
        Key.LeftAlt,
        Key.RightAlt,
        Key.LWin,
        Key.RWin
    };
 

    static public SortedDictionary<string, Key> KeysList
    {
        get
        {
            if (keysList == null)
            {

                keysList = new SortedDictionary<string, Key>();
                foreach (Key v in Enum.GetValues(typeof(Key)))
                {
                    if (!IgnoreKeys.Contains(v))
                    {
                        keysList[v.ToString()] = v;
                    }
                }
            }
            return keysList;
        }
    }
}