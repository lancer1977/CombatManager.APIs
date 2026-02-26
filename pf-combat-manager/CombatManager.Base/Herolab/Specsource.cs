namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("specsource", IsNullable=false)]
public class Specsource 
{
    [XmlText]
    public string Value { get; set; }
}