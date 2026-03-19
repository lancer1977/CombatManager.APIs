namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("settings", IsNullable=false)]
public class Settings 
{
    [XmlAttribute]
    public string Summary { get; set; }
}