namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("damagereduction", IsNullable=false)]
public class Dr 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}