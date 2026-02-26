namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("skilltricks", IsNullable=false)]
public class Skilltricks 
{
    [XmlElement("skilltrick")]
    public Skilltrick[] Skilltrick { get; set; }
}