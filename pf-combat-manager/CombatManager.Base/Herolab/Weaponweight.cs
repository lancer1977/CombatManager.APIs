namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("weight", IsNullable=false)]
public class Weaponweight 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}