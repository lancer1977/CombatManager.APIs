namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("spelllevel", IsNullable=false)]
public class Spelllevel 
{
    public Spelllevel() 
    {
        Maxcasts = "0";
        Used = "0";
        Unlimited = SpelllevelUnlimited.No;
    }

    [XmlAttribute]
    public string Level { get; set; }
    [XmlAttribute, DefaultValue("0")]
    public string Maxcasts { get; set; }
    [XmlAttribute, DefaultValue("0")]
    public string Used { get; set; }
    [XmlAttribute, DefaultValue(SpelllevelUnlimited.No)]
    public SpelllevelUnlimited Unlimited { get; set; }
}