namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("melee", IsNullable=false)]
public class Melee 
{
    [XmlElement("weapon")]
    public Weapon[] Weapon { get; set; }
}