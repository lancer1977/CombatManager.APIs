using CombatManager.Interfaces;
using SQLite;

namespace CombatManager.Database;

public class DatabaseLoader<T> : ISQLService<T>
{
    private readonly string _filename;
    private readonly SQLiteConnection _db;

    public DatabaseLoader(string filename)
    {
        _filename = filename;
        _db = new SQLiteConnection(filename);
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public T GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public void AddItem(T item)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(T item)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(T item)
    {
        throw new NotImplementedException();
    }
}