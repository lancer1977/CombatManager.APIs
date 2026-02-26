namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("size", IsNullable=false)]
public class Size 
{
    public Space Space { get; set; }
    public Reach Reach { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
}