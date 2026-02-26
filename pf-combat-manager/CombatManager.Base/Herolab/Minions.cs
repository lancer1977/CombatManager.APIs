namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("minions", IsNullable=false)]
public class Minions 
{
    [XmlElement("character")]
    public Character[] Character { get; set; }
}