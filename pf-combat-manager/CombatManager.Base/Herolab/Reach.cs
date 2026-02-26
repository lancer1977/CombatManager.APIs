namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("reach", IsNullable=false)]
public class Reach 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}