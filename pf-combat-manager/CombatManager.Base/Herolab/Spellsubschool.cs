namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellsubschool", IsNullable=false)]
public class Spellsubschool 
{
    [XmlText]
    public string Value { get; set; }
}