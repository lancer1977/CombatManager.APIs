namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spelllike", IsNullable=false)]
public class Spelllike 
{
    [XmlElement("special")]
    public Special[] Special { get; set; }
}