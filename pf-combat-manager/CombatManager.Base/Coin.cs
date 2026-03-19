/*
 *  Coin.cs
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
    public class Coin : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int _pp;
        private int _gp;
        private int _sp;
        private int _cp;

        public Coin()
        {

        }
        public Coin(Coin c)
        {
            this._pp = c._pp;
            this._gp = c._gp;
            this._sp = c._sp;
            this._cp = c._cp;
        }

        public Coin(string s)
        {
            string text = s.Replace(",", "");
            Regex regCoin = new Regex("(?<val>[0-9]+) +(?<type>(pp|gp|sp|cp))");

            foreach (Match m in regCoin.Matches(text))
            {

                int val = int.Parse(m.Groups["val"].Value);

                string type = m.Groups["type"].Value;

                if (type == "pp")
                {
                    _pp += val;
                }
                else if (type == "gp")
                {
                    _gp += val;
                }
                else if (type == "sp")
                {
                    _sp += val;
                }
                else if (type == "cp")
                {
                    _cp += val;
                }

            }
        }

        public int Pp
        {
            get => _pp;
            set
            {
                if (_pp != value)
                {
                    _pp = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PP")); }
                }
            }
        }
        public int Gp
        {
            get => _gp;
            set
            {
                if (_gp != value)
                {
                    _gp = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("GP")); }
                }
            }
        }
        public int Sp
        {
            get => _sp;
            set
            {
                if (_sp != value)
                {
                    _sp = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SP")); }
                }
            }
        }
        public int Cp
        {
            get => _cp;
            set
            {
                if (_cp != value)
                {
                    _cp = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CP")); }
                }
            }
        }

        public decimal GpValue
        {
            get
            {
                decimal val = Pp * 10;
                val += Gp;

                val += ((decimal)Sp) / 10.0m;
                val += ((decimal)Cp) / 100.0m;

                return val;
            }
        }
        public override string ToString()
        {
            string text = "";

            bool first = true;

            if (Pp != 0)
            {
                text += Pp + " pp";
                first = false;
            }
            if (Gp != 0)
            {
                if (!first)
                {
                    text += " ";
                }
                text += Gp + " gp";
                first = false;
            }
            if (Sp != 0)
            {
                if (!first)
                {
                    text += " ";
                }
                text += Sp + " sp";
                first = false;
            }
            if (Cp != 0)
            {
                if (!first)
                {
                    text += " ";
                }
                text += Cp + " cp";
            }

            if (text.Length == 0)
            {
                text = "0 gp";
            }

            return text;

        }


        public object Clone()
        {
            return new Coin(this);
        }

        public static Coin operator +(Coin a, Coin b)
        {
            Coin c = new Coin();
            c.Cp = a.Cp + b.Cp;
            c.Sp = a.Sp + b.Sp;
            c.Gp = a.Gp + b.Gp;
            c.Pp = a.Pp + b.Pp;

            return c;
        }

        public static Coin operator -(Coin a, Coin b)
        {
            Coin c = new Coin();
            c.Cp = a.Cp - b.Cp;
            c.Sp = a.Sp - b.Sp;
            c.Gp = a.Gp - b.Gp;
            c.Pp = a.Pp - b.Pp;

            return c;
        }


    }
}
