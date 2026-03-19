using CombatManager.Utilities;

namespace CombatManager.PF2
{
    class Pf2Stats : SimpleNotifyClass
    {
        private string _ancestry;
        private string _background;
        private string _age;
        private int _totalLevel;
        private Stat _keyStat;
        private int _heroPoints;

        public string Ancestry
        {
            get => _ancestry;
            set
            {
                if (_ancestry != value)
                {
                    _ancestry = value;
                    Notify();
                }
            }
        }
        public string Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    Notify();
                }
            }
        }

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
        public int TotalLevel
        {
            get => _totalLevel;
            set
            {
                if (_totalLevel != value)
                {
                    _totalLevel = value;
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
        public int HeroPoints
        {
            get => _heroPoints;
            set
            {
                if (_heroPoints != value)
                {
                    _heroPoints = value;
                    Notify();
                }
            }
        }

    }
}
