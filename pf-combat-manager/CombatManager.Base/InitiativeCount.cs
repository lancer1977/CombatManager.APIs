/*
 *  InitiativeCount.cs
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
    public class InitiativeCount : INotifyPropertyChanged, IComparable<InitiativeCount>, IComparable
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private int _base;
        private int _dex;
        private int _tiebreaker;

        public InitiativeCount()
        {
        
        }

        public InitiativeCount(int baseval, int dex, int tiebreaker)
        {
            _base = baseval;
            _dex = dex;
            _tiebreaker = tiebreaker;
        }

        public InitiativeCount(InitiativeCount count) : this(count._base, count._dex, count._tiebreaker)
        {

        }

        public static InitiativeCount MaxValue
        {
            get
            {
                InitiativeCount count = new InitiativeCount();
                count._base = int.MaxValue;
                count._dex = int.MaxValue;
                count.Tiebreaker = int.MaxValue;
                return count;
            }
        }


        public object Clone()
        {

            return new InitiativeCount(this);
        }

        public override bool Equals(object obj)
        {
            if (typeof(InitiativeCount) == obj.GetType())
            {
                return (this == (InitiativeCount)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.Base << 8) ^ (this.Dex << 4) ^ (this.Tiebreaker);
        }

        public static bool operator == (InitiativeCount counta, InitiativeCount countb)
        {
            object a = (object)counta;
            object b = (object)countb;
            if (a == null || b == null)
            {
                return (a == null && b == null);
            }

            return counta.CompareTo(countb) == 0;
        }


        public static bool operator !=(InitiativeCount counta, InitiativeCount countb)
        {

            return !(counta == countb);
        }

        public static bool operator > (InitiativeCount counta, InitiativeCount countb)
        {
            return counta.CompareTo(countb) > 0;
        }

        public static bool operator < (InitiativeCount counta, InitiativeCount countb)
        {
            return counta.CompareTo(countb) < 0;
        }

        public static bool operator >= (InitiativeCount counta, InitiativeCount countb)
        {
            return counta.CompareTo(countb) >= 0;
        }

        public static bool operator <= (InitiativeCount counta, InitiativeCount countb)
        {
            return counta.CompareTo(countb) <= 0;
        }



        public int CompareTo(InitiativeCount other)
        {
            if (Base != other.Base)
            {
                return Base.CompareTo(other.Base);
            }

            if (Dex != other.Dex)
            {
                return Dex.CompareTo(other.Dex);
            }

            return Tiebreaker.CompareTo(other.Tiebreaker);

        }



        public int Base
        {
            get => _base;
            set
            {
                if (_base != value)
                {
                    _base = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Base")); }
                }
            }
        }
        public int Dex
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
        public int Tiebreaker
        {
            get => _tiebreaker;
            set
            {
                if (_tiebreaker != value)
                {
                    _tiebreaker = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Tiebreaker")); }
                }
            }
        }

        public string Text => ToString();

        public override string ToString()
        {
            return _base + "-" + _dex + "-" + _tiebreaker;
        }



        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(InitiativeCount))
            {
                throw new ArgumentException("Cannot compare", "obj");
            }
            else
            {
                return CompareTo((InitiativeCount)obj);
            }
        }
    }
}
