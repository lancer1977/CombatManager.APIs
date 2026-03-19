namespace CombatManager.Utilities;

public static class FileUtilities
{
    public static bool IsZipFile(this string value)
    {
        return value.ToLower().Contains(".zip");
    }
}