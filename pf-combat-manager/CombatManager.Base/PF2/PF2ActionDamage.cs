using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public class Pf2ActionDamage : SimpleNotifyClass, ICloneable
    {
        private DieRoll _dieRoll;
        private string _type;
        private int? _feet;

        public Pf2ActionDamage() { }

        public Pf2ActionDamage(Pf2ActionDamage r)
        {
            CopyFrom(r);
        }

        public object Clone()
        {
            return new Pf2ActionDamage(this);
        }

        public void CopyFrom(Pf2ActionDamage r)
        {
            DieRoll = r._dieRoll.CloneOrNull();
            Type = r._type;
            Feet = r._feet;
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

        public string Type
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
            list.AddIfNotNull(_dieRoll);
            list.AddIfNotNull(_type);
            list.AddIfNotNull(_feet, postfix: " feet");
            return list.WeaveString(" ");
        }

    }
}
