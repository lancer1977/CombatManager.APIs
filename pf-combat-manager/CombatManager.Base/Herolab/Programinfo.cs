namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("programinfo", IsNullable=false)]
public class Programinfo
{
    [XmlText]
    public string Value { get; set; }
}