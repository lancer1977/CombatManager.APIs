namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("armorclass", IsNullable=false)]
public class Armorclass 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Ac { get; set; }
    [XmlAttribute]
    public string Flatfooted { get; set; }
    [XmlAttribute]
    public string Touch { get; set; }
    [XmlAttribute]
    public string Fromarmor { get; set; }
    [XmlAttribute]
    public string Fromdeflect { get; set; }
    [XmlAttribute]
    public string Fromdexterity { get; set; }
    [XmlAttribute]
    public string Fromdodge { get; set; }
    [XmlAttribute]
    public string Frommisc { get; set; }
    [XmlAttribute]
    public string Fromnatural { get; set; }
    [XmlAttribute]
    public string Fromshield { get; set; }
    [XmlAttribute]
    public string Fromsize { get; set; }
}