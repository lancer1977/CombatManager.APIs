namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("flaw", IsNullable=false)]
public class Flaw 
{
    public Flaw() {Useradded = FlawUseradded.Yes;}
    public Description Description { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(FlawUseradded.Yes)]
    public FlawUseradded Useradded { get; set; }
}