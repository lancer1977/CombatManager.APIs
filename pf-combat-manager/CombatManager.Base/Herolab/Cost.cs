namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("cost", IsNullable=false)]
public class Cost 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}