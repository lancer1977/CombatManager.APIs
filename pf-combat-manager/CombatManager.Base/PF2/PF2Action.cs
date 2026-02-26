using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public enum Pf2ActionType
    {
        Free = 0,
        Single = 1,
        Two = 2,
        Three = 3,
        Reaction = int.MinValue

    }

    public class Pf2Action : SimpleNotifyClass, ICloneable
    {
        Pf2ActionType _type;
        string _name;
        string _weapon;
        int? _mod;
        ObservableCollection<Pf2ActionTrait> _traits;
        string _trigger;
        string _requirements;
        string _effect;
        int? _dc;
        Pf2SpellList _spellList;
        ObservableCollection<Pf2ActionDamage> _damage;

        public Pf2Action()
        {

        }

        public Pf2Action(Pf2Action r)
        {
            CopyFromInternal(r, true);
        }

        public void CopyFrom(Pf2Action r)
        {
            CopyFromInternal(r, false);
        }

         public void CopyFromInternal(Pf2Action r, bool newc)
        {
            Type = r._type;
            Name = r._name;
            Weapon = r._weapon;
            Mod = r._mod;
            if (newc)
            {
                _traits = r.Traits.CloneContents();
                _damage = r.Damage.CloneContents();
            }
            else
            {
                Traits.ReplaceClone(r.Traits);
                Damage.ReplaceClone(r.Damage);
            }
            Effect = r.Effect;
            Trigger = r.Trigger;
            Dc = r.Dc;
            SpellList = r._spellList;

        }

        public object Clone()
        {
            return new Pf2Action(this);

        }



        public Pf2ActionType Type
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

        public string Weapon
        {
            get => _weapon;
            set
            {
                if (_weapon != value)
                {
                    _weapon = value;
                    Notify();
                }
            }
        }

        public int? Mod
        {
            get => _mod;
            set
            {
                if (_mod != value)
                {
                    _mod = value;
                    Notify();
                }
            }
        }


        ObservableCollection<Pf2ActionTrait> Traits
        {
            get
            {
                if (_traits == null)
                {
                    _traits = new ObservableCollection<Pf2ActionTrait>();
                }

                return _traits;
            }

        }

        ObservableCollection<Pf2ActionDamage> Damage
        {
            get
            {
                if (_damage == null)
                {
                    _damage = new ObservableCollection<Pf2ActionDamage>();
                }

                return _damage;
            }

        }
        public string Requirements
        {
            get => _requirements;
            set
            {
                if (_requirements != value)
                {
                    _requirements = value;
                    Notify();
                }
            }
        }

        public string Trigger
        {
            get => _trigger;
            set
            {
                if (_trigger != value)
                {
                    _trigger = value;
                    Notify();
                }
            }
        }

        public string Effect
        {
            get => _effect;
            set
            {
                if (_effect != value)
                {
                    _effect = value;
                    Notify();
                }
            }
        }
        public int? Dc
        {
            get => _dc;
            set
            {
                if (_dc != value)
                {
                    _dc = value;
                    Notify("DC");
                }
            }
        }

        public Pf2SpellList SpellList
        {
            get => _spellList;
            set
            {
                if (_spellList != value)
                {
                    _spellList = value.CloneOrNull();
                    Notify();
                }
            }
        }
    }
}