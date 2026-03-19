using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CombatManager.WPF7;

[ValueConversion(typeof(ViewModels.Attack), typeof(ImageSource))]
class AttackToTypeImageConverter : IValueConverter
{
    static BitmapImage natural;
    static BitmapImage melee;
    static BitmapImage ranged;

    static AttackToTypeImageConverter()
    {
        try
        {
            natural = new BitmapImage(new Uri("pack://application:,,,/Images/claw-16.png"));
            melee = new BitmapImage(new Uri("pack://application:,,,/Images/sword-single-16.png"));
            ranged = new BitmapImage(new Uri("pack://application:,,,/Images/bow-16.png"));
        }
        catch (Exception)
        {
            //this is to prevent problems in the editor
        }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(ImageSource))
        {
            return null;
        }

        var attack = (ViewModels.Attack)value;

        if (attack.Weapon != null)
        {
            if (attack.Weapon.Class == "Natural")
            {
                return natural; 
            }
            else if (attack.Weapon.Ranged)
            {
                return ranged;
            }
            else
            {
                return melee;
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}