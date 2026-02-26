namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("situationalmodifier", IsNullable=false)]
public class Situationalmodifier
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Source { get; set; }
}