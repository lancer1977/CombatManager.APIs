using System;
using System.Windows;
using System.Windows.Media;

namespace CombatManager.WPF7
{
    static class ColorManager
    {

        public static event Action<ColorScheme, bool> NewSchemeSet;

        public static void PrepareCurrentScheme()
        {
            SetNewScheme(UserSettings.Settings.ColorScheme, UserSettings.Settings.DarkScheme);
        }


        public static void SetNewScheme(int id, bool darkScheme)
        {
            var scheme = ColorSchemeManager.Manager.SchemeById(id);

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    var change = ColorScheme.GetColorName(i, j);
                    var changeBrush = change + "Brush";
                    var res = Application.Current.Resources[change];

                    var c = scheme.GetColorUInt32(i, j).ToColor();
                    Application.Current.Resources[change] = c;
                    Application.Current.Resources[changeBrush] = new SolidColorBrush(c);

                }
            }

            var fore = (Color)( darkScheme ? Application.Current.Resources["ThemeTextForegroundDark"] 
                : Application.Current.Resources["ThemeTextForegroundLight"]);
            var back = (Color)(darkScheme ? Application.Current.Resources["ThemeTextBackgroundDark"]
                : Application.Current.Resources["ThemeTextBackgroundLight"]);

            var newFore = scheme.GetTextColor(true, darkScheme);
            if (newFore != 0)
            {
                fore = newFore.ToColor();
            }

            var newBack = scheme.GetTextColor(false, darkScheme);
            if (newBack != 0)
            {
                back = newBack.ToColor();
            }

            Application.Current.Resources["ThemeTextForeground"] = fore;
            Application.Current.Resources["ThemeTextForegroundBrush"] = new SolidColorBrush(fore);
            Application.Current.Resources["ThemeTextBackground"] = back;
            Application.Current.Resources["ThemeTextBackgroundBrush"] = new SolidColorBrush(back);

            var healthBackground = (Color)(Application.Current.Resources
                [darkScheme ? "HealthDarkBackground" : "HealthLightBackground"]);
            Application.Current.Resources["HealthBackground"] = healthBackground;
            Application.Current.Resources["HealthBackgroundBrush"] = new SolidColorBrush(healthBackground);

            NewSchemeSet?.Invoke(scheme, darkScheme);

        }
    }
}
