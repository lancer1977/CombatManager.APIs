using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CombatManager.WPF7;

[ValueConversion(typeof(String), typeof(Image))]
class StringImageConverter : IValueConverter
{
    public static Dictionary<string, ImageSource> loadedImages;

    static StringImageConverter()
    {
        loadedImages = new Dictionary<string, ImageSource>();
    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var name = (string)value;

        ImageSource image = null;

        if (name != null)
        {

            if (loadedImages.ContainsKey(name))
            {
                image = loadedImages[name];
            }

            else
            {
                image = new BitmapImage(new Uri("pack://application:,,,/Images/" + name + "-16.png"));
                if (image != null)
                {
                    loadedImages[name] = image;
                }
            }

        }
        var imageControl = new Image
        {
            Source = image
        };
        return imageControl;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}