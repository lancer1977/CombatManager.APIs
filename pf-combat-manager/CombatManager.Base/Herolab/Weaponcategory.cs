namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("wepcategory", IsNullable=false)]
public class Weaponcategory 
{
    [XmlText]
    public string Value { get; set; }
}