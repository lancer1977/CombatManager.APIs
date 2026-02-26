namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("character", IsNullable=false)]
public class Character 
{
    public Character() 
    {
        Active = CharacterActive.No;
        Nature = "normal";
        Role = Role.Pc;
        Relationship = Relationship.Ally;
        Type = "Hero";
    }

    public Bookinfo Bookinfo { get; set; }
    public Pathfindersociety Pathfindersociety { get; set; }
    public Race Race { get; set; }
    public Alignment Alignment { get; set; }
    public Templates Templates { get; set; }
    public Size Size { get; set; }
    public Deity Deity { get; set; }
    public Cr Challengerating { get; set; }
    public XPaward Xpaward { get; set; }
    public Classes Classes { get; set; }
    public Factions Factions { get; set; }
    public Types Types { get; set; }
    public Subtypes Subtypes { get; set; }
    public Heropoints Heropoints { get; set; }
    public Senses Senses { get; set; }
    public Auras Auras { get; set; }
    public Favoredclasses Favoredclasses { get; set; }
    public Health Health { get; set; }
    public Xp Xp { get; set; }
    public Money Money { get; set; }
    public Personal Personal { get; set; }
    public Languages Languages { get; set; }
    public Attributes Attributes { get; set; }
    public Saves Saves { get; set; }
    public Defensive Defensive { get; set; }
    public Dr Damagereduction { get; set; }
    public Immunities Immunities { get; set; }
    public Resistances Resistances { get; set; }
    public Weaknesses Weaknesses { get; set; }
    public Armorclass Armorclass { get; set; }
    public Penalties Penalties { get; set; }
    public Maneuvers Maneuvers { get; set; }
    public Initiative Initiative { get; set; }
    public Movement Movement { get; set; }
    public Encumbrance Encumbrance { get; set; }
    public Skills Skills { get; set; }
    public Skillabilities Skillabilities { get; set; }
    public Feats Feats { get; set; }
    public Traits Traits { get; set; }
    public Flaws Flaws { get; set; }
    public Skilltricks Skilltricks { get; set; }
    public Animaltricks Animaltricks { get; set; }
    public Attack Attack { get; set; }
    public Melee Melee { get; set; }
    public Ranged Ranged { get; set; }
    public Defenses Defenses { get; set; }
    public Magicitems Magicitems { get; set; }
    public Gear Gear { get; set; }
    public Spelllike Spelllike { get; set; }
    public Trackedresources Trackedresources { get; set; }
    public Otherspecials Otherspecials { get; set; }
    public Spellsknown Spellsknown { get; set; }
    public Spellsprepared Spellsmemorized { get; set; }
    public Spellbook Spellbook { get; set; }
    public Spellclasses Spellclasses { get; set; }
    public Journals Journals { get; set; }
    public Images Images { get; set; }
    public Validation Validation { get; set; }
    public Settings Settings { get; set; }
    public Npc Npc { get; set; }
    public Minions Minions { get; set; }
    [XmlAttribute, DefaultValue(CharacterActive.No)]
    public CharacterActive Active { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Playername { get; set; }
    [XmlAttribute]
    public string Nature { get; set; }
    [XmlAttribute, DefaultValue(Role.Pc)]
    public Role Role { get; set; }
    [XmlAttribute, DefaultValue(Relationship.Ally)]
    public Relationship Relationship { get; set; }
    [XmlAttribute, DefaultValue("Hero")]
    public string Type { get; set; }
}