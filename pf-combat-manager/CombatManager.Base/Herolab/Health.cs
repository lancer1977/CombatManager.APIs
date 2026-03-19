namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("health", IsNullable=false)]
public class Health 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
    [XmlAttribute]
    public string Currenthp { get; set; }
    [XmlAttribute]
    public string Damage { get; set; }
    [XmlAttribute]
    public string Hitdice { get; set; }
    [XmlAttribute]
    public string Hitpoints { get; set; }
    [XmlAttribute]
    public string Nonlethal { get; set; }
}