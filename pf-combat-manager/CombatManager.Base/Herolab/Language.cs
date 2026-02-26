namespace CombatManager.Herolab;

[Serializable]
[XmlRoot("language", IsNullable=false)]
public class Language 
{
    public Language() {Useradded = LanguageUseradded.Yes;}
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute, DefaultValue(LanguageUseradded.Yes)]
    public LanguageUseradded Useradded { get; set; }
}