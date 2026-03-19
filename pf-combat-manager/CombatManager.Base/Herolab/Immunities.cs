namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("immunities", IsNullable=false)]
public class Immunities 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}