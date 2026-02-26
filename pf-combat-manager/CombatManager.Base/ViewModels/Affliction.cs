/*
 *  Affliction.cs
 *
 *  Copyright (C) 2010-2012 Kyle Olson, kyle@kyleolson.com
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public License
 *  as published by the Free Software Foundation; either version 2
 *  of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

using CombatManager.Utilities;

namespace CombatManager.ViewModels
{
    public class Affliction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _type;
        private string _cause;
        private string _saveType;
        private int _save;
        private DieRoll _onset;
        private string _onsetUnit;
        private bool _immediate;
        private int _frequency;
        private string _frequencyUnit;
        private int _limit;
        private string _limitUnit;
        private string _specialEffectName;
        private DieRoll _specialEffectTime;
        private string _specialEffectUnit;
        private string _otherEffect;
        private bool _once;
        private DieRoll _damageDie;
        private string _damageType;
        private bool _isDamageDrain;
        private DieRoll _secondaryDamageDie;
        private string _secondaryDamageType;
        private bool _isSecondaryDamageDrain;
        private string _damageExtra;
        private string _cure;
        private string _details;
        private string _cost;

        public Affliction()
        {

        }

        public Affliction(Affliction a)
        {
            _name = a._name;
            _type = a._type;
            _cause = a._cause;
            _saveType = a._saveType;
            _save = a._save;
            if (a._onset != null)
            {
                _onset = (DieRoll)a._onset.Clone();
            }
            _onsetUnit = a._onsetUnit;
            _immediate = a._immediate;
            _frequency = a._frequency;
            _frequencyUnit = a._frequencyUnit;
            _limit = a._limit;
            _limitUnit = a._limitUnit;
            _once = a._once;
            if (a._damageDie != null)
            {
                _damageDie = (DieRoll)a._damageDie.Clone();
            }
            _damageType = a._damageType;
            _isDamageDrain = a.IsDamageDrain;
            if (a._secondaryDamageDie != null)
            {
                _secondaryDamageDie = (DieRoll)a._secondaryDamageDie.Clone();
            }
            _secondaryDamageType = a._secondaryDamageType;
            _isSecondaryDamageDrain = a._isSecondaryDamageDrain;
            _damageExtra = a._damageExtra;
            _specialEffectName = a._specialEffectName;
            if (a._specialEffectTime != null)
            {
                _specialEffectTime = (DieRoll)a._specialEffectTime.Clone();
            }
            _specialEffectUnit = a._specialEffectUnit;
            _otherEffect = a.OtherEffect;
            _cure = a._cure;
            _details = a._details;
            _cost = a._cost;

        }

        public object Clone()
        {
            return new Affliction(this);
        }

        public static Affliction FromSpecialAbility(Monster monster, SpecialAbility sa)
        {
            Affliction a = null;

            Regex reg = new Regex(RegexString, RegexOptions.IgnoreCase);

            Match m = reg.Match(sa.Text);

            if (m.Success)
            {
                a = new Affliction();

                a.Type = sa.Name;

                if (m.Groups["afflictionname"].Success && m.Groups["afflictionname"].Value.Trim().Length > 0)
                {
                    a.Name = m.Groups["afflictionname"].Value.Trim();

                    a.Name = a.Name.Capitalize();

                    if (a.Name == "Filth Fever")
                    {
                        a.Name += " - " + monster.Name;
                    }
                }
                else
                {
                    a.Name = monster.Name + " " + a.Type;
                }

                if (m.Groups["cause"].Success)
                {
                    a.Cause = m.Groups["cause"].Value.Trim();
                }

                a.SaveType = m.Groups["savetype"].Value;
                a.Save = int.Parse(m.Groups["savedc"].Value);
                
                if (m.Groups["onset"].Success)
                {
                    if (m.Groups["immediateonset"].Success)
                    {
                        a.Immediate = true;
                    }
                    else
                    {
                        a.Onset = GetDie(m.Groups["onsetdie"].Value);

                        a.OnsetUnit = m.Groups["onsetunit"].Value;
                    }
                }
                if (m.Groups["once"].Success)
                {
                    a.Once = true;
                }
                else
                {
                    a.Frequency = int.Parse(m.Groups["frequencycount"].Value);

                    a.FrequencyUnit = m.Groups["frequencyunit"].Value;

                    if (m.Groups["Limit"].Success)
                    {
                        a.Limit = int.Parse(m.Groups["limitcount"].Value);
                        a.LimitUnit = m.Groups["limittype"].Value;

                    }
                }

                if (m.Groups["damageeffect"].Success)
                {

                    a.DamageDie = GetDie(m.Groups["damagedie"].Value);
                    a.DamageType = m.Groups["damagetype"].Value;

                    if (m.Groups["secondarydamagedie"].Success)
                    {

                        a.SecondaryDamageDie = GetDie(m.Groups["secondarydamagedie"].Value);
                        a.SecondaryDamageType = m.Groups["secondarydamagetype"].Value;
                    }
                }
                else if (m.Groups["specialeffect"].Success)
                {

                    a.SpecialEffectTime = GetDie(m.Groups["specialeffectdie"].Value);
                    a.SpecialEffectName = m.Groups["specialeffectname"].Value;
                    a.SpecialEffectUnit = m.Groups["specialeffectunit"].Value;
                }
                else if (m.Groups["othereffect"].Success)
                {
                    a.OtherEffect = m.Groups["othereffect"].Value.Trim();
                }

                a.Cure = m.Groups["cure"].Value;

                if (m.Groups["details"].Success)
                {
                    a.Details = m.Groups["details"].Value.Trim();
                }
                

            }

            return a;
        }


        public static string RegexString =>
            "((^)|((?<afflictionname>[\\p{L} ]+): ))(?<cause>[- \\p{L}]+)" + 
            "; save (?<savetype>((Fort)|(Fortitude)|(Ref)|(Reflex)|(Will))) DC (?<savedc>[0-9]+)" +

            "(?<onset>; onset (((?<onsetdie>([0-9]+)(d[0-9]+)?) ((?<onsetunit>[\\p{L}]+?)s?))|(?<immediateonset>immediate)))?" + 
            "; frequency (((?<frequencycount>[0-9]+)([/ ]+)((?<frequencyunit>[\\p{L}]+?)s?)(?<limit> for (?<limitcount>[0-9]+) (?<limittype>[\\p{L}]+)s?)?)|(?<once>once))" +
            "; effect (" + 
            "(?<damageeffect>(?<damagedie>([0-9]+)(d[0-9]+)?) (?<damagetype>[\\p{L}]+)( and (?<secondarydamagedie>([0-9]+)(d[0-9]+)?) (?<secondarydamagetype>[\\p{L}]+))?( damage)?([-\\(\\) ,\\.\\p{L}0-9]+)?)|" +
            "(?<specialeffect>(?<specialeffectname>[\\p{L}]+) for (?<specialeffectdie>([0-9]+)(d[0-9]+)?) (?<specialeffectunit>[\\p{L}]+?)s?)|" +
            "(?<othereffect>[- '\\p{L}0-9]+)"+
            ")" +
            "(; cure (?<cure>([- +\\(\\),\\p{L}0-9]+)))?(\\.)?( (?<details>.+$))?";


        private static DieRoll GetDie(string text)
        {
            DieRoll dieroll;
            dieroll = Monster.FindNextDieRoll(text);

            if (dieroll == null)
            {
                dieroll = new DieRoll(int.Parse(text), 1, 0);
            }

            return dieroll;

        }

        private static string DieText(DieRoll roll)
        {
            if (roll.Die == 1)
            {
                return roll.Count.ToString();
            }
            else
            {
                return roll.Text;
            }
        }

        [XmlIgnore]
        public string Text
        {
            get
            {
                string text = "";

                text += Type;

                if (Cause != null)
                {
                    text += ", " + Cause;
                }

                text += "; ";

                text += "save " + SaveType + " DC " + Save + "; ";

                if (Immediate)
                {
                    text += "onset immediate; ";
                }
                else if (Onset != null)
                {
                    text += "onset " + DieText(Onset) + " " + OnsetUnit + ((Onset.Count == 1 && Onset.Die == 1) ? "" : "s") + "; ";
                }

                if (Once)
                {
                    text += "frequency once; ";
                }
                else
                {
                    text += "frequency " + Frequency + " " + FrequencyUnit + ((Frequency==1) ? "" : "s") + "; ";
                }

                text += "effect ";

                if (DamageDie != null)
                {
                    text += DieText(DamageDie) + " " + DamageType;

                    if (SecondaryDamageDie != null)
                    {

                        text += " " + DieText(DamageDie) + " " + DamageType;
                    }
                }
                else if (SpecialEffectTime != null)
                {
                    text += SpecialEffectName + " for " + DieText(SpecialEffectTime) + " " + SpecialEffectUnit;
                }
                else if (OtherEffect != null)
                {
                    text += OtherEffect;
                }

                if (DamageExtra != null)
                {
                    text += " " + DamageExtra;
                }

                text += "; ";

                text += "cure " + Cure;

                if (Details != null)
                {
                    text += ". " + Details;
                }

                return text;
            }
        }

        public override string ToString()
        {
            string text = "";

            if (Name != null)
            {
                text += Name + ": ";
            }

            text += Text;

            return text;
        }



        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Type")); }
                }
            }
        }

        public string Cause
        {
            get => _cause;
            set
            {
                if (_cause != value)
                {
                    _cause = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Cause")); }
                }
            }
        }
        public string SaveType
        {
            get => _saveType;
            set
            {
                if (_saveType != value)
                {
                    _saveType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SaveType")); }
                }
            }
        }
        public int Save
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Save")); }
                }
            }
        }
        public DieRoll Onset
        {
            get => _onset;
            set
            {
                if (_onset != value)
                {
                    _onset = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Onset")); }
                }
            }
        }
        public string OnsetUnit
        {
            get => _onsetUnit;
            set
            {
                if (_onsetUnit != value)
                {
                    _onsetUnit = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("OnsetUnit")); }
                }
            }
        }
        public bool Immediate
        {
            get => _immediate;
            set
            {
                if (_immediate != value)
                {
                    _immediate = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Immediate")); }
                }
            }
        }
        public int Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Frequency")); }
                }
            }
        }
        public string FrequencyUnit
        {
            get => _frequencyUnit;
            set
            {
                if (_frequencyUnit != value)
                {
                    _frequencyUnit = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("FrequencyUnit")); }
                }
            }
        }

        public int Limit
        {
            get => _limit;
            set
            {
                if (_limit != value)
                {
                    _limit = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Limit")); }
                }
            }
        }
        public string LimitUnit
        {
            get => _limitUnit;
            set
            {
                if (_limitUnit != value)
                {
                    _limitUnit = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LimitUnit")); }
                }
            }
        }

        public string SpecialEffectName
        {
            get => _specialEffectName;
            set
            {
                if (_specialEffectName != value)
                {
                    _specialEffectName = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SpecialEffectName")); }
                }
            }
        }

        public DieRoll SpecialEffectTime
        {
            get => _specialEffectTime;
            set
            {
                if (_specialEffectTime != value)
                {
                    _specialEffectTime = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SpecialEffectTime")); }
                }
            }
        }
        public string SpecialEffectUnit
        {
            get => _specialEffectUnit;
            set
            {
                if (_specialEffectUnit != value)
                {
                    _specialEffectUnit = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SpecialEffectUnit")); }
                }
            }
        }

        public string OtherEffect
        {
            get => _otherEffect;
            set
            {
                if (_otherEffect != value)
                {
                    _otherEffect = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("OtherEffect")); }
                }
            }
        }

        public bool Once
        {
            get => _once;
            set
            {
                if (_once != value)
                {
                    _once = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Once")); }
                }
            }
        }
        public DieRoll DamageDie
        {
            get => _damageDie;
            set
            {
                if (_damageDie != value)
                {
                    _damageDie = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DamageDie")); }
                }
            }
        }
        public string DamageType
        {
            get => _damageType;
            set
            {
                if (_damageType != value)
                {
                    _damageType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DamageType")); }
                }
            }
        }
        public bool IsDamageDrain
        {
            get => _isDamageDrain;
            set
            {
                if (_isDamageDrain != value)
                {
                    _isDamageDrain = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IsDamageDrain")); }
                }
            }
        }
        public DieRoll SecondaryDamageDie
        {
            get => _secondaryDamageDie;
            set
            {
                if (_secondaryDamageDie != value)
                {
                    _secondaryDamageDie = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SecondaryDamageDie")); }
                }
            }
        }
        public string SecondaryDamageType
        {
            get => _secondaryDamageType;
            set
            {
                if (_secondaryDamageType != value)
                {
                    _secondaryDamageType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SecondaryDamageType")); }
                }
            }
        }
        public bool IsSecondaryDamageDrain
        {
            get => _isSecondaryDamageDrain;
            set
            {
                if (_isSecondaryDamageDrain != value)
                {
                    _isSecondaryDamageDrain = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IsSecondaryDamageDrain")); }
                }
            }
        }
        public string DamageExtra
        {
            get => _damageExtra;
            set
            {
                if (_damageExtra != value)
                {
                    _damageExtra = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DamageExtra")); }
                }
            }
        }
        public string Cure
        {
            get => _cure;
            set
            {
                if (_cure != value)
                {
                    _cure = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Cure")); }
                }
            }
        }
        public string Details
        {
            get => _details;
            set
            {
                if (_details != value)
                {
                    _details = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Details")); }
                }
            }
        }
        public string Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Cost")); }
                }
            }
        }


    }
}
