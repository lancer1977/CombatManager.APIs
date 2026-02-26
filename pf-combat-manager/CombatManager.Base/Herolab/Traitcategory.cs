namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("traitcategory", IsNullable=false)]
public class Traitcategory 
{
    [XmlText]
    public string Value { get; set; }
}