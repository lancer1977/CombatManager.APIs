namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellsmemorized", IsNullable=false)]
public class Spellsprepared 
{
    [XmlElement("spell")]
    public Spell[] Spell { get; set; }
}