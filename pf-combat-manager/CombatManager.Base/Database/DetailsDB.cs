using CombatManager.Interfaces;
using PolyhydraGames.Core.Interfaces;
using SQLite;

namespace CombatManager.Database;

public   class DetailsDb
{
    SQLiteConnection detailsDB;
    public static ISQLService<MagicItem> MagicItems { get; }
    public static ISQLService<Monster> Monsters { get; }

    public   Monster GetMonsterDetails(string id)
    {
        throw new NotImplementedException("GetMonster Details");
        return null;
    }
     
     
}
