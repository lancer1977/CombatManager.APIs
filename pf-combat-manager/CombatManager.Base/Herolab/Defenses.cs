namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("defenses", IsNullable=false)]
public class Defenses 
{
    [XmlElement("armor")]
    public Armor[] Armor { get; set; }
}