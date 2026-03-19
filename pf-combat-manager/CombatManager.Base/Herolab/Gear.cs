namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("gear", IsNullable=false)]
public class Gear 
{
    [XmlElement("item")]
    public Item[] Item { get; set; }
}