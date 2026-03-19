namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("race", IsNullable=false)]
public class Race 
{
    [XmlAttribute]
    public string Racetext { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Ethnicity { get; set; }
}