namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("initiative", IsNullable=false)]
public class Initiative 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Total { get; set; }
    [XmlAttribute]
    public string Attrtext { get; set; }
    [XmlAttribute]
    public string Misctext { get; set; }
    [XmlAttribute]
    public string Attrname { get; set; }
}