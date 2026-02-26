namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("pathfindersociety", IsNullable=false)]
public class Pathfindersociety 
{
    [XmlAttribute]
    public string Playernum { get; set; }
    [XmlAttribute]
    public string Characternum { get; set; }
}