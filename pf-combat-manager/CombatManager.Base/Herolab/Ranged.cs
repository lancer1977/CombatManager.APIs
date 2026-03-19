namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("ranged", IsNullable=false)]
public class Ranged 
{
    [XmlElement("weapon")]
    public Weapon[] Weapon { get; set; }
}