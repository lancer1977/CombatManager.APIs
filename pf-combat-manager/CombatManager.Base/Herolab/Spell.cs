namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spell", IsNullable=false)]
public class Spell 
{
    public Spell() 
    {
        Useradded = SpellUseradded.Yes;
        Spontaneous = SpellSpontaneous.No;
        Unlimited = SpellUnlimited.No;
    }

    public Description Description { get; set; }
    [XmlElement("spellcomp")]
    public Spellcomponent[] Spellcomp { get; set; }
    [XmlElement("spellschool")]
    public Spellschool[] Spellschool { get; set; }
    [XmlElement("spellsubschool")]
    public Spellsubschool[] Spellsubschool { get; set; }
    [XmlElement("spelldescript")]
    public Spelldescription[] Spelldescript { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Level { get; set; }
    [XmlAttribute]
    public string Area { get; set; }
    [XmlAttribute]
    public string Casterlevel { get; set; }
    [XmlAttribute]
    public string Castsleft { get; set; }
    [XmlAttribute]
    public string Casttime { get; set; }
    [XmlAttribute]
    public string Class { get; set; }
    [XmlAttribute]
    public string Componenttext { get; set; }
    [XmlAttribute]
    public string Dc { get; set; }
    [XmlAttribute]
    public string Descriptor { get; set; }
    [XmlAttribute]
    public string Duration { get; set; }
    [XmlAttribute]
    public string Effect { get; set; }
    [XmlAttribute]
    public string Range { get; set; }
    [XmlAttribute]
    public string Resist { get; set; }
    [XmlAttribute]
    public string Save { get; set; }
    [XmlAttribute]
    public string Schooltext { get; set; }
    [XmlAttribute]
    public string Subschooltext { get; set; }
    [XmlAttribute]
    public string Descriptortext { get; set; }
    [XmlAttribute]
    public string Savetext { get; set; }
    [XmlAttribute]
    public string Resisttext { get; set; }
    [XmlAttribute]
    public string Target { get; set; }
    [XmlAttribute, DefaultValue(SpellUseradded.Yes)]
    public SpellUseradded Useradded { get; set; }
    [XmlAttribute, DefaultValue(SpellSpontaneous.No)]
    public SpellSpontaneous Spontaneous { get; set; }
    [XmlAttribute, DefaultValue(SpellUnlimited.No)]
    public SpellUnlimited Unlimited { get; set; }
}