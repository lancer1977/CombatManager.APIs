namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("flaws", IsNullable=false)]
public class Flaws 
{
    [XmlElement("flaw")]
    public Flaw[] Flaw { get; set; }
}