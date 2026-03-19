namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("challengerating", IsNullable=false)]
public class Cr 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}