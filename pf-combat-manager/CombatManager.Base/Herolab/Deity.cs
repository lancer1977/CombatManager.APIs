namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("deity", IsNullable=false)]
public class Deity 
{
    [XmlAttribute]
    public string Name { get; set; }
}