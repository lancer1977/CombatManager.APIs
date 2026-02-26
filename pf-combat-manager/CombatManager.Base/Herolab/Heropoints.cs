namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("heropoints", IsNullable=false)]
public class Heropoints 
{
    public Heropoints() {Enabled = HeropointsEnabled.Yes;}
    [XmlAttribute, DefaultValue(HeropointsEnabled.Yes)]
    public HeropointsEnabled Enabled { get; set; }
    [XmlAttribute]
    public string Total { get; set; }
}