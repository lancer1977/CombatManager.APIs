using System;

namespace CombatManager.WPF7;

public class DocumentLinkEventArgs : EventArgs
{
    string _Name;
    string _Type;

    public DocumentLinkEventArgs(string name, string type)
    {
        _Name = name;
        _Type = type;
    }



    public string Type
    {
        get
        {
            return _Type;
        }
        set
        {
            _Type = value;
        }
    }

    public string Name
    {
        get
        {
            return _Name;
        }
        set
        {
            _Name = value;
        }
    }
}