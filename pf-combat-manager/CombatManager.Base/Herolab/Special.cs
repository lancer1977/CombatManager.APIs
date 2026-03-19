namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("special", IsNullable=false)]
public class Special 
{
    public Description Description { get; set; }
    [XmlElement("specsource")]
    public Specsource[] Specsource { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Shortname { get; set; }
    [XmlAttribute]
    public string Sourcetext { get; set; }
    [XmlAttribute]
    public string Type { get; set; }
}