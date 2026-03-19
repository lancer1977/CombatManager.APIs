namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("arcanespellfailure", IsNullable=false)]
public class Arcanespellfailure 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}