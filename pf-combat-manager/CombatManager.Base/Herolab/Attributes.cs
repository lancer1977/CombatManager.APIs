namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("attributes", IsNullable=false)]
public class Attributes 
{
    [XmlElement("attribute")]
    public Attribute[] Attribute { get; set; }
}