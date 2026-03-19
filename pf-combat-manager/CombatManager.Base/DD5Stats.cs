using CombatManager.Utilities;

namespace CombatManager
{
    public enum Dd5Skill
    {
        Acrobatics = 0,
        AnimalHandling = 1,
        Arcana = 2,
        Athletics = 3,
        Deception = 4,
        History = 5,
        Insight = 6,
        Initimidation = 7,
        Investigation = 8,
        Medicine = 9,
        Nature = 10,
        Perception = 11,
        Performance = 12,
        Persuasion = 13,
        Religion = 14,
        SleightOfHand = 15,
        Stealth = 16,
        Survival = 17
    }


    public class Dd5Stats : SimpleNotifyClass
    {

        private string _age;
        private string _eyes;
        private string _skin;
        private string _hair;


        private int _proficiencyBonus;
        private int _deathSaveSuccesses;
        private int _deathSaveFailures;

        private ObservableCollection<string> _personalityTrails;
        private ObservableCollection<string> _bonds;
        private ObservableCollection<string> _ideals;
        private ObservableCollection<string> _flaws;

        private int? _spellSaveDc;
        private int? _spellAttack;


        private ObservableCollection<bool> _skillsList;

        public string Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    Notify();
                }
            }
        }
        public string Eyes
        {
            get => _eyes;
            set
            {
                if (_eyes != value)
                {
                    _eyes = value;
                    Notify();
                }
            }
        }
        public string Skin
        {
            get => _skin;
            set
            {
                if (_skin != value)
                {
                    _skin = value;
                    Notify();
                }
            }
        }
        public string Hair
        {
            get => _hair;
            set
            {
                if (_hair != value)
                {
                    _hair = value;
                    Notify();
                }
            }
        }

        public int ProficiencyBonus
        {
            get => _proficiencyBonus;
            set
            {
                if (_proficiencyBonus != value)
                {
                    _proficiencyBonus = value;
                    Notify();
                }
            }
        }

        public int? SpellSaveDc
        {
            get => _spellSaveDc;
            set
            {
                if (_spellSaveDc != value)
                {
                    _spellSaveDc = value;
                    Notify("SpellSaveDC");
                }
            }
        }
        public int? SpellAttack
        {
            get => _spellAttack;
            set
            {
                if (_spellAttack != value)
                {
                    _spellAttack = value;
                    Notify();
                }
            }
        }

        public ObservableCollection<bool> SkillsList
        {
            get
            {
                if (_skillsList == null)
                {
                    _skillsList = new ObservableCollection<bool>();
                    _skillsList.CollectionChanged += SkillsList_CollectionChanged;
                    int count = Enum.GetValues(typeof(Dd5Skill)).Length;
                    for (int i = 0; i < count; i++)
                    {
                        _skillsList.Add(false);
                    }
                }
                return _skillsList;
            }
            set
            {
                if (_skillsList != value)
                {
                    _skillsList = value;
                    Notify();
                    Notify("Skills");
                }
            }
        }

        private void SkillsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("SkillsList");
            Notify("Skills");
        }

        private Dd5Skills _skills;

        [XmlIgnore]
        public Dd5Skills Skills
        {
            get
            {
                if (_skills == null)
                {
                    _skills = new Dd5Skills(this);
                }
                return _skills;
            }
        }

        public int DeathSaveSuccesses
        {
            get => _deathSaveSuccesses;
            set
            {
                if (_deathSaveSuccesses != value)
                {
                    _deathSaveSuccesses = value;
                    Notify();
                }
            }
            
        }
        public int DeathSaveFailures
        {
            get => _deathSaveFailures;
            set
            {
                if (_deathSaveFailures != value)
                {
                    _deathSaveFailures = value;
                    Notify();
                }
            }
        }

        public ObservableCollection<string> PersonalityTrails
        {
            get => _personalityTrails;
            set
            {
                if (_personalityTrails != value)
                {
                    _personalityTrails = value;
                    Notify();
                }
            }
        }
        public ObservableCollection<string> Bonds
        {
            get => _bonds;
            set
            {
                if (_bonds != value)
                {
                    _bonds = value;
                    Notify();
                }
            }
        }
        public ObservableCollection<string> Ideals
        {
            get => _ideals;
            set
            {
                if (_ideals != value)
                {
                    _ideals = value;
                    Notify();
                }
            }
        }
        public ObservableCollection<string> Flaws
        {
            get => _flaws;
            set
            {
                if (_flaws != value)
                {
                    _flaws = value;
                    Notify();
                }
            }
        }


        public class Dd5Skills : SimpleNotifyInternalClass<Dd5Stats>
        {
            public Dd5Skills(Dd5Stats stats) : base(stats)
            {
            }
            
            public bool this[Dd5Skill skill]
            {
                get => Parent.SkillsList[(int)skill];
                set
                {
                    if (Parent.SkillsList[(int)skill] != value)
                    {
                        Parent.SkillsList[(int)skill] = value;
                    }
                }

            }
        }





    }
}
