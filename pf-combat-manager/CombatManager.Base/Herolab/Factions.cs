namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("factions", IsNullable=false)]
public class Factions 
{
    [XmlElement("faction")]
    public Faction[] Faction { get; set; }
}