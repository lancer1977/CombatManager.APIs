//#if MONO
//namespace CombatManager;

//public class MonoPreferences : IPreferences
//{
//    public   void SaveBoolValue(string name, bool value)
//    {
//        Preferences.Set(name, value);
//    }


//    public   bool LoadBoolValue(string name, bool def)
//    {
//        return Preferences.Get(name, def);

//    }
//    public   void SavestringValue(string name, string value)
//    {
//        Preferences.Set(name, value);
//    }


//    public   string LoadstringValue(string name, string def)
//    {
//        return Preferences.Get(name, (string)def);
//    }
//    public   void SaveIntValue(string name, int value)
//    {
//        Preferences.Set(name, value);
//    }


//    public   int LoadIntValue(string name, int def)
//    {
//        return Preferences.Get(name, def);
//    }
//}
//#endif