namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("alignment", IsNullable=false)]
public class Alignment 
{
    [XmlAttribute]
    public string Name { get; set; }
}