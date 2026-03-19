/*
 *  SpellLevelInfo.cs
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
    public class SpellLevelInfo : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int? _level;
        private int? _perDay;
        private bool _atWill;
        private int? _more;
        private int? _cast; 
        private bool _constant;


        private ObservableCollection<SpellInfo> _spells = new ObservableCollection<SpellInfo>();

        public SpellLevelInfo() { }

        public SpellLevelInfo (SpellLevelInfo old)
        {
            _level = old._level;
            _perDay = old._perDay;
            _atWill = old._atWill;
            _more = old._more;
            _cast = old._cast;
            _constant = old._constant;

            foreach (SpellInfo info in old.Spells)
            {
                _spells.Add(new SpellInfo(info));
            }
        }

        public object Clone()
        {
            return new SpellLevelInfo(this);
        }

        public int? Level
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
        public int? PerDay
        {
            get => _perDay;
            set
            {
                if (_perDay != value)
                {
                    _perDay = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PerDay")); }
                }
            }
        }
        public bool AtWill
        {
            get => _atWill;
            set
            {
                if (_atWill != value)
                {
                    _atWill = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AtWill")); }
                }
            }
        }
        public int? More
        {
            get => _more;
            set
            {
                if (_more != value)
                {
                    _more = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("More")); }
                }
            }
        }
        public int? Cast
        {
            get => _cast;
            set
            {
                if (_cast != value)
                {
                    _cast = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Cast")); }
                }
            }
        }


        public bool Constant
        {
            get => _constant;
            set
            {
                if (_constant != value)
                {
                    _constant = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Constant")); }
                }
            }
        }

        public ObservableCollection<SpellInfo> Spells
        {
            get => _spells;
            set
            {
                if (_spells != value)
                {
                    _spells = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Spells")); }
                }
            }
        }



    }
}
