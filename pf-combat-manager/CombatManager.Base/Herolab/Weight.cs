namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("charweight", IsNullable=false)]
public class Weight 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}