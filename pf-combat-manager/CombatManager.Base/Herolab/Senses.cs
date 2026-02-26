namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("senses", IsNullable=false)]
public class Senses 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}