using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

public class CombatListWindowCharacterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Character c = null;
        CombatListWindow list = null;

        foreach (var ob in values)
        {
            if (ob != null)
            {
                if (ob.GetType() == typeof(Character))
                {
                    c = (Character)ob;
                }
                else if (ob.GetType() == typeof(CombatListWindow))
                {
                    list = (CombatListWindow)ob;
                }
            }
        }

        if (c != null && list != null)
        {


            if (c.IsMonster ? !UserSettings.Settings.InitiativeShowMonsters : !UserSettings.Settings.InitiativeShowPlayers)
            {
                return "";
            }

            if (c.IsMonster ? UserSettings.Settings.InitiativeHideMonsterNames : UserSettings.Settings.InitiativeHidePlayerNames)
            {
                return "??????";
            }
            else
            {
                return c.Name;
            }
        }

        return null;
    }


    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return new object[targetTypes.Length];
    }



}