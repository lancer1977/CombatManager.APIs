using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public class Pf2ActionTrait : SimpleNotifyClass, ICloneable
    {
        private string _name;
        private DieRoll _dieRoll;
        private int? _feet;

        public Pf2ActionTrait() { }

        public Pf2ActionTrait(Pf2ActionTrait r)
        {
            CopyFrom(r);
        }

        public void CopyFrom(Pf2ActionTrait r)
        {
            Name = r._name;
            DieRoll = r._dieRoll;
            Feet = r._feet;

        }

        public object Clone()
        {
            return new Pf2ActionTrait(this);

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

        public DieRoll DieRoll
        {
            get => _dieRoll;
            set
            {
                if (_dieRoll != value)
                {
                    _dieRoll = value.CloneOrNull();
                    Notify();
                }
            }
        }

        public int? Feet
        {
            get => _feet;
            set
            {
                if (_feet != value)
                {
                    _feet = value;
                    Notify();
                }
            }
        }

        public override string ToString()
        {

            List<string> list = new List<string>();
            list.AddIfNotNull(_name);
            list.AddIfNotNull(_dieRoll);
            list.AddIfNotNull(_feet, postfix: " feet");
            return list.WeaveString(" ");

        }
    }
}
