namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("allsaves", IsNullable=false)]
public class Allsaves 
{
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Save { get; set; }
    [XmlAttribute]
    public string Base { get; set; }
    [XmlAttribute]
    public string Frommisc { get; set; }
    [XmlAttribute]
    public string Fromresist { get; set; }
}