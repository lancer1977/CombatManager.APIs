namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("xp", IsNullable=false)]
public class Xp 
{
    [XmlAttribute]
    public string Total { get; set; }
}