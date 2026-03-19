namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("itemslot", IsNullable=false)]
public class Itemslot 
{
    [XmlText]
    public string Value { get; set; }
}