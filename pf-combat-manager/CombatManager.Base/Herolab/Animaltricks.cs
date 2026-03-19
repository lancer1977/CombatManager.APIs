namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("animaltricks", IsNullable=false)]
public class Animaltricks 
{
    [XmlElement("animaltrick")]
    public Animaltrick[] Animaltrick { get; set; }
}