/*
 *  ConditionBonus.cs
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

namespace CombatManager
{
    public class ConditionBonus : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int? _str;
        private int? _dex;
        private int? _con;
        private int? _int;
        private int? _wis;
        private int? _cha;
        private int? _strSkill;
        private int? _dexSkill;
        private int? _conSkill;
        private int? _intSkill;
        private int? _wisSkill;
        private int? _chaSkill;
        private int? _dodge;
        private int? _armor;
        private int? _shield;
        private int? _naturalArmor;
        private int? _deflection;
        private int? _ac;
        private int? _initiative;
        private int? _allAttack;
        private int? _meleeAttack;
        private int? _rangedAttack;
        private int? _attackDamage;
        private int? _meleeDamage;
        private int? _rangedDamage;
        private int? _perception;
        private bool _loseDex;//*
        private int? _size;
        private int? _fort;
        private int? _ref;
        private int? _will;
        private int? _allSaves;
        private int? _allSkills; 
        private int? _cmb;
        private int? _cmd;
        private bool _strZero; 
        private bool _dexZero;
        private int? _hp;




        public ConditionBonus()
        {
        }

        public ConditionBonus(ConditionBonus bonus)
        {
            _str = bonus._str;
            _dex = bonus._dex;
            _con = bonus._con;
            _int = bonus._int;
            _wis = bonus._wis;
            _cha = bonus._cha;
            _strSkill = bonus._strSkill;
            _dexSkill = bonus._dexSkill;
            _conSkill = bonus._conSkill;
            _intSkill = bonus._intSkill;
            _wisSkill = bonus._wisSkill;
            _chaSkill = bonus._chaSkill;
            _dodge = bonus._dodge;
            _armor = bonus._armor;
            _shield = bonus._shield;
            _naturalArmor = bonus._naturalArmor;
			_deflection = bonus._deflection;
            _initiative = bonus._initiative;
            _allAttack = bonus._allAttack;
            _ac = bonus._ac;
            _meleeAttack = bonus._meleeAttack;
            _rangedAttack = bonus._rangedAttack;
            _attackDamage = bonus._attackDamage;
            _meleeDamage = bonus._meleeDamage;
            _rangedDamage = bonus._rangedDamage;
            _perception = bonus._perception;
            _loseDex = bonus._loseDex;
            _size = bonus._size;
            _fort = bonus._fort;
            _ref = bonus._ref;
            _will = bonus._will;
            _allSaves = bonus._allSaves;
            _allSkills = bonus._allSkills;
            _cmb = bonus.Cmb;
            _cmd = bonus.Cmd;
            _strZero = bonus._strZero;
            _dexZero = bonus._dexZero;
            _hp = bonus._hp;
        }


        public int? Str
        {
            get => _str;
            set
            {
                if (_str != value)
                {
                    _str = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Str")); }
                }
            }
        }
        public int? Dex
        {
            get => _dex;
            set
            {
                if (_dex != value)
                {
                    _dex = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Dex")); }
                }
            }
        }
        public int? Con
        {
            get => _con;
            set
            {
                if (_con != value)
                {
                    _con = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Con")); }
                }
            }
        }
        public int? Int
        {
            get => _int;
            set
            {
                if (_int != value)
                {
                    _int = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Int")); }
                }
            }
        }
        public int? Wis
        {
            get => _wis;
            set
            {
                if (_wis != value)
                {
                    _wis = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Wis")); }
                }
            }
        }
        public int? Cha
        {
            get => _cha;
            set
            {
                if (_cha != value)
                {
                    _cha = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Cha")); }
                }
            }
        }
        public int? StrSkill
        {
            get => _strSkill;
            set
            {
                if (_strSkill != value)
                {
                    _strSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("StrSkill")); }
                }
            }
        }
        public int? DexSkill
        {
            get => _dexSkill;
            set
            {
                if (_dexSkill != value)
                {
                    _dexSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DexSkill")); }
                }
            }
        }
        public int? ConSkill
        {
            get => _conSkill;
            set
            {
                if (_conSkill != value)
                {
                    _conSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ConSkill")); }
                }
            }
        }
        public int? IntSkill
        {
            get => _intSkill;
            set
            {
                if (_intSkill != value)
                {
                    _intSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IntSkill")); }
                }
            }
        }
        public int? WisSkill
        {
            get => _wisSkill;
            set
            {
                if (_wisSkill != value)
                {
                    _wisSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("WisSkill")); }
                }
            }
        }
        public int? ChaSkill
        {
            get => _chaSkill;
            set
            {
                if (_chaSkill != value)
                {
                    _chaSkill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ChaSkill")); }
                }
            }
        }

        public int? Dodge
        {
            get => _dodge;
            set
            {
                if (_dodge != value)
                {
                    _dodge = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Dodge")); }
                }
            }
        }
        public int? Armor
        {
            get => _armor;
            set
            {
                if (_armor != value)
                {
                    _armor = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Armor")); }
                }
            }
        }
        public int? Shield
        {
            get => _shield;
            set
            {
                if (_shield != value)
                {
                    _shield = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Shield")); }
                }
            }
        }
        public int? NaturalArmor
        {
            get => _naturalArmor;
            set
            {
                if (_naturalArmor != value)
                {
                    _naturalArmor = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NaturalArmor")); }
                }
            }
        }
        public int? Deflection
        {
            get => _deflection;
            set
            {
                if (_deflection != value)
                {
                    _deflection = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Deflection")); }
                }
            }
        }
        public int? Ac
        {
            get => _ac;
            set
            {
                if (_ac != value)
                {
                    _ac = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AC")); }
                }
            }
        }
        public int? Initiative
        {
            get => _initiative;
            set
            {
                if (_initiative != value)
                {
                    _initiative = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Initiative")); }
                }
            }
        }
        public int? AllAttack
        {
            get => _allAttack;
            set
            {
                if (_allAttack != value)
                {
                    _allAttack = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AllAttack")); }
                }
            }
        }
        public int? MeleeAttack
        {
            get => _meleeAttack;
            set
            {
                if (_meleeAttack != value)
                {
                    _meleeAttack = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MeleeAttack")); }
                }
            }
        }
        public int? RangedAttack
        {
            get => _rangedAttack;
            set
            {
                if (_rangedAttack != value)
                {
                    _rangedAttack = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedAttack")); }
                }
            }
        }
        public int? AttackDamage
        {
            get => _attackDamage;
            set
            {
                if (_attackDamage != value)
                {
                    _attackDamage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AttackDamage")); }
                }
            }
        }

        public int? MeleeDamage
        {
            get => _meleeDamage;
            set
            {
                if (_meleeDamage != value)
                {
                    _meleeDamage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MeleeDamage")); }
                }
            }
        }
        public int? RangedDamage
        {
            get => _rangedDamage;
            set
            {
                if (_rangedDamage != value)
                {
                    _rangedDamage = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedDamage")); }
                }
            }
        }


        public int? Perception
        {
            get => _perception;
            set
            {
                if (_perception != value)
                {
                    _perception = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Perception")); }
                }
            }
        }
        public bool LoseDex
        {
            get => _loseDex;
            set
            {
                if (_loseDex != value)
                {
                    _loseDex = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LoseDex")); }
                }
            }
        }
        public int? Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Size")); }
                }
            }
        }
        public int? Fort
        {
            get => _fort;
            set
            {
                if (_fort != value)
                {
                    _fort = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Fort")); }
                }
            }
        }
        public int? Ref
        {
            get => _ref;
            set
            {
                if (_ref != value)
                {
                    _ref = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Ref")); }
                }
            }
        }
        public int? Will
        {
            get => _will;
            set
            {
                if (_will != value)
                {
                    _will = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Will")); }
                }
            }
        }


        public int? AllSaves
        {
            get => _allSaves;
            set
            {
                if (_allSaves != value)
                {
                    _allSaves = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AllSaves")); }
                }
            }
        }
        public int? AllSkills
        {
            get => _allSkills;
            set
            {
                if (_allSkills != value)
                {
                    _allSkills = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AllSkills")); }
                }
            }
        }



        public int? Cmb
        {
            get => _cmb;
            set
            {
                if (_cmb != value)
                {
                    _cmb = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CMB")); }
                }
            }
        }
        public int? Cmd
        {
            get => _cmd;
            set
            {
                if (_cmd != value)
                {
                    _cmd = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CMD")); }
                }
            }
        }

        public bool StrZero
        {
            get => _strZero;
            set
            {
                if (_strZero != value)
                {
                    _strZero = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("StrZero")); }
                }
            }
        }
        public bool DexZero
        {
            get => _dexZero;
            set
            {
                if (_dexZero != value)
                {
                    _dexZero = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DexZero")); }
                }
            }
        }

        public int? Hp
        {
            get => _hp;
            set
            {
                if (_hp != value)
                {
                    _hp = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HP"));
                }
            }
        }

        public object Clone()
        {
            return new ConditionBonus(this);
        }

    }
}
