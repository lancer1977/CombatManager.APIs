namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("npcinfo", IsNullable=false)]
public class NpCinfo 
{
    [XmlAttribute]
    public string Name { get; set; }
    [XmlText]
    public string[] Text { get; set; }
}