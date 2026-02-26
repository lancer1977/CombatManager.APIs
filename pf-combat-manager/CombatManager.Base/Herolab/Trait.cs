namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("trait", IsNullable=false)]
public class Trait 
{
    public Trait() {Useradded = TraitUseradded.Yes;}
    public Description Description { get; set; }
    [XmlElement("traitcategory")]
    public Traitcategory[] Traitcategory { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Categorytext { get; set; }
    [XmlAttribute, DefaultValue(TraitUseradded.Yes)]
    public TraitUseradded Useradded { get; set; }
}