namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("situationalmodifiers", IsNullable=false)]
public class Situationalmodifiers 
{
    [XmlElement("situationalmodifier")]
    public Situationalmodifier[] Situationalmodifier { get; set; }
    [XmlAttribute]
    public string Text { get; set; }
}