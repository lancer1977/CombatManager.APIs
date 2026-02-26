using System.IO;
using Microsoft.Win32;

namespace CombatManager.WPF7;

public class WindowsPreferences : IPreferences
{
    public bool LoadBoolValue(string name, bool defaultValue)
    {

        var value = defaultValue;

        try
        {
            var key = RegKey;
            if (key != null)
            {
                var ki = key.GetValueKind(name);

                if (ki == RegistryValueKind.DWord)
                {
                    var val = (int)key.GetValue(name);

                    value = (val != 0);
                }
            }
        }
        catch (IOException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

        return value;


    }

    public double LoadDoubleValue(string name, double defaultValue)
    {
        var value = defaultValue;
        try
        {
            var key = RegKey;
            if (key != null)
            {
                var ki = key.GetValueKind(name);

                if (ki == RegistryValueKind.String)
                {
                    var val = (string)key.GetValue(name);

                    if (val != null)
                    {
                        double.TryParse(val, out value);
                    }
                }
            }
        }
        catch (IOException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

        return value;

    }

    public int LoadIntValue(string name, int defaultValue)
    {
        var value = defaultValue;
        try
        {

            var key = RegKey;
            if (key != null)
            {
                var ki = key.GetValueKind(name);

                if (ki == RegistryValueKind.DWord)
                {
                    value = (int)key.GetValue(name);

                }
            }
        }
        catch (IOException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

        return value;

    }

    public string LoadStringValue(string name, string defaultValue)
    {
        var value = defaultValue;
        try
        {
            var key = RegKey;
            var ki = key?.GetValueKind(name);

            if (ki == RegistryValueKind.String)
            {
                value = (string)key.GetValue(name);

            }
        }
        catch (IOException ex)
        {
            System.Diagnostics.Debug.WriteLine(name + ": " + ex.ToString());
        }

        return value;

    }

    public RegistryKey RegKey => Registry.CurrentUser.OpenSubKey("Software\\CombatManager", true);

    public void SaveBoolValue(string name, bool value)
    {

        RegKey.SetValue(name, value ? 1 : 0, RegistryValueKind.DWord);
    }

    public void SaveDoubleValue(string name, double value)
    {
        RegKey.SetValue(name, value.ToString(), RegistryValueKind.String);
    }

    public void SaveIntValue(string name, int value)
    {

        RegKey.SetValue(name, value, RegistryValueKind.DWord);

    }

    public void SaveStringValue(string name, string value)
    {

        RegKey.SetValue(name, value, RegistryValueKind.String);

    }
}