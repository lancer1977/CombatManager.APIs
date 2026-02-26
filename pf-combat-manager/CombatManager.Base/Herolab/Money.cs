namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("money", IsNullable=false)]
public class Money 
{
    [XmlAttribute]
    public string Cp { get; set; }
    [XmlAttribute]
    public string Gp { get; set; }
    [XmlAttribute]
    public string Pp { get; set; }
    [XmlAttribute]
    public string Sp { get; set; }
    [XmlAttribute]
    public string Total { get; set; }
    [XmlAttribute]
    public string Valuables { get; set; }
}