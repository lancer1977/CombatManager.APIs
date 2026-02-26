namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("weapon", IsNullable=false)]
public class Weapon 
{
    public Weapon() 
    {
        Equipped = WeaponEquipped.No;
        Useradded = WeaponUseradded.Yes;
        Quantity = "1";
    }

    public Rangedattack Rangedattack { get; set; }
    public Weaponweight Weight { get; set; }
    public Cost Cost { get; set; }
    public Description Description { get; set; }
    public Itemslot Itemslot { get; set; }
    [XmlElement("itempower")]
    public Itempower[] Itempower { get; set; }
    [XmlElement("wepcategory")]
    public Weaponcategory[] Wepcategory { get; set; }
    [XmlElement("weptype")]
    public Weapontype[] Weptype { get; set; }
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Attack { get; set; }
    [XmlAttribute]
    public string Damage { get; set; }
    [XmlAttribute]
    public string Crit { get; set; }
    [XmlAttribute]
    public string Categorytext { get; set; }
    [XmlAttribute]
    public string Typetext { get; set; }
    [XmlAttribute]
    public string Size { get; set; }
    [XmlAttribute]
    public string Flurryattack { get; set; }
    [XmlAttribute, DefaultValue(WeaponEquipped.No)]
    public WeaponEquipped Equipped { get; set; }
    [XmlAttribute, DefaultValue(WeaponUseradded.Yes)]
    public WeaponUseradded Useradded { get; set; }
    [XmlAttribute, DefaultValue("1")]
    public string Quantity { get; set; }
}