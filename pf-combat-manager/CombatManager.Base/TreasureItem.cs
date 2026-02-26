/*
 *  TreasureItem.cs
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
    public class TreasureItem : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private decimal _value;
        private Spell _spell;
        private MagicItem _magicItem;
        private string _type;
        private Equipment _equipment;
 

        public TreasureItem()
        {


        }

        public TreasureItem(TreasureItem t)
        {
            _name = t._name;
            _value = t._value;
            if (t.Spell != null)
            {
                _spell = t.Spell;
            }
            if (t.MagicItem != null)
            {
                _magicItem = t.MagicItem ;
            }
        }

        public object Clone()
        {
            return new TreasureItem(this);
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
        public decimal Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
                }
            }
        }
        public Spell Spell
        {
            get => _spell;
            set
            {
                if (_spell != value)
                {
                    _spell = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Spell")); }
                }
            }
        }
        public MagicItem MagicItem
        {
            get => _magicItem;
            set
            {
                if (_magicItem != value)
                {
                    _magicItem = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MagicItem")); }
                }
            }
        }

        public Equipment Equipment
        {
            get => _equipment;
            set
            {
                if (_equipment != value)
                {
                    _equipment = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Equipment")); }
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

    }
}
