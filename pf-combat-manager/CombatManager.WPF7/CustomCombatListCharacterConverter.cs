using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatManager.WPF7;

class CustomCombatListCharacterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Character c = null;
        CustomCombatList list = null;

        foreach (var ob in values)
        {
            if (ob != null)
            {
                if (ob.GetType() == typeof(Character))
                {
                    c = (Character)ob;
                }
                else if (ob.GetType() == typeof(CustomCombatList))
                {
                    list = (CustomCombatList)ob;
                }
            }
        }

        if (c != null && list != null)
        {
            if (c.IsMonster ? !list.ShowMonsters : !list.ShowPlayers)
            {
                return "";
            }

            if (c.IsMonster ? list.HideMonsterNames : list.HidePlayerNames)
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