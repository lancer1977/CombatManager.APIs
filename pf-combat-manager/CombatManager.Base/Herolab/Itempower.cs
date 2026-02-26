namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("itempower", IsNullable=false)]
public class Itempower 
{
    public Description Description { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Pricebonusvalue { get; set; }
    [XmlAttribute]
    public string Pricebonustext { get; set; }
    [XmlAttribute]
    public string Pricecashvalue { get; set; }
    [XmlAttribute]
    public string Pricecashtext { get; set; }
}