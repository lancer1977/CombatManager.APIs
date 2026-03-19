namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("report", IsNullable=false)]
public class Report 
{
    [XmlText]
    public string Value { get; set; }
}