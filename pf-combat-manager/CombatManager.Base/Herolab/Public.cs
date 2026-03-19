namespace CombatManager.Herolab;

[Serializable]
    [XmlRoot("public", IsNullable=false)]
    public class Public 
    {
        public Program Program { get; set; }
        public Localization Localization { get; set; }
        [XmlElement("character")]
        public Character[] Character { get; set; }
    }