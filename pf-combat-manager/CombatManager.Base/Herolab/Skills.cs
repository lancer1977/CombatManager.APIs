namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("skills", IsNullable=false)]
public class Skills 
{
    [XmlElement("skill")]
    public Skill[] Skill { get; set; }
}