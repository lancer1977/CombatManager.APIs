namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("version", IsNullable=false)]
public class Version
{
    //TODO: Version conflicts with object name
    //[XmlAttribute]
    //public string Version { get; set; }
    [XmlAttribute]
    public string Build { get; set; }
    [XmlAttribute]
    public string Primary { get; set; }
    [XmlAttribute]
    public string Secondary { get; set; }
    [XmlAttribute]
    public string Tertiary { get; set; }
}