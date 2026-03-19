namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("attrbonus", IsNullable=false)]
public class Attrbonus 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Base { get; set; }
    [XmlAttribute]
    public string Modified { get; set; }
}