namespace CombatManager
{
    public interface IPreferences
    {
        bool LoadBoolValue(string name, bool defaultValue);

        double LoadDoubleValue(string name, double defaultValue);
        int LoadIntValue(string name, int defaultValue);
        string LoadStringValue(string name, string defaultValue);
        void SaveBoolValue(string name, bool value);
        void SaveDoubleValue(string name, double value);

        void SaveIntValue(string name, int value);
        void SaveStringValue(string name, string value);

    }
}