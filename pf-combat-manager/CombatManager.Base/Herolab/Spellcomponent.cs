namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spellcomp", IsNullable=false)]
public class Spellcomponent 
{
    [XmlText]
    public string Value { get; set; }
}