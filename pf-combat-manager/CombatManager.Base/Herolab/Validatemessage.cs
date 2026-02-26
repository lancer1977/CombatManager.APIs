namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("validmessage", IsNullable=false)]
public class Validatemessage 
{
    [XmlText]
    public string Value { get; set; }
}