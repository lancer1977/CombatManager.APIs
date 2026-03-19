/*
 *  SpellInfo.cs
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
    public class SpellInfo : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private int? _dc;
        private Spell _spell;
        private int? _count;
        private int? _cast;
        private bool _alreadyCast;
        private string _only;
        private string _other;
        private bool _mythic;

        public SpellInfo() { }

        public SpellInfo(SpellInfo old)
        {
            _name = old._name;
            _dc = old._dc;
            _spell = old._spell;
            _count = old._count;
            _cast = old._cast;
            _alreadyCast = old._alreadyCast;
            _only = old._only;
            _other = old._other;
            _mythic = old._mythic;
        }

        public object Clone()
        {
            return new SpellInfo(this);
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
        public int? Dc
        {
            get => _dc;
            set
            {
                if (_dc != value)
                {
                    _dc = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DC")); }
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
        public int? Count
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
        public bool AlreadyCast
        {
            get => _alreadyCast;
            set
            {
                if (_alreadyCast != value)
                {
                    _alreadyCast = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AlreadyCast")); }
                }
            }
        }
        public string Only
        {
            get => _only;
            set
            {
                if (_only != value)
                {
                    _only = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Only")); }
                }
            }
        }

        public string Other
        {
            get => _other;
            set
            {
                if (_other != value)
                {
                    _other = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Other")); }
                }
            }
        }



        public bool Mythic
        {
            get => _mythic;
            set
            {
                if (_mythic != value)
                {
                    _mythic = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Mythic")); }
                }
            }
        }


    }
}
