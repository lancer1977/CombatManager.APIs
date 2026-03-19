namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("classes", IsNullable=false)]
public class Classes 
{
    [XmlElement("class")]
    public Class[] Class { get; set; }
    [XmlAttribute]
    public string Level { get; set; }
    [XmlAttribute]
    public string Summary { get; set; }
    [XmlAttribute]
    public string Summaryabbr { get; set; }
}