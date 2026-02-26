namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("favoredclasses", IsNullable=false)]
public class Favoredclasses 
{
    [XmlElement("favoredclass")]
    public Favoredclass[] Favoredclass { get; set; }
}