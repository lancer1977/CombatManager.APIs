namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellsknown", IsNullable=false)]
public class Spellsknown 
{
    [XmlElement("spell")]
    public Spell[] Spell { get; set; }
}