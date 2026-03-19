namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("attribute", IsNullable=false)]
public class Attribute 
{
    public Attrvalue Attrvalue { get; set; }
    public Attrbonus Attrbonus { get; set; }
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
}