/*
 *  WeaponSpecialAbility.cs
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
    public class WeaponSpecialAbility : INotifyPropertyChanged
    {



        public event PropertyChangedEventHandler PropertyChanged;

        private string _minor;
        private string _medium;
        private string _major;
        private string _rangedMinor;
        private string _rangedMedium;
        private string _rangedMajor;
        private string _name;
        private string _altName;
        private int _basePriceMod;
        private bool _melee;
        private bool _ranged;
        private string _plus;
        private string _text;

        private static List<WeaponSpecialAbility> _abilities;
        static WeaponSpecialAbility()
        {

            Load();

        }

        static void Load()
        {
            _abilities = XmlListLoader<WeaponSpecialAbility>.Load("WeaponSpecialAbility.xml");

        }

        public static List<WeaponSpecialAbility> SpecialAbilities => new(_abilities);

        public static List<WeaponSpecialAbility> RangedAbilities
        {
            get
            {
                List<WeaponSpecialAbility> list = new List<WeaponSpecialAbility>();

                foreach (WeaponSpecialAbility w in SpecialAbilities)
                {
                    if (w.Ranged)
                    {
                        list.Add(w);
                    }
                }

                return list;
            }
        }



        public static List<WeaponSpecialAbility> MeleeAbilities
        {
            get
            {
                List<WeaponSpecialAbility> list = new List<WeaponSpecialAbility>();

                foreach (WeaponSpecialAbility w in SpecialAbilities)
                {
                    if (w.Melee)
                    {
                        list.Add(w);
                    }
                }

                return list;
            }
        }


        public string Minor
        {
            get => _minor;
            set
            {
                if (_minor != value)
                {
                    _minor = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Minor")); }
                }
            }
        }
        public string Medium
        {
            get => _medium;
            set
            {
                if (_medium != value)
                {
                    _medium = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Medium")); }
                }
            }
        }
        public string Major
        {
            get => _major;
            set
            {
                if (_major != value)
                {
                    _major = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Major")); }
                }
            }
        }
        public string RangedMinor
        {
            get => _rangedMinor;
            set
            {
                if (_rangedMinor != value)
                {
                    _rangedMinor = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedMinor")); }
                }
            }
        }
        public string RangedMedium
        {
            get => _rangedMedium;
            set
            {
                if (_rangedMedium != value)
                {
                    _rangedMedium = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedMedium")); }
                }
            }
        }
        public string RangedMajor
        {
            get => _rangedMajor;
            set
            {
                if (_rangedMajor != value)
                {
                    _rangedMajor = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedMajor")); }
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
        public int BasePriceMod
        {
            get => _basePriceMod;
            set
            {
                if (_basePriceMod != value)
                {
                    _basePriceMod = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("BasePriceMod")); }
                }
            }
        }
        public bool Melee
        {
            get => _melee;
            set
            {
                if (_melee != value)
                {
                    _melee = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Melee")); }
                }
            }
        }
        public bool Ranged
        {
            get => _ranged;
            set
            {
                if (_ranged != value)
                {
                    _ranged = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Ranged")); }
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

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Text")); }
                }
            }
        }
    }
}
