namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("basespeed", IsNullable=false)]
public class Basespeed 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}