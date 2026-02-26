namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("program", IsNullable=false)]
public class Program 
{
    public Programinfo Programinfo { get; set; }
    public Version Version { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Url { get; set; }
}