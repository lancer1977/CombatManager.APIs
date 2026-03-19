namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("armor", IsNullable=false)]
public class Armor 
{
    public Armor() 
    {
        Equipped = ArmorEquipped.No;
        Natural = Naturalarmor.No;
        Useradded = ArmorUseradded.Yes;
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
    [XmlAttribute]
    public string Ac { get; set; }
    [XmlAttribute, DefaultValue(ArmorEquipped.No)]
    public ArmorEquipped Equipped { get; set; }
    [XmlAttribute, DefaultValue(Naturalarmor.No)]
    public Naturalarmor Natural { get; set; }
    [XmlAttribute, DefaultValue(ArmorUseradded.Yes)]
    public ArmorUseradded Useradded { get; set; }
    [XmlAttribute, DefaultValue("1")]
    public string Quantity { get; set; }
}