namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("encumbrance", IsNullable=false)]
public class Encumbrance 
{
    [XmlAttribute]
    public string Carried { get; set; }
    [XmlAttribute]
    public string Encumstr { get; set; }
    [XmlAttribute]
    public string Heavy { get; set; }
    [XmlAttribute]
    public string Level { get; set; }
    [XmlAttribute]
    public string Light { get; set; }
    [XmlAttribute]
    public string Medium { get; set; }
}