namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("npc", IsNullable=false)]
public class Npc 
{
    public Description Description { get; set; }
    public Basics Basics { get; set; }
    public Tactics Tactics { get; set; }
    public Ecology Ecology { get; set; }
    public Additional Additional { get; set; }
}