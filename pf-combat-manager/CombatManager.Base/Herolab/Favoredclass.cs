namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("favoredclass", IsNullable=false)]
public class Favoredclass 
{
    [XmlAttribute]
    public string Name { get; set; }
}