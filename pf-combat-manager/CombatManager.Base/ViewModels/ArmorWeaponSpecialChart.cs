/*
 *  ArmorWeaponSpecialChart.cs
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

namespace CombatManager.ViewModels
{
    public class ArmorWeaponSpecialChart : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _minor;
        private string _medium;
        private string _major;
        private string _name;
        private string _cost;
        private string _bonus;
        private string _type;

        
        private static List<ArmorWeaponSpecialChart> _chart;

        static ArmorWeaponSpecialChart()
        {
            _chart = XmlListLoader<ArmorWeaponSpecialChart>.Load("ArmorWeaponSpecialChart.xml");
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
        public string Bonus
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
        
        public string LevelWeight(ItemLevel level)
        {
            if (level == ItemLevel.Minor)
            {
                return _minor;
            }
            else if (level == ItemLevel.Medium)
            {
                return _medium;
            }
            else if (level == ItemLevel.Major)
            {
                return _major;
            }

            return null;

        }

        public static List<ArmorWeaponSpecialChart> Chart => _chart;
    }
}
