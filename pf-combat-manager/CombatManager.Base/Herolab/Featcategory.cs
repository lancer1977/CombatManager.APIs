namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("featcategory", IsNullable=false)]
public class Featcategory 
{
    [XmlText]
    public string Value { get; set; }
}