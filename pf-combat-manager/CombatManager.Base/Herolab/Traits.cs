namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("traits", IsNullable=false)]
public class Traits 
{
    [XmlElement("trait")]
    public Trait[] Trait { get; set; }
}