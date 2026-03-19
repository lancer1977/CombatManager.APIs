namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("validation", IsNullable=false)]
public class Validation 
{
    public Report Report { get; set; }
    [XmlElement("validmessage")]
    public Validatemessage[] Validmessage { get; set; }
}