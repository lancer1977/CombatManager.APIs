namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellclasses", IsNullable=false)]
public class Spellclasses 
{
    [XmlElement("spellclass")]
    public Spellclass[] Spellclass { get; set; }
}