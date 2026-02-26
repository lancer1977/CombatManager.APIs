using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CombatManager.WPF7;

[ValueConversion(typeof(String), typeof(ImageSource))]
class StringImageSmallIconConverter : IValueConverter
{

    public static Dictionary<string, BitmapImage> loadedImages;

    static StringImageSmallIconConverter()
    {
        loadedImages = new Dictionary<string, BitmapImage>();
    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var name = (string)value;

        return FromName(name);
    }

    public static BitmapImage FromName(string name)
    {
        BitmapImage image = null;

        if (name != null)
        {

            if (loadedImages.ContainsKey(name))
            {
                image = loadedImages[name];
            }

            else
            {
                try
                {
                    image = new BitmapImage(new Uri("pack://application:,,,/Images/" + name + "-16.png"));
                    if (image != null)
                    {
                        loadedImages[name] = image;
                    }
                }
                catch (IOException)
                {
                }
            }

        }
        return image;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}