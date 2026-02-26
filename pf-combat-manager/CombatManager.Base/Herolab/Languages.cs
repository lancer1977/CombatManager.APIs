namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("languages", IsNullable=false)]
public class Languages 
{
    [XmlElement("language")]
    public Language[] Language { get; set; }
}