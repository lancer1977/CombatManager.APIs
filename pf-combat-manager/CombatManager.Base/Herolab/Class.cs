namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("class", IsNullable=false)]
public class Class 
{
    public Arcanespellfailure Arcanespellfailure { get; set; }
    [XmlAttribute]
    public string Level { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Spells { get; set; }
    [XmlAttribute]
    public string Casterlevel { get; set; }
    [XmlAttribute]
    public string Concentrationcheck { get; set; }
    [XmlAttribute]
    public string Overcomespellresistance { get; set; }
    [XmlAttribute]
    public string Basespelldc { get; set; }
    [XmlAttribute]
    public string Castersource { get; set; }
}