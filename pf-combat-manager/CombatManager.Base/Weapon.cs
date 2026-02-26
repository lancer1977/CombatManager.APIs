/*
 *  Weapon.cs
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

using System.IO;

namespace CombatManager
{
    public class Weapon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _altName;
        private string _originalName;
        private string _cost;
        private string _dmgS;
        private string _dmgM;
        private string _critical;
        private string _range;
        private string _weight;
        private string _dmgType;
        private string _special;
        private string _source;
        private string _hands;
        private string _class;
		private string _plural;
        private List<string> _groups;
        private bool _isHand;
        private bool _throw;
        private int _critRange;
        private int _critMultiplier;
        private bool _rangedTouch;
        private bool _altDamage;
        private Stat _altDamageStat;
        private bool _altDamageDrain;
        private string _url;
        private string _desc;
        private string _misfire;
        private string _capacity;

        
        static Dictionary<string, Weapon> _weapons;
        static Dictionary<string, Weapon> _weaponsPlural;
        static Dictionary<string, Weapon> _weaponsOriginalName;	
        static Dictionary<string, Weapon> _weaponsAltName;	

        static Weapon()
        {
            LoadWeapons();		
        }
        public Weapon()
        {
            _critRange = 20;
            _critMultiplier = 2;
        }
        //create a blank weapon from an attack
        public Weapon(ViewModels.Attack attack, bool ranged, MonsterSize size)
        {
            

            _name = attack.Name;

            DieRoll medRoll = DieRoll.StepDie(attack.Damage, ((int)MonsterSize.Medium) - (int)size);
            DieRoll smRoll = DieRoll.StepDie(attack.Damage, -1);

            _dmgM = new DieStep(medRoll).ToString();
            _dmgS = new DieStep(smRoll).ToString();

            _critRange = attack.CritRange;
            _critMultiplier = attack.CritMultiplier;

            if (attack.Bonus.Count > 1)
            {
                _class = "Martial";
            }
            else
            {
                _class = "Natural";
            }

            if (ranged)
            {
                _hands = "Ranged";
            }
            else
            {
                _hands = "One-Handed";
            }

            _rangedTouch = attack.RangedTouch;
            _altDamage = attack.AltDamage;
            _altDamageStat = attack.AltDamageStat;
            _altDamageDrain = attack.AltDamageDrain;          
        }
        public object Clone()
        {
            Weapon weapon = new Weapon();
            weapon._name = _name;
            weapon._cost = _cost;
            weapon._dmgS = _dmgS;
            weapon._dmgM = _dmgM;
            weapon._critical = _critical;
            weapon._range = _range;
            weapon._weight = _weight;
            weapon._dmgType = _dmgType;
            weapon._special = _special;
            weapon._source = _source;
            weapon._hands = _hands;
            weapon._class = _class;
            weapon._plural = _plural;
            weapon._isHand = _isHand;
            weapon._throw = _throw;
            weapon._critRange = _critRange;
            weapon._critMultiplier = _critMultiplier;
            weapon._rangedTouch = _rangedTouch;
            weapon._altDamage = _altDamage;
            weapon._altDamageStat = _altDamageStat;
            weapon._altDamageDrain = _altDamageDrain;
            weapon._misfire = _misfire;
            weapon._capacity = _capacity;
            if (_groups != null)
            {
                weapon._groups = new List<string>(_groups);
            }


            return weapon;
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
        public string AltName
        {
            get => _altName;
            set
            {
                if (_altName != value)
                {
                    _altName = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AltName")); }
                }
            }
        }
        [XmlIgnore]
        public string OriginalName
        {
            get => _originalName;
            set
            {
                if (_originalName != value)
                {
                    _originalName = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("OriginalName")); }
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
        public string DmgS
        {
            get => _dmgS;
            set
            {
                if (_dmgS != value)
                {
                    _dmgS = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DmgS")); }
                }
            }
        }
        public string DmgM
        {
            get => _dmgM;
            set
            {
                if (_dmgM != value)
                {
                    _dmgM = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DmgM")); }
                }
            }
 
        }		
		[XmlIgnore]
		public DieRoll DamageDie => DieRoll.FromString(DmgM, 0);

        [XmlIgnore]
        public string DamageText
        {
            get
            {
                if (DamageDie == null)
                {
                    return "";
                }
                else
                {
                    string critText = "";

                    if (CritRange != 20)
                    {
                        critText += "/" + CritRange + "-20";
                    }

                    if (CritMultiplier != 2)
                    {
                        critText += "/x" + CritMultiplier;
                    }



                    return DamageDie.Text + critText;
                }
            }
        }
        public DieRoll SizeDamageDie(MonsterSize size)
        {
            if (DamageDie == null)
            {
                return null;
            }

            return DieRoll.StepDie(DamageDie, (size - MonsterSize.Medium));
        }
        public string SizeDamageText(MonsterSize size)
        {
            DieRoll roll = SizeDamageDie(size);

            if (roll == null)
            {
                return "";
            }

            string critText = "";

            if (CritRange != 20)
            {
                critText += "/" + CritRange + "-20";
            }

            if (CritMultiplier != 2)
            {
                critText += "/x" + CritMultiplier;
            }

            return roll.Text + critText;
        }	
        private const string RegCritString = "((?<critrange>[0-9]+)-[0-9]+)?(/?x(?<critmultiplier>[0-9]+))?";
        public string Critical
        {
            get => _critical;
            set
            {
                if (_critical != value)
                {
                    _critical = value;                  
                    _critRange = 20;
                    _critMultiplier = 2;
                    if (_critical != null)
                    {
                        Match m = new Regex(RegCritString).Match(_critical);

                        if (m.Groups["critrange"].Success)
                        {
                            _critRange = int.Parse(m.Groups["critrange"].Value);
                        }
                        if (m.Groups["critmultiplier"].Success)
                        {
                            _critMultiplier = int.Parse(m.Groups["critmultiplier"].Value);
                        }
                        
                    }

                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Critical")); }
                }
            }
        }
        public string Range
        {
            get => _range;
            set
            {
                if (_range != value)
                {
                    _range = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Range")); }
                }
            }
        }
        public string Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Weight")); }
                }
            }
        }
        public string DmgType
        {
            get => _dmgType;
            set
            {
                if (_dmgType != value)
                {
                    _dmgType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DmgType")); }
                }
            }
        }
        public string Special
        {
            get => _special;
            set
            {
                if (_special != value)
                {
                    _special = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Special")); }
                }
            }
        }
        public string Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Source")); }
                }
            }
        }
        public string Hands
        {
            get => _hands;
            set
            {
                if (_hands != value)
                {
                    _hands = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Hands")); }
                }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Class")); }
                }
            }
        }
        public string Plural
        {
            get => _plural;
            set
            {
                if (_plural != value)
                {
                    _plural = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Plural")); }
                }
            }
        }
        public bool IsHand
        {
            get => _isHand;
            set
            {
                if (_isHand != value)
                {
                    _isHand = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IsHand")); }
                }
            }
        }
        public bool Throw
        {
            get => _throw;
            set
            {
                if (_throw != value)
                {
                    _throw = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Throw")); }
                }
            }
        }
        public bool RangedTouch
        {
            get => _rangedTouch;
            set
            {
                if (_rangedTouch != value)
                {
                    _rangedTouch = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedTouch")); }
                }
            }
        }
        public bool AltDamage
        {
            get => _altDamage;
            set
            {
                if (_altDamage != value)
                {
                    _altDamage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AltDamage")); }
                }
            }
        }
        public Stat AltDamageStat
        {
            get => _altDamageStat;
            set
            {
                if (_altDamageStat != value)
                {
                    _altDamageStat = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AltDamageStat")); }
                }
            }
        }
        public bool AltDamageDrain
        {
            get => _altDamageDrain;
            set
            {
                if (_altDamageDrain != value)
                {
                    _altDamageDrain = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AltDamageDrain")); }
                }
            }
        }
        public string Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("URL")); }
                }
            }
        }
        public string Desc
        {
            get => _desc;
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Desc")); }
                }
            }
        }        
        public string Misfire
        {
            get => _misfire;
            set
            {
                if (_misfire != value)
                {
                    _misfire = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Misfire")); }
                }
            }
        }
        public string Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Capacity")); }
                }
            }
        }
        [XmlArrayItem("Group")]
        public List<string> Groups
        {
            get => _groups;
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Groups")); }
                }

            }
        }
        [XmlIgnore]
        public int CritRange => _critRange;

        [XmlIgnore]
        public int CritMultiplier => _critMultiplier;

        [XmlIgnore]
        public int HandsUsed
        {
            get
            {
                if (Class != "Natural")
                {
                    return (Hands == "Two-Handed" || Hands == "Double" || Hands == "Two-Handed Firearm") ? 2 : 1;
                }
                else
                {
                    return IsHand ? 1 : 0;
                }

            }
        }			
		[XmlIgnore]
        public bool TwoHanded => HandsUsed == 2;

        [XmlIgnore]
        public bool Natural => Class == "Natural";

        [XmlIgnore]
        public bool Ranged => Hands == "Ranged" || Hands == "One-Handed Firearm" || Hands == "Two-Handed Firearm";

        [XmlIgnore]
        public bool Firearm => Hands == "One-Handed Firearm" || Hands == "Two-Handed Firearm";

        [XmlIgnore]
        public bool Double => Hands == "Double";

        [XmlIgnore]
        public bool Light => Hands == "Light" || Hands == "Unarmed" || Hands == "One-Handed Firearm";

        [XmlIgnore]
        public bool WeaponFinesse
        {
            get
            {
                if (Light || Natural)
                {
                    return true;
                }
                else if (_special != null)
                {
                    return Regex.Match(_special, "Weapon Finesse", RegexOptions.IgnoreCase).Success;
                }
                else
                {
                    return false;
                }
            }
        }
        public static Dictionary<string, Weapon> Weapons => _weapons;

        public static Dictionary<string, Weapon> WeaponsPlural => _weaponsPlural;

        public static Dictionary<string, Weapon> WeaponsOriginalName => _weaponsPlural;

        public static Dictionary<string, Weapon> WeaponsAltName => _weaponsAltName;

        public static Weapon Find(string name)
        {
            if (_weapons.ContainsKey(name))
            {
                return _weapons[name];
            }
            else if (_weaponsPlural.ContainsKey(name))
            {
                return _weaponsPlural[name];
            }
            else if (_weaponsOriginalName.ContainsKey(name))
            {
                return _weaponsOriginalName[name];
            }
            else if (_weaponsAltName.ContainsKey(name))
            {
                return _weaponsAltName[name];
            }

            return null;
        }
        static void LoadWeapons()
        {
            FileStream fs = null;
            try
            {

                List<Weapon> set = XmlListLoader<Weapon>.Load("Weapons.xml");


                _weapons = new Dictionary<string, Weapon>(new InsensitiveEqualityCompararer());
                _weaponsPlural = new Dictionary<string, Weapon>(new InsensitiveEqualityCompararer());
                _weaponsOriginalName = new Dictionary<string, Weapon>(new InsensitiveEqualityCompararer());
                _weaponsAltName = new Dictionary<string, Weapon>(new InsensitiveEqualityCompararer());

                foreach (Weapon weapon in set)
                {
                    Regex reg = new Regex("([-\\. \\p{L}]+), ([-\\. \\p{L}]+)");

                    Match m = reg.Match(weapon.Name);

                    if (m.Success)
                    {
                        weapon.OriginalName = weapon.Name;
                        weapon.Name = m.Groups[2].Value + " " + m.Groups[1].Value;
                        _weaponsOriginalName.Add(weapon.OriginalName, weapon);
                    }

                    _weapons[weapon.Name] = weapon;

                    if (weapon.Plural != null && weapon.Plural.Length > 0)
                    {
                        _weaponsPlural[weapon.Plural] = weapon;
                    }

                    if (weapon.AltName != null && weapon.AltName.Length > 0)
                    {
                        _weaponsAltName[weapon.AltName] = weapon;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        public static string ReplaceOriginalWeaponNames(string text, bool uppercase)
        {
            string returnText = text;

            foreach (Weapon weapon in Weapons.Values)
            {
                if (weapon.OriginalName != null)
                {
                    string name = weapon.Name;

                    if (uppercase)
                    {
                        name = StringCapitalizer.Capitalize(name);
                    }
                    else
                    {
                        name = name.ToLower();
                    }

                    returnText = new Regex(Regex.Escape(weapon.OriginalName), RegexOptions.IgnoreCase).Replace(returnText, name);
                }
            }

            return returnText;
        }

    }
}
