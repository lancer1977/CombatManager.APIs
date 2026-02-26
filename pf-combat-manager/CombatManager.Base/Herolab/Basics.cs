namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("basics", IsNullable=false)]
public class Basics 
{
    [XmlElement("npcinfo")]
    public NpCinfo[] Npcinfo { get; set; }
}