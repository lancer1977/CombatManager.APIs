namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("subtype", IsNullable=false)]
public class Subtype 
{
    [XmlAttribute]
    public string Name { get; set; }
}