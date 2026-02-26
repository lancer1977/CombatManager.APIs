namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellbook", IsNullable=false)]
public class Spellbook 
{
    [XmlElement("spell")]
    public Spell[] Spell { get; set; }
}