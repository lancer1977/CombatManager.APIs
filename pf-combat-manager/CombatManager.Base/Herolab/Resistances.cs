namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("resistances", IsNullable=false)]
public class Resistances 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}