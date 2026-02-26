namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("types", IsNullable=false)]
public class Types 
{
    [XmlElement("type")]
    public Type[] Type { get; set; }
}