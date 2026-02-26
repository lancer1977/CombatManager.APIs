using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public class Pf2Score : SimpleNotifyClass
    {
        string _name;
        Stat _keyStat;
        int _stat;
        ProficiencyRank _rank;
        int _item;
        int _armor;
        bool _usesArmor;
        bool _signature;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    Notify();
                }
            }
        }

        public int Stat
        {
            get => _stat;
            set
            {
                if (_stat != value)
                {
                    _stat = value;
                    Notify();
                }
            }
        }
        public Stat KeyStat
        {
            get => _keyStat;
            set
            {
                if (_keyStat != value)
                {
                    _keyStat = value;
                    Notify();
                }
            }
        }

        public ProficiencyRank Rank
        {
            get => _rank;
            set
            {
                if (_rank != value)
                {
                    _rank = value;
                    Notify();
                }
            }
        }

        public int Item
        {
            get => _item;
            set
            {
                if (_item != value)
                {
                    _item = value;
                    Notify();
                }
            }
        }

        public int Armor
        {
            get => _armor;
            set
            {
                if (_armor != value)
                {
                    _armor = value;
                    Notify();
                }
            }
        }

        public bool UsesArmor
        {
            get => _usesArmor;
            set
            {
                if (_usesArmor != value)
                {
                    _usesArmor = value;
                    Notify();
                }
            }
        }

        public bool Signature
        {
            get => _signature;
            set
            {
                if (_signature != value)
                {
                    _signature = value;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public int Total => _stat + _rank.Modifier() + _item + (_usesArmor ? _armor : 0);
    }
}
