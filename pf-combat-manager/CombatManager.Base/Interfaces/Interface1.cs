using CombatManager.Database.Models;

namespace CombatManager.Interfaces
{
 

    public interface ISQLService<T> : IDisposable
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void AddItem(T item);
        void UpdateItem(T item);
        void DeleteItem(T item);
    }

  

    public interface IRulesService
    {
        string GetDetails(int id);

    }

 
}
