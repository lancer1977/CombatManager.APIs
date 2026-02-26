namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("attack", IsNullable=false)]
public class Attack 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
    [XmlAttribute]
    public string Attackbonus { get; set; }
    [XmlAttribute]
    public string Baseattack { get; set; }
    [XmlAttribute]
    public string Meleeattack { get; set; }
    [XmlAttribute]
    public string Rangedattack { get; set; }
}