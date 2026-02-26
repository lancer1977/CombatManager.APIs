namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellschool", IsNullable=false)]
public class Spellschool 
{
    [XmlText]
    public string Value { get; set; }
}