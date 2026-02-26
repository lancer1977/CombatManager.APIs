/*
 *  Treasure.cs
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
  

    public class Good : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private int _value;

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
        public int Value
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

    }

    public class Treasure : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int _level;
        private Coin _coin;
        private List<Good> _goods;
        private List<TreasureItem> _items;

        public Treasure()
        {
            _goods = new List<Good>();
            _items = new List<TreasureItem>();
        }

        public int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Level")); }
                }
            }
        }
        public Coin Coin
        {
            get => _coin;
            set
            {
                if (_coin != value)
                {
                    _coin = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Coin")); }
                }
            }
        }
        public List<Good> Goods
        {
            get => _goods;
            set
            {
                if (_goods != value)
                {
                    _goods = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Goods")); }
                }
            }
        }

        public List<TreasureItem> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Items")); }
                }
            }
        }

        public decimal TotalValue
        {
            get
            {
                decimal val = 0;

                if (_coin != null)
                {
                    val += _coin.GpValue;
                }

                foreach (Good good in _goods)
                {
                    val += (decimal)good.Value;
                }

                foreach (TreasureItem item in Items)
                {
                    val += (decimal)item.Value;
                }

                return val;
            }
        }

    }
}
