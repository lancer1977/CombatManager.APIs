namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("images", IsNullable=false)]
public class Images 
{
    [XmlElement("image")]
    public Image[] Image { get; set; }
}