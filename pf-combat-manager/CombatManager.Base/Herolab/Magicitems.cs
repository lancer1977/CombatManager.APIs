namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("magicitems", IsNullable=false)]
public class Magicitems 
{
    [XmlElement("item")]
    public Item[] Item { get; set; }
}