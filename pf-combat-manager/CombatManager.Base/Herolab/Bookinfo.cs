namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("bookinfo", IsNullable=false)]
public class Bookinfo 
{
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Id { get; set; }
}