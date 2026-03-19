namespace CombatManager.Utilities;

public abstract class SimpleNotifyInternalClass<T> : SimpleNotifyClass
{
    private T _parent;
    public SimpleNotifyInternalClass(T parent)
    {
        this._parent = parent;
    }

    protected T Parent => _parent;
}