namespace CombatManager.Utilities;

public class NotifyValue<T> : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    T _value;

    public NotifyValue(T val)
    {
        _value = val;
    }

    public NotifyValue()
    {
    }

    public T Value
    {

        get => _value;
        set
        {
            _value = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                
        }
    }

}