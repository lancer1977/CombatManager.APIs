using System;
using System.IO;

namespace CombatManager.WPF7;

public static class CMFileUtilities
{
    public static string AppDataDir
    {
        get
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "Combat Manager");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }

    public static string AppDataSubDir(String name)
    {
        var path = Path.Combine(AppDataDir, name);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

}