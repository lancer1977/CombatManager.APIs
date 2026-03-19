using CombatManager.Personalization;
using System;
using System.Windows.Media;

namespace CombatManager
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
                    var res = App.Current.Resources[change];

                    var c = scheme.GetColorUInt32(i, j).ToColor();
                    App.Current.Resources[change] = c;
                    App.Current.Resources[changeBrush] = new SolidColorBrush(c);

                }
            }

            var fore = (Color)( darkScheme ? App.Current.Resources["ThemeTextForegroundDark"] 
                : App.Current.Resources["ThemeTextForegroundLight"]);
            var back = (Color)(darkScheme ? App.Current.Resources["ThemeTextBackgroundDark"]
                : App.Current.Resources["ThemeTextBackgroundLight"]);

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

            App.Current.Resources["ThemeTextForeground"] = fore;
            App.Current.Resources["ThemeTextForegroundBrush"] = new SolidColorBrush(fore);
            App.Current.Resources["ThemeTextBackground"] = back;
            App.Current.Resources["ThemeTextBackgroundBrush"] = new SolidColorBrush(back);

            var healthBackground = (Color)(App.Current.Resources
                [darkScheme ? "HealthDarkBackground" : "HealthLightBackground"]);
            App.Current.Resources["HealthBackground"] = healthBackground;
            App.Current.Resources["HealthBackgroundBrush"] = new SolidColorBrush(healthBackground);

            NewSchemeSet?.Invoke(scheme, darkScheme);

        }
    }
}
