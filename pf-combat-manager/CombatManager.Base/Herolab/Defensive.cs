namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("defensive", IsNullable=false)]
public class Defensive 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}