namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("xpaward", IsNullable=false)]
public class XPaward 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
}