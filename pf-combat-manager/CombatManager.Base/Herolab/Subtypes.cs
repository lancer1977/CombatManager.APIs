namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("subtypes", IsNullable=false)]
public class Subtypes 
{
    [XmlElement("subtype")]
    public Subtype[] Subtype { get; set; }
}