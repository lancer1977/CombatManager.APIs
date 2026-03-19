namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("saves", IsNullable=false)]
public class Saves 
{
    [XmlElement("save")]
    public Save[] Save { get; set; }
    public Allsaves Allsaves { get; set; }
}