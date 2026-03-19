namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("type", IsNullable=false)]
public class Type 
{
    public Type() {Active = TypeActive.No;}
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(TypeActive.No)]
    public TypeActive Active { get; set; }
}