namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("image", IsNullable=false)]
public class Image 
{
    [XmlAttribute]
    public string Filename { get; set; }
}