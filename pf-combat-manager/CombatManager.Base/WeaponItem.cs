/*
 *  WeaponItem.cs
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

namespace CombatManager
{

    public class WeaponItemPlus : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;
        private DieRoll _roll;

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

        public DieRoll Roll
        {
            get => _roll;
            set
            {
                if (_roll != value)
                {
                    _roll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Roll")); }
                }
            }
        }
    }

    public class WeaponItem : INotifyPropertyChanged
    {

        

        public event PropertyChangedEventHandler PropertyChanged;

        private int _count;
        private Weapon _weapon;
        private bool _masterwork;
        private bool _broken;
        private int _magicBonus;
        private string _specialAbilities;
        private bool _mainHand;
		private bool _twoHanded;
        private string _plus;
        private bool _noMods;
        private DieStep _step;
        private Dictionary<string, WeaponItemPlus> _plusList;

        


        private SortedDictionary<string, string> _specialAbilitySet;



        public WeaponItem() 
        {
            _specialAbilitySet = new SortedDictionary<string, string>();
            
        }
        public WeaponItem(ViewModels.Attack attack)
        {

            _specialAbilitySet = new SortedDictionary<string, string>();
            Weapon = (Weapon)attack.Weapon.Clone();
            Count = attack.Count;
            MagicBonus = attack.MagicBonus;
            Masterwork = attack.Masterwork;
            Broken = attack.Broken;
            SpecialAbilities = attack.SpecialAbilities;
			TwoHanded = attack.TwoHanded;
            

            if (Weapon.Class == "Natural" )
            {

                Step = attack.Damage.Step;
                if (attack.Plus != null)
                {
                    Plus = attack.Plus;
                }
            }
            
        }


        public WeaponItem(Weapon weapon)
        {
            Weapon = (Weapon)weapon.Clone();
            _specialAbilitySet = new SortedDictionary<string, string>();
            Count = 1;
            MagicBonus = 0;
            Masterwork = false;
			TwoHanded = weapon.TwoHanded;

            if (Weapon.Class == "Natural")
            {
                Step = weapon.DamageDie.Step;
            }
        }

        public object Clone()
        {
            WeaponItem item = new WeaponItem();

            item.Weapon = (Weapon)Weapon.Clone();
            item.Count = Count;
            item.MagicBonus = MagicBonus;
            item.Masterwork = Masterwork;
            item.Broken = Broken;
            item.SpecialAbilities = SpecialAbilities;
            item.MainHand = MainHand;
            item.Plus = Plus;
            item.Step = Step;
            item.NoMods = NoMods;
			item.TwoHanded = TwoHanded;

            return item;
        }


        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
					OnPropertyChanged("Count");
                }
            }
        }
        public Weapon Weapon
        {
            get => _weapon;
            set
            {
                if (_weapon != value)
                {
                    _weapon = value;
					OnPropertyChanged("Weapon");
                }
            }
        }
        public bool Masterwork
        {
            get => _masterwork;
            set
            {
                if (_masterwork != value)
                {
                    _masterwork = value;
					OnPropertyChanged("Masterwork");
                }
            }
        }
        public bool Broken
        {
            get => _broken;
            set
            {
                if (_broken != value)
                {
                    _broken = value;
					OnPropertyChanged("Broken");
                }
            }
        }
        public int MagicBonus
        {
            get => _magicBonus;
            set
            {
                if (_magicBonus != value)
                {
                    _magicBonus = value;
					OnPropertyChanged("MagicBonus");
                }
            }
        }
        public bool MainHand
        {
            get => _mainHand;
            set
            {
                if (_mainHand != value)
                {
                    _mainHand = value;
					OnPropertyChanged("MainHand");
                }
            }
        }

		public bool TwoHanded
		{
			get => _twoHanded;
            set
			{
				if (_twoHanded != value)
				{
					_twoHanded = value;
					OnPropertyChanged("TwoHanded");
				}
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if(PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

        public string SpecialAbilities
        {
            get => _specialAbilities;
            set
            {
                if (_specialAbilities != value)
                {
                    _specialAbilities = value;

                    //update special ability set
                    _specialAbilitySet.Clear();
                    if (_specialAbilities != null)
                    {
						string text = _specialAbilities;
                        foreach (WeaponSpecialAbility w in WeaponSpecialAbility.SpecialAbilities)
                        {
                           	if (w.Name.Contains(" "))
							{

                                text = FindSpecial(text, w.Name);

                                if (w.AltName != null && w.AltName.Length > 0)
                                {

                                    text = FindSpecial(text, w.AltName);
                                }
	
							}
                        }
						
						 foreach (WeaponSpecialAbility w in WeaponSpecialAbility.SpecialAbilities)
                        {
                           	if (!w.Name.Contains(" "))
							{

                                text = FindSpecial(text, w.Name);

                                if (w.AltName != null && w.AltName.Length > 0)
                                {

                                    text = FindSpecial(text, w.AltName);
                                }
									
							}
                        }
                    }

					OnPropertyChanged("SpecialAbilities");
					OnPropertyChanged("SpecialAbilitySet");
                }
            }
        }

        private string FindSpecial(string text, string name)
        {

            Regex regSpec = new Regex("(^| )" + name + "( |$)", RegexOptions.IgnoreCase);

            return regSpec.Replace(text, delegate(Match m)
            {

                _specialAbilitySet[name] = name;
                return "";
            });
        }

        public string Plus
        {
            get => _plus;
            set
            {
                if (_plus != value)
                {
                    _plus = value;
					OnPropertyChanged("Plus");
                    if (_plusList != null)
                    {
                        _plusList = null;
                    }
                }
            }
        }

        public bool NoMods
        {
            get => _noMods;
            set
            {
                if (_noMods != value)
                {
                    _noMods = value;
                    OnPropertyChanged("NoMods");
                }
            }
        }


        public DieStep Step
        {
            get => _step;
            set
            {
                if (_step != value)
                {
                    _step = value;
					OnPropertyChanged("Step");
                }
            }
        }

        [XmlIgnore]
        public string PlusText
        {
            get
            {
                string text = Plus;



                if (SpecialAbilitySet != null)
                {
                    foreach (string ab in SpecialAbilitySet.Values)
                    {
                        WeaponSpecialAbility sp = WeaponSpecialAbility.SpecialAbilities.Find(delegate(WeaponSpecialAbility wp)
                        {
                            return string.Compare(wp.Name, ab) == 0;
                        }
                                    );

                        if (sp != null)
                        {
                            if (sp.Plus != null && sp.Plus.Length > 0)
                            {
                                text = ViewModels.Attack.AddPlus(text, sp.Plus);
                            }
                        }

                    }
                }

                if (Weapon.Special != null)
                {
                    if (Weapon.Special.Contains("grapple"))
                    {
                        text = ViewModels.Attack.AddPlus(text, "grab");
                    }
                    if (Weapon.Special.Contains("trip"))
                    {
                        text = ViewModels.Attack.AddPlus(text, "trip");
                    }
                    if (Weapon.Special.Contains("disarm"))
                    {
                        text = ViewModels.Attack.AddPlus(text, "disarm");
                    }
                }

                return text;
            }
        }

        public bool HasSpecialAbility(string name)
        {
            return SpecialAbilitySet.ContainsKey(name);
        }
            

        [XmlIgnore]
        public SortedDictionary<string, string> SpecialAbilitySet
        {
            get => _specialAbilitySet;
            set
            {
                _specialAbilitySet = value;

                string text = null;

                if (_specialAbilitySet != null)
                {
                    bool first = true;
                    foreach (string ab in _specialAbilitySet.Values)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            text += " ";
                        }

                        text += ab;
                    }
                }

                _specialAbilities = text;

				OnPropertyChanged("SpecialAbilitySet");
				OnPropertyChanged("SpecialAbilities");                
            }
        }

        [XmlIgnore]
        public string Name => Weapon.Name;


        [XmlIgnore]
        public string FullName
        {
            get
            {
                string text = "";
				
				if (Count != 1)
				{
					text += (Count.ToString()) + " ";
				}

                if (MagicBonus != 0)
                {
                    text += CmStringUtilities.PlusFormatNumber(MagicBonus) + " ";

                    if (SpecialAbilities != null && SpecialAbilities.Length > 0)
                    {
                        text += SpecialAbilities + " ";
                    }
                }
                else if (Masterwork)
                {
                    text += "mwk ";
                }
                else if (Broken)
                {
                    text += "broken ";
                }

                text += Weapon.Name + (Count != 1 ? "s" : "");


                if (Weapon.AltDamage)
                {
                    text += " " + Monster.StatText(Weapon.AltDamageStat) + " " + (Weapon.AltDamageDrain ? "drain" : "damage");
                }


				if (Plus != null && Plus.Length > 0)
				{
					text += " plus " + Plus;	
				}

                return text;

            }
        }



        public Dictionary<string, WeaponItemPlus> PlusList
        {
            get
            {
                if (_plusList == null)
                {
                    ParsePlusList();
                }
                return _plusList;
            }
        }

        private void ParsePlusList()
        {
            _plusList = new Dictionary<string,WeaponItemPlus>();
            if (_plus != null)
            {
                Regex regex = new Regex("((?<dieroll>([0-9]+)d([0-9]+)((\\+|-)[0-9]+)?) +)?(?<name>[-\\p{L}0-9 ]+?)(( +and +)|($))");

                foreach (Match m in regex.Matches(_plus))
                {
                    WeaponItemPlus p = new WeaponItemPlus();
                    p.Name = m.Groups["name"].Value;
                    if (m.Groups["dieroll"].Success)
                    {
                        p.Roll = Monster.FindNextDieRoll(m.Groups["dieroll"].Value);
                    }
                    _plusList[p.Name] = p;

                }
            }
        }


    }

    
}
