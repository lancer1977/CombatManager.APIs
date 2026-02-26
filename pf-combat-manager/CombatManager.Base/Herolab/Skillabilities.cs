namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("skillabilities", IsNullable=false)]
public class Skillabilities 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}