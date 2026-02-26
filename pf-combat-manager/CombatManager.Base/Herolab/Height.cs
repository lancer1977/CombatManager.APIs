namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("charheight", IsNullable=false)]
public class Height 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}