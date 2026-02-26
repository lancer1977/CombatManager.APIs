namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("auras", IsNullable=false)]
public class Auras 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}