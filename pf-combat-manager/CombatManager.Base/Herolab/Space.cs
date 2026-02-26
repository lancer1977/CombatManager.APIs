namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("space", IsNullable=false)]
public class Space 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}