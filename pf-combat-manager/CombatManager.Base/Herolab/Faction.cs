namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("faction", IsNullable=false)]
public class Faction 
{
    public Faction() {Retired = FactionRetired.No;}
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Tpa { get; set; }
    [XmlAttribute]
    public string Cpa { get; set; }
    [XmlAttribute, DefaultValue(FactionRetired.No)]
    public FactionRetired Retired { get; set; }
}