namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("weaknesses", IsNullable=false)]
public class Weaknesses 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}