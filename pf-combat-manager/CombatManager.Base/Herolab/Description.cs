namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("description", IsNullable=false)]
public class Description 
{
    [XmlText]
    public string Value { get; set; }
}