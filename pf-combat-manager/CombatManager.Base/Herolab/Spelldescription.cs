namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spelldescript", IsNullable=false)]
public class Spelldescription 
{
    [XmlText]
    public string Value { get; set; }
}