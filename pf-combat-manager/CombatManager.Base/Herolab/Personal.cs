namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("personal", IsNullable=false)]
public class Personal 
{
    public Description Description { get; set; }
    public Height Charheight { get; set; }
    public Weight Charweight { get; set; }
    [XmlAttribute]
    public string Age { get; set; }
    [XmlAttribute]
    public string Eyes { get; set; }
    [XmlAttribute]
    public Gender Gender { get; set; }
    [XmlIgnore]
    public bool GenderSpecified { get; set; }
    [XmlAttribute]
    public string Hair { get; set; }
    [XmlAttribute]
    public string Skin { get; set; }
}