namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("feats", IsNullable=false)]
public class Feats 
{
    [XmlElement("feat")]
    public Feat[] Feat { get; set; }
}