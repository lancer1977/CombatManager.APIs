using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public enum Pf2MonsterType
    {
        Pc = 1,
        Creature = 2,
        Hazard = 3,
        Item = 4
    }

    public class Pf2Monster : BaseMonster, ICloneable
    {
        Pf2MonsterType _type;

        ObservableCollection<string> _traits;
        ObservableCollection<Pf2Action> _actions;

        public Pf2Monster()
        {
        }

        public Pf2Monster(Pf2Monster m)
        {
            CopyFromInternal(m, true);
        }



        public void CopyFrom(Pf2Monster m)
        {
            CopyFromInternal(m, false);
        }

        public void CopyFromInternal(Pf2Monster m, bool newc)
        {
            BaseMonsterCopy(m);
            _type = m.Type;
            if (newc)
            {
                _traits = m.Traits.CloneContents();
                _actions = m.Actions.CloneContents();
            }
            else
            {
                Traits.ReplaceContents(m.Traits);
                Actions.ReplaceContents(m.Actions);
            }

        }

        public object Clone()
        {
            return new Pf2Monster(this); ;
        }

        public Pf2MonsterType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    Notify();
                }
            }
        }


        public ObservableCollection<string> Traits
        {
            get
            {
                if (_traits == null)
                {
                    _traits = new ObservableCollection<string>();

                }

                return _traits;

            }
        }
        
        public ObservableCollection<Pf2Action> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = new ObservableCollection<Pf2Action>();

                }

                return _actions;

            }
        }

        public bool AddTrait(string trait)
        {
            bool add = true;
            if (_traits == null)
            {
                _traits = new ObservableCollection<string>();
            }
            else
            {
                add = !_traits.Contains(trait);
            }
            if (add)
            {
                _traits.Add(trait);
            }
            return add;
        }

        public bool RemoveTrait(string trait)
        {
            if (_traits == null)
            {
                return false;
            }
            if (!_traits.Contains(trait))
            {
                return false;
            }
            _traits.Remove(trait);
            return true;

        }
    }
}
