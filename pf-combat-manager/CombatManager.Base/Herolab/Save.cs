namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("save", IsNullable=false)]
public class Save 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Abbr { get; set; }
    //TODO: SAve conflicts with object name
    //[XmlAttribute]
    //public string Save { get; set; }
    [XmlAttribute]
    public string Base { get; set; }
    [XmlAttribute]
    public string Fromattr { get; set; }
    [XmlAttribute]
    public string Frommisc { get; set; }
    [XmlAttribute]
    public string Fromresist { get; set; }
}