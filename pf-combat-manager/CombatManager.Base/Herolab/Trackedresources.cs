namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("trackedresources", IsNullable=false)]
public class Trackedresources 
{
    [XmlElement("trackedresource")]
    public Trackedresource[] Trackedresource { get; set; }
}