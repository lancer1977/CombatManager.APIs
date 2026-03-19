namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("weptype", IsNullable=false)]
public class Weapontype 
{
    [XmlText]
    public string Value { get; set; }
}