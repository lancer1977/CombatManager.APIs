using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public class Pf2SpellList : SimpleNotifyClass, ICloneable
    {
        string _class;
        ObservableCollection<Pf2SpellLevel> _levels;

        public Pf2SpellList() { }

        public object Clone()
        {
            return new Pf2SpellList(this);
        }

        public Pf2SpellList(Pf2SpellList r)
        {
            CopyFromInternal(r, true);
        }

        public void CopyFrom(Pf2SpellList r)
        {
            CopyFromInternal(r, false);
        }

        public void CopyFromInternal(Pf2SpellList r, bool newc)
        {
            Class = r._class;
            if (newc)
            {
                _levels = r.Levels.CloneContents();
            }
            else
            {
                Levels.ReplaceClone(r.Levels);
            }
        }


        public string Class
        {
            get => _class;
            set
            {
                if (_class != value)
                {
                    _class = value;
                    Notify();
                }
            }
        }


        ObservableCollection<Pf2SpellLevel> Levels
        {
            get
            {
                if (_levels == null)
                {
                    _levels = new ObservableCollection<Pf2SpellLevel>();
                }

                return _levels;
            }

        }

    }

   
}