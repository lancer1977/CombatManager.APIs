namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("localization", IsNullable=false)]
public class Localization 
{
    [XmlAttribute]
    public string Language { get; set; }
    [XmlAttribute]
    public string Units { get; set; }
}