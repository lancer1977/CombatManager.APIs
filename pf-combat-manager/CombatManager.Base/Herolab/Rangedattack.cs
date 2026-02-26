namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("rangedattack", IsNullable=false)]
public class Rangedattack 
{
    [XmlAttribute]
    public string Attack { get; set; }
    [XmlAttribute]
    public string Flurryattack { get; set; }
    [XmlAttribute]
    public string Rangeinctext { get; set; }
    [XmlAttribute]
    public string Rangeincvalue { get; set; }
}