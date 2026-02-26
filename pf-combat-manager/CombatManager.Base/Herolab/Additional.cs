namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("additional", IsNullable=false)]
public class Additional 
{
    [XmlElement("npcinfo")]
    public NpCinfo[] Npcinfo { get; set; }
}