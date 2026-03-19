using System.Runtime.Serialization;
using CombatManager.ViewModels;
using ReactiveUI.Fody.Helpers;
using static CombatManager.Character;

namespace CombatManager
{
    public abstract class BaseMonster : BaseDbClass
    { 
        private string _cr;
        private string _alignment;
        private string _size;
        private int _hp;
        private string _speed;
        private int _init;
        private string _languages;
        private string _hpMods;
        private int? _fort;
        private int? _reflex;
        private int? _will;
        private string _resist;
        private string _immune;
        private string _weaknesses;
        private string _source;

        public ObservableCollection<ActiveCondition> ActiveConditions { get; } = new ObservableCollection<ActiveCondition>();
        public ObservableCollection<ActiveResource> TrackedResources { get; } = new ObservableCollection<ActiveResource>(); 

        public void BaseMonsterCopy(BaseMonster m)
        {
            Name = m.Name;
            Cr = m._cr;
            Alignment = m._alignment;
            Size = m._size;
            Hp = m._hp;
            Speed = m._speed;
            Init = m._init;
            Languages = m._languages;
            HpMods = m._hpMods;
            Fort = m._fort;
            Ref = m._reflex;
            Will = m._will;
            Resist = m._resist;
            Weaknesses = m._weaknesses;
            Immune = m._immune;
            Source = m._source;

        }

        public void BaseMonsterClone(BaseMonster m)
        {
            m.Name = Name;
            m._cr = _cr;
            m._alignment = _alignment;
            m._size = _size;
            m._hp = _hp;
            m._speed = _speed;
            m._init = _init;
            m._languages = _languages;
            m._hpMods = _hpMods;
            m._fort = _fort;
            m._reflex = _reflex;
            m._will = _will;
            m._resist = _resist;
            m._weaknesses = _weaknesses;
            m._immune = _immune;
            m._source = _source;
        }

        [DataMember][Reactive] public string Name { get; set; }

        [DataMember]
        public string Cr
        {
            get => _cr;
            set
            {
                _cr = value;
                Notify("CR");
            }
        }

        [DataMember]
        public string Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                Notify();
            }
        }

        [DataMember]
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                Notify();
            }
        }

        [DataMember]
        public int Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                Notify("HP");
            }
        }

        [DataMember]
        public string HpMods
        {
            get => _hpMods;
            set
            {
                _hpMods = value;
                Notify("HP_Mods");
            }
        }

        [DataMember]
        public string Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                Notify();
            }
        }

        [DataMember]
        public int Init
        {
            get => _init;
            set
            {
                _init = value;
                Notify();
            }
        }

        [DataMember]
        public string Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                Notify();
            }
        }

        [DataMember]
        public int? Fort
        {
            get => _fort;
            set
            {
                _fort = value;
                Notify();
            }
        }

        [DataMember]
        public int? Ref
        {
            get => _reflex;
            set
            {
                _reflex = value;
                Notify();
            }
        }

        [DataMember]
        public int? Will
        {
            get => _will;
            set
            {
                _will = value;
                Notify();
            }
        }

      
     

        [DataMember]
        public string Resist
        {
            get => _resist;
            set
            {
                _resist = value;
                Notify();
            }
        }
        [DataMember]
        public string Immune
        {
            get => _immune;
            set
            {
                _immune = value;
                Notify();
            }
        }
        [DataMember]
        public string Weaknesses
        {
            get => _weaknesses;
            set
            {
                _weaknesses = value;
                Notify();
            }
        }

        [DataMember]
        public string Source
        {
            get => _source;
            set
            {
                _source = value;
                Notify();
            }
        }

        [DataMember]
        public virtual int? Strength
        {
            get; set;
        }

        [DataMember]
        public virtual int? Dexterity
        {
            get; set;
        }

        [DataMember]
        public virtual int? Constitution
        {
            get; set;
        }

        [DataMember]
        public virtual int? Intelligence
        {
            get; set;
        }

        [DataMember]
        public virtual int? Wisdom
        {
            get; set;
        }

        [DataMember]
        public virtual int? Charisma
        {
            get; set;
        }


        public int StrengthBonus
        {
            get => AbilityBonus(Strength);
            set => Strength = AbilityFromBonus(value);
        }

        public int DexterityBonus
        {
            get => AbilityBonus(Dexterity);
            set => Dexterity = AbilityFromBonus(value);
        }

        public int ConstitutionBonus
        {
            get => AbilityBonus(Constitution);
            set => Constitution = AbilityFromBonus(value);
        }

        public int IntelligenceBonus
        {
            get => AbilityBonus(Intelligence);
            set => Intelligence = AbilityFromBonus(value);
        }

        public int WisdomBonus
        {
            get => AbilityBonus(Wisdom);
            set => Wisdom = AbilityFromBonus(value);
        }

        public int CharismaBonus
        {
            get => AbilityBonus(Charisma);
            set => Charisma = AbilityFromBonus(value);
        }

        public int? GetStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.Strength:
                    return Strength;
                case Stat.Dexterity:
                    return Dexterity;
                case Stat.Constitution:
                    return Constitution;
                case Stat.Intelligence:
                    return Intelligence;
                case Stat.Wisdom:
                    return Wisdom;
                case Stat.Charisma:
                default:
                    return Charisma;
            }
        }

        public int GetStatBonus(Stat stat)
        {
            return AbilityBonus(GetStat(stat));
        }

        public void SetStatDirect(Stat stat, int ? value)
        {

            switch (stat)
            {
                case Stat.Strength:
                    Strength = value;
                    break;
                case Stat.Dexterity:
                    Dexterity = value;
                    break;
                case Stat.Constitution:
                    Constitution = value;
                    break;
                case Stat.Intelligence:
                    Intelligence = value;
                    break;
                case Stat.Wisdom:
                    Wisdom = value;
                    break;
                case Stat.Charisma:
                    Charisma = value;
                    break;

                default:
                    throw new NotImplementedException(stat.ToString());
            }
        }

        public void SetAbilityBonus(Stat stat, int bonus)
        {
            AdjustStat(stat, AbilityFromBonus(bonus).Value);
        }

        public void AdjustStat(Stat stat, int value)
        {
            switch (stat)
            {
                case Stat.Strength:
                    AdjustStrength(value);
                    break;
                case Stat.Dexterity:
                    AdjustDexterity(value);
                    break;
                case Stat.Constitution:
                    AdjustConstitution(value);
                    break;
                case Stat.Intelligence:
                    AdjustIntelligence(value);
                    break;
                case Stat.Wisdom:
                    AdjustWisdom(value);
                    break;
                case Stat.Charisma:
                    AdjustCharisma(value);
                    break;
            }

        }


        public virtual void AdjustStrength(int value)
        {
            Strength = value;
        }
        public virtual void AdjustDexterity(int value)
        {
            Dexterity = value;
        }
        public virtual void AdjustConstitution(int value)
        {
            Constitution = value;
        }
        public virtual void AdjustIntelligence(int value)
        {
            Intelligence = value;
        }
        public virtual void AdjustWisdom(int value)
        {
            Wisdom = value;
        }
        public virtual void AdjustCharisma(int value)
        {
            Charisma = value;
        }

        public virtual void ApplyDefaultConditions()
        {
        }


        public virtual int GetStartingHp(HpMode mode)
        {
            return _hp;
        }

        public virtual IEnumerable<ActiveResource> LoadResources()
        {
            return new List<ActiveResource>();
        }
    

        public ActiveCondition FindCondition(string name)
        {
            return ActiveConditions.FirstOrDefault
                (a => string.Compare(a.Condition.Name, name, true) == 0);
        }

        public void AddCondition(ActiveCondition c)
        {
            ActiveConditions.Add(c);

            if (c.Bonus != null)
            {
                ApplyBonus(c.Bonus, false);
            }

        }

        public void RemoveCondition(ActiveCondition c)
        {
            ActiveConditions.Remove(c);

            if (c.Bonus != null)
            {
                ApplyBonus(c.Bonus, true);
            }
        }

        public virtual void ApplyBonus(ConditionBonus bonus, bool remove)
        {

        }

        public static int AbilityBonus(int? score)
        {
            if (score == null)
            {
                return 0;
            }

            return (score.Value / 2) - 5;
        }

        public static int? AbilityFromBonus(int bonus)
        {
            return (bonus * 2) + 10;
        }

        public static string GetSaveText(SaveType type)
        {
            if (type == SaveType.Fort)
            {
                return "Fort";
            }
            else if (type == SaveType.Ref)
            {
                return "Ref";
            }
            else if (type == SaveType.Will)
            {
                return "Will";
            }

            return "";
        }

        public enum OrderAxis
        {
            Lawful = 0,
            Neutral = 1,
            Chaotic = 2
        }

        public enum MoralAxis
        {
            Good = 0,
            Neutral = 1,
            Evil = 2
        }

        public enum SaveType
        {
            Fort = 0,
            Ref,
            Will
        }

        public struct AlignmentType
        {
            public OrderAxis Order;
            public MoralAxis Moral;
        }

        protected static string AddToStringList(string text, string type)
        {
            return AddToStringList(text, type, out _);

        }

        protected static string AddToStringList(string text, string type, out bool added)
        {
            added = false;


            string returnText = text;
            if (returnText == null)
            {
                returnText = "";
            }


            if (!StringListHasItem(returnText, type))
            {

                returnText = returnText + (returnText.Length > 0 ? ", " : "") + type;

                added = true;

            }

            return returnText;
        }

        protected static bool StringListHasItem(string list, string item)
        {
            Regex regType = new Regex(Regex.Escape(item) + "(\\Z|$|,)", RegexOptions.IgnoreCase);

            return regType.Match(list).Success;
        }


        protected static string RemoveFromStringList(string text, string type)
        {
            bool removed;
            return RemoveFromStringList(text, type, out removed);
        }

        protected static string RemoveFromStringList(string text, string type, out bool removed)
        {
            removed = false;

            Regex regex = new Regex("(^| )(" + type + ")(\\Z|,)");

            return regex.Replace(text, "").Trim();

        }
    }
}
