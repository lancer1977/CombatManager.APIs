namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("maneuvertype", IsNullable=false)]
public class Maneuvertype 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Bonus { get; set; }
    [XmlAttribute]
    public string Cmb { get; set; }
    [XmlAttribute]
    public string Cmd { get; set; }
}