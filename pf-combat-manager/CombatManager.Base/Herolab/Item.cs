namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("item", IsNullable=false)]
public class Item 
{
    public Item() 
    {
        Useradded = ItemUseradded.Yes;
        Quantity = "1";
    }

    public Weaponweight Weight { get; set; }
    public Cost Cost { get; set; }
    public Description Description { get; set; }
    public Itemslot Itemslot { get; set; }
    [XmlElement("itempower")]
    public Itempower[] Itempower { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(ItemUseradded.Yes)]
    public ItemUseradded Useradded { get; set; }
    [XmlAttribute, DefaultValue("1")]
    public string Quantity { get; set; }
}