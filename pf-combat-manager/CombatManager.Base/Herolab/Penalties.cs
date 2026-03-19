namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("penalties", IsNullable=false)]
public class Penalties 
{
    [XmlElement("penalty")]
    public Penalty[] Penalty { get; set; }
}