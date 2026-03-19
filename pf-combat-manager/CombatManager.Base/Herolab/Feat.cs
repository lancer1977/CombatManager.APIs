namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("feat", IsNullable=false)]
public class Feat 
{
    public Feat() 
    {
        Profgroup = FeatProfgroup.No;
        Useradded = FeatUseradded.Yes;
    }

    public Description Description { get; set; }
    [XmlElement("featcategory")]
    public Featcategory[] Featcategory { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Categorytext { get; set; }
    [XmlAttribute, DefaultValue(FeatProfgroup.No)]
    public FeatProfgroup Profgroup { get; set; }
    [XmlAttribute, DefaultValue(FeatUseradded.Yes)]
    public FeatUseradded Useradded { get; set; }
}