namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("animaltrick", IsNullable=false)]
public class Animaltrick 
{
    public Animaltrick() {Useradded = AnimaltrickUseradded.Yes;}
    public Description Description { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(AnimaltrickUseradded.Yes)]
    public AnimaltrickUseradded Useradded { get; set; }
}