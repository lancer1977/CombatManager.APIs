namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("trackedresource", IsNullable=false)]
public class Trackedresource 
{
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Text { get; set; }
    [XmlAttribute]
    public string Used { get; set; }
    [XmlAttribute]
    public string Left { get; set; }
    [XmlAttribute]
    public string Min { get; set; }
    [XmlAttribute]
    public string Max { get; set; }
}