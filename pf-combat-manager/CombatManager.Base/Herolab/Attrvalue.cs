namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("attrvalue", IsNullable=false)]
public class Attrvalue 
{
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Base { get; set; }
    [XmlAttribute]
    public string Modified { get; set; }
}