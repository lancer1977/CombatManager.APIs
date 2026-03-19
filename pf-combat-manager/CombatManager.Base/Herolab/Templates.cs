namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("templates", IsNullable=false)]
public class Templates 
{
    [XmlAttribute]
    public string Summary { get; set; }
}