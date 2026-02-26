namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellclass", IsNullable=false)]
public class Spellclass 
{
    [XmlElement("spelllevel")]
    public Spelllevel[] Spelllevel { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Maxspelllevel { get; set; }
    [XmlAttribute]
    public string Spells { get; set; }
}