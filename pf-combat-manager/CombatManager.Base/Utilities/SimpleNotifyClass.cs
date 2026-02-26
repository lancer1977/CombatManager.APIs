using System.Runtime.CompilerServices;

namespace CombatManager.Utilities;

public abstract class SimpleNotifyClass : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    public void Notify([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop)); 
    }
}