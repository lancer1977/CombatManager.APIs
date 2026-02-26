namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("journals", IsNullable=false)]
public class Journals 
{
    [XmlElement("journal")]
    public Journal[] Journal { get; set; }
}