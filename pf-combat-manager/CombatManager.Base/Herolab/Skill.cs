namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("skill", IsNullable=false)]
public class Skill 
{
    public Skill() 
    {
        Armorcheck = SkillArmorcheck.No;
        Classskill = Classskill.No;
        Trainedonly = Trainedonly.No;
        Usable = SkillUsable.No;
    }


    public Description Description { get; set; }
    public Situationalmodifiers Situationalmodifiers { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Value { get; set; }
    [XmlAttribute]
    public string Ranks { get; set; }
    [XmlAttribute]
    public string Attrbonus { get; set; }
    [XmlAttribute]
    public string Attrname { get; set; }
    [XmlAttribute]
    public SkillTools Tools { get; set; }
    [XmlIgnore]
    public bool ToolsSpecified { get; set; }
    [XmlAttribute, DefaultValue(SkillArmorcheck.No)]
    public SkillArmorcheck Armorcheck { get; set; }
    [XmlAttribute, DefaultValue(Classskill.No)]
    public Classskill Classskill { get; set; }
    [XmlAttribute, DefaultValue(Trainedonly.No)]
    public Trainedonly Trainedonly { get; set; }
    [XmlAttribute, DefaultValue(SkillUsable.No)]
    public SkillUsable Usable { get; set; }
}