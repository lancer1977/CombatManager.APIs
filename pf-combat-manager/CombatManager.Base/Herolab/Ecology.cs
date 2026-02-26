namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("ecology", IsNullable=false)]
public class Ecology 
{
    [XmlElement("npcinfo")]
    public NpCinfo[] Npcinfo { get; set; }
}