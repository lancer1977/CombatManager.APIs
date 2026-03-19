namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("skilltrick", IsNullable=false)]
public class Skilltrick 
{
    public Skilltrick() {Useradded = SkilltrickUseradded.Yes;}
    public Description Description { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(SkilltrickUseradded.Yes)]
    public SkilltrickUseradded Useradded { get; set; }
}