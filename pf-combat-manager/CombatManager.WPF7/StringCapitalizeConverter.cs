using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(string), typeof(string))]
class StringCapitalizeConverter : IValueConverter
{

    public static SortedSet<String> ignoreWords;

    static StringCapitalizeConverter()
    {
        ignoreWords = new SortedSet<string>(new InsensitiveComparer());
            
        string[] ignore = { "the", "of", "from", "to", "and" };

        foreach (var str in ignore)
        {
            ignoreWords.Add(str);
        }
    }



    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }
						

        return Capitalize ((string)value);
			


    }

    public static string Capitalize(string text)
    {
        if (text != null)
        {

            var regWord = new Regex("\\w+('s)?");

            text = regWord.Replace(text, delegate(Match m)
            {
                var x = m.Value;

                if (!ignoreWords.Contains(x))
                {

                    x = x.Substring(0, 1).ToUpper() + x.Substring(1);
                }

                return x;
            });
        }

        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        return ((string)value).ToLower();

    }
}