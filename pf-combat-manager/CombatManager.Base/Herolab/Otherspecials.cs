namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("otherspecials", IsNullable=false)]
public class Otherspecials 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}