using CombatManager.Utilities;

namespace CombatManager.PF2
{
    public class Pf2SpellLevel : SimpleNotifyClass, ICloneable
    {
        public const int Cantrip = 0;

        int _level;
        int? _cantripLevel;

        ObservableCollection<string> _spells;

        public Pf2SpellLevel() { }

        public Pf2SpellLevel(Pf2SpellLevel r)
        {
            CopyFromInternal(r, true);
        }

        public void CopyFrom(Pf2SpellLevel r)
        {
            CopyFromInternal(r, false);
        }

        public void CopyFromInternal(Pf2SpellLevel r, bool newc)
        {
            Level = r._level;
            if (newc)
            {
                _spells = r._spells.CloneContents();
            }
            else
            {
                _spells.ReplaceClone(r.Spells);
            }
        }

        public object Clone()
        {
            return new Pf2SpellLevel(this);
        }

        public int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    Notify();
                }
            }
        }
        public int? CantripLevel
        {
            get => _cantripLevel;
            set
            {
                if (_cantripLevel != value)
                {
                    _cantripLevel = value;
                    Notify();
                }
            }
        }


        ObservableCollection<string> Spells
        {
            get
            {
                if (_spells == null)
                {
                    _spells = new ObservableCollection<string>();
                }

                return _spells;
            }

        }

        [XmlIgnore]
        public string Title
        {
            get
            {
                if (_level == Cantrip)
                {
                    return _level.PastTense();
                }
                else
                {
                    string text = "Cantrips";
                    if (_cantripLevel != null)
                    {
                        text += " (" + _cantripLevel.Value.PastTense() + ")";
                    }
                    return text;

                }
            }
        }

        [XmlIgnore]
        public string AllSpells => Spells.WeaveString(", ");
    }
}