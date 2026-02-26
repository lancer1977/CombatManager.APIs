namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("tactics", IsNullable=false)]
public class Tactics 
{
    [XmlElement("npcinfo")]
    public NpCinfo[] Npcinfo { get; set; }
}