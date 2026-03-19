namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("journal", IsNullable=false)]
public class Journal 
{
    public Description Description { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Cp { get; set; }
    [XmlAttribute]
    public string Gamedate { get; set; }
    [XmlAttribute]
    public string Gp { get; set; }
    [XmlAttribute]
    public string Pp { get; set; }
    [XmlAttribute]
    public string Prestigeaward { get; set; }
    [XmlAttribute]
    public string Prestigespend { get; set; }
    [XmlAttribute]
    public string Realdate { get; set; }
    [XmlAttribute]
    public string Sp { get; set; }
    [XmlAttribute]
    public string Xp { get; set; }
}