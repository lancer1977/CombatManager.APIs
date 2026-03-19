namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("speed", IsNullable=false)]
public class Speed 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}