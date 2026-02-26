namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("movement", IsNullable=false)]
public class Movement 
{
    public Speed Speed { get; set; }
    public Basespeed Basespeed { get; set; }
    [XmlElement("special")]
    public Special[] Special { get; set; }
}