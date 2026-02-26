namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("maneuvers", IsNullable=false)]
public class Maneuvers 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlElement("maneuvertype")]
    public Maneuvertype[] Maneuvertype { get; set; }
    [XmlAttribute]
    public string Cmb { get; set; }
    [XmlAttribute]
    public string Cmd { get; set; }
    [XmlAttribute]
    public string Cmdflatfooted { get; set; }
}