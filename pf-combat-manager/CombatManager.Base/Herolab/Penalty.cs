namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("penalty", IsNullable=false)]
public class Penalty 
{
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}