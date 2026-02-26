/*
 *  Attack.cs
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

using System.Text;
using CombatManager.Utilities;

namespace CombatManager.ViewModels
{
    public class Attack : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;



        private int _count;
        private string _name;
        private List<int> _bonus;
        private DieRoll _damage;
        private DieRoll _offHandDamage;
        private string _plus;
        private int _critRange;
        private int _critMultiplier;
        private Weapon _baseWeapon;
        private bool _masterwork;
        private bool _broken;
        private int _magicBonus;
        private string _specialAbilities;
        private bool _rangedTouch;
        private bool _altDamage;
        private Stat _altDamageStat;
        private bool _altDamageDrain;
		private bool _twoHanded;

        private static string _specialAbilityString;



        public Attack()
        {
        }


        public Attack(int count, string name, int bonus, DieRoll damage, string plus)
        {
            Count = count;
            Name = name;
            Bonus = new List<int>();
            Bonus.Add(bonus);
            Damage = damage;
            Plus = plus;
            CritRange = 20;
            CritMultiplier = 2;
        }

        public Attack(int count, string name, DieRoll damage, string plus)
        {
            Count = count;
            Name = name;
            Bonus = new List<int>();
            Damage = damage;
            Plus = plus;
            CritRange = 20;
            CritMultiplier = 2;
        }


        public object Clone()
        {
            Attack atk = new Attack();

            atk._count = _count;
            atk._name = _name;
            atk._bonus = new List<int>(_bonus);
            atk._damage = _damage;
            atk._offHandDamage = _offHandDamage;
            atk._plus = _plus;
            atk._critRange = _critRange;
            atk._critMultiplier = _critMultiplier;
            atk._baseWeapon = _baseWeapon;
            atk._masterwork = _masterwork;
            atk._broken = _broken;
            atk._magicBonus = _magicBonus;
            atk._specialAbilities = _specialAbilities;
            atk._rangedTouch = _rangedTouch;
            atk._altDamage = _altDamage;
            atk._altDamageStat = _altDamageStat;
            atk._altDamageDrain = _altDamageDrain;
			atk._twoHanded = _twoHanded;

            return atk;
        }
        public static string AddPlus(string plus, string newPlus)
        {
            string text = plus;

            if (text == null || text.Trim().Length == 0)
            {
                text = newPlus;
            }
            else
            {
                text = text.Trim();

                Regex reg = new Regex(Regex.Escape(newPlus), RegexOptions.IgnoreCase);

                if (!(reg.Match(text).Success))
                {
                    text += " and " + newPlus;
                }
            }
            return text;
        }
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Count")); }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
                }
            }
        }
        public List<int> Bonus
        {
            get => _bonus;
            set
            {
                if (_bonus != value)
                {
                    _bonus = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Bonus")); }
                }
            }
        }
        public DieRoll Damage
        {
            get => _damage;
            set
            {
                if (_damage != value)
                {
                    _damage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Damage")); }
                }
            }
        }
        public DieRoll OffHandDamage
        {
            get => _offHandDamage;
            set
            {
                if (_offHandDamage != value)
                {
                    _offHandDamage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("OffHandDamage")); }
                }
            }
        }
        public string Plus
        {
            get => _plus;
            set
            {
                if (_plus != value)
                {
                    _plus = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Plus")); }
                }
            }
        }
        public int CritRange
        {
            get => _critRange;
            set
            {
                if (_critRange != value)
                {
                    _critRange = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CritRange")); }
                }
            }
        }
        public int CritMultiplier
        {
            get => _critMultiplier;
            set
            {
                if (_critMultiplier != value)
                {
                    _critMultiplier = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CritMultiplier")); }
                }
            }
        }
        public Weapon Weapon
        {
            get => _baseWeapon;
            set
            {
                if (_baseWeapon != value)
                {
                    _baseWeapon = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("BaseWeapon")); }

					this.TwoHanded = _baseWeapon.TwoHanded;
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Masterwork")); }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Broken")); }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MagicBonus")); }
                }
            }
        }
        public string SpecialAbilities
        {
            get => _specialAbilities;
            set
            {
                if (_specialAbilities != value)
                {
                    _specialAbilities = value;
					
                    if (PropertyChanged != null) 
					{ 
						PropertyChanged(this, new PropertyChangedEventArgs("SpecialAbilities")); 	
					}
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
		public bool TwoHanded
		{
			get => _twoHanded;
            set
			{
				if (_twoHanded != value)
				{
					_twoHanded = value;
					if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("TwoHanded")); }
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
        [XmlIgnore]
        public string Text => AttackText(this);

        public override string ToString()
        {
            return Text;
        }
        public static string DieRegexString => "([0-9/]+)(d)([0-9]+)((\\+|-)[0-9]+)?";

        public static string RegexString(string attackName)
        {
            string text =
            "((and )|(or ))?(?<count>[0-9]+( )+)?((?<magicbonus>(\\+|-)[0-9]+) +(?<specialabilities>((" + SpecialAbilityString + ") )*)?)?(?<broken>broken )?(?<mwk>((mwk)|(masterwork)) )?(?<name>";

            if (attackName == null)
            {
                text += "[\\p{L}][- \\p{L}]*)(s?";
            }

            else
            {
                text += Regex.Escape(attackName) + ")(s?";
            }

            text += ")( )((?<incorrectmagicbonus>\\+[0-9]+ +)?(\\([- \\p{L}]+\\) )?(?<bonus>[-\\+0-9/]+))( melee)?(?<rangedtouch> ranged touch)?( +\\()(?<damage>" + DieRegexString + ")(/(?<offhanddamage>" + DieRegexString + "))?(?<critrange>/[0-9]+-[0-9]+)?(?<critmultiplier>/x[0-9]+)?(?<altdamage> (?<altdamagestat>(Strength)|(Dexterity)|(Constitution)|(Intelligence)|(Wisdom)|(Charisma)) (?<altdamagetype>(damage)|(drain)))?(, (?<savetype>((Fort)|(Ref)|(Will)))\\. DC (?<saveval>[0-9]+) (?<saveresult>((half)|(negates))) ?)?(?<allplus> (plus|and) (?<plus>[- \\p{L}0-9\\.\\+;]+))?(\\))";

            return text;
        }
        private static string SpecialAbilityString
        {
            get
            {
                if (_specialAbilityString == null)
                {
                    StringBuilder bld = new StringBuilder();

                    bool first = true;

                    foreach (WeaponSpecialAbility w in WeaponSpecialAbility.SpecialAbilities)
                    {
                        bld.Append(first ? w.Name : ("|" + w.Name));

                        if (w.AltName != null && w.AltName.Length > 0)
                        {

                            bld.Append("|" + w.AltName);
                        }

                        first = false;
                    }

                    _specialAbilityString = bld.ToString();
                }

                return _specialAbilityString;
            }
        }
        public static Attack ParseAttack(Match m)
        {
            Attack info = new Attack();

            if (m.Groups["count"].Success)
            {
                info.Count = int.Parse(m.Groups["count"].Value);
            }
            else
            {
                info.Count = 1;
            }

            info.Name = m.Groups["name"].Value.Trim() ;

            if (info.Count > 1 && info.Name[info.Name.Length - 1] == 's')
            {
                info.Name = info.Name.Substring(0, info.Name.Length - 1);
            }


            info.Bonus = new List<int>();

            Regex bonus = new Regex("((-|\\+)[0-9]+)(/)?");

            foreach (Match b in bonus.Matches(m.Groups["bonus"].Value))
            {
                info.Bonus.Add(int.Parse(b.Groups[1].Value));
            };


            if (m.Groups["rangedtouch"].Success)
            {
                info.RangedTouch = true;
            }


            info.Damage = Monster.FindNextDieRoll(m.Groups["damage"].Value, 0);

			if (m.Groups["offhanddamage"].Success)
            {
                info.OffHandDamage = Monster.FindNextDieRoll(m.Groups["offhanddamage"].Value, 0);
            }

            if (m.Groups["altdamage"].Success)
            {
                info.AltDamage = true;
                info.AltDamageStat = Monster.StatFromName(m.Groups["altdamagestat"].Value);
                info.AltDamageDrain = (string.Compare(m.Groups["altdamagetype"].Value, "drain", true) == 0);
            }

            info.Plus = null;

            if (m.Groups["critrange"].Success)
            {
                info.CritRange = FindNextNum(m.Groups["critrange"].Value);
            }
            else
            {
                info.CritRange = 20;
            }

            if (m.Groups["critmultiplier"].Success)
            {
                info.CritMultiplier = FindNextNum(m.Groups["critmultiplier"].Value);
            }
            else
            {
                info.CritMultiplier = 2;
            }

            info.Masterwork = m.Groups["mwk"].Success;
            info.Broken = m.Groups["broken"].Success;

            if (m.Groups["magicbonus"].Success)
            {
                info.MagicBonus = int.Parse(m.Groups["magicbonus"].Value);

                if (m.Groups["specialabilities"].Success)
                {
                    info.SpecialAbilities = m.Groups["specialabilities"].Value.Trim();
                }
            
            }
            else if (m.Groups["incorrectmagicbonus"].Success)
            {
                info.MagicBonus = int.Parse(m.Groups["incorrectmagicbonus"].Value);                
            }

            if (m.Groups["allplus"].Success)
            {
                info.Plus = m.Groups["plus"].Value;
            }

            if (Weapon.Weapons.ContainsKey(info.Name))
            {
                info.Weapon = Weapon.Weapons[info.Name];
            }
            else if (Weapon.WeaponsAltName.ContainsKey(info.Name))
            {
                info.Weapon = Weapon.WeaponsAltName[info.Name];
            }
            else if (info.Name[info.Name.Length - 1] == 's' && Weapon.Find(info.Name.Substring(0, info.Name.Length - 1)) != null)
            {
                info.Weapon = Weapon.Find(info.Name.Substring(0, info.Name.Length - 1));
            }
            else if (Weapon.WeaponsPlural.ContainsKey(info.Name + "s"))
            {
                info.Weapon = Weapon.WeaponsPlural[info.Name + "s"];
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Unknown weapon: \"" + info.Name + "\"");                
            }


            return info;
        }
        private static int FindNextNum(string text)
        {
            int val = 0;

            Regex regEx = new Regex("-?[0-9]+");

            Match m = regEx.Match(text);

            if (m.Success)
            {
                val = int.Parse(m.Value);
            }

            return val;

        }
        private static string AttackText(Attack info)
        {
            string text = "";

            if (info.Count != 1)
            {
                text += (info.Count.ToString()) + " ";
            }

            if (info.MagicBonus != 0)
            {
                text += info.MagicBonus.PlusFormat() +" ";

                if (info.SpecialAbilities != null && info.SpecialAbilities.Length > 0)
                {
                    text += info.SpecialAbilities + " ";
                }
            }
            else if (info.Masterwork)
            {
                text += "mwk ";
            }
            else if (info.Broken)
            {
                text += "broken ";
            }

            text += info.Name + (info.Count != 1 ? "s" : "");

            if (info.RangedTouch)
            {
                text += " ranged touch";
            }

            text += " " + AttackBonusText(info.Bonus) + " (";

            text += Monster.DieRollText(info.Damage);

            if (info.OffHandDamage != null)
            {
                text += "/" + Monster.DieRollText(info.OffHandDamage);
            }

            if (info.CritRange != 20)
            {
                text += "/" + info.CritRange + "-20";
            }
            if (info.CritMultiplier != 2)
            {
                text += "/x" + info.CritMultiplier;
            }

            if (info.AltDamage)
            {
                text += " " + Monster.StatText(info.AltDamageStat) + " " + (info.AltDamageDrain ? "drain" : "damage");
            }

            if (info.Plus != null && info.Plus.Length > 0)
            {
                text += " plus " + info.Plus;
            }


            text += ")";


            return text;
        }
        private static string AttackBonusText(List<int> bonus)
        {
            string text = "";

            for (int i = 0; i < bonus.Count; i++)
            {
                if (i > 0)
                {
                    text += "/";

                }
                text += bonus[i].PlusFormat();
            }

            return text;
        }		
		[XmlIgnore]		
		private string FullName
		{
			get
			{
				string text = "";
	
				if (MagicBonus != 0)
				{
					text += MagicBonus.PlusFormat() + " ";
	
					if (SpecialAbilities != null && SpecialAbilities.Length > 0)
					{
						text += SpecialAbilities + " ";
					}
				}
				else if (Masterwork)
				{
					text += "mwk ";
				}
	
				text += Name;
				
				return text;
			}
		}
    }
}
