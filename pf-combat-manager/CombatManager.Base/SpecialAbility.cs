/*
 *  SpecialAbility.cs
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
    public class SpecialAbility : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _type;
        private string _text;
        private int? _constructionPoints;



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
        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    if (PropertyChanged != null) 
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                        PropertyChanged(this, new PropertyChangedEventArgs("AbilityTypeIndex")); 
                    }
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
        public int? ConstructionPoints
        {

            get => _constructionPoints;
            set
            {
                if (_constructionPoints != value)
                {
                    _constructionPoints = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ConstructionPoints")); }
                }
            }
        }

        [XmlIgnore]
        public int AbilityTypeIndex
        {
            get
            {
                if (string.Compare(_type, "Ex") == 0)
                {
                    return 0;
                }
                else if (string.Compare(_type, "Sp") == 0)
                {
                    return 1;
                }
                else if (string.Compare(_type, "Su") == 0)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            set
            {
                if (value == 0)
                {
                    Type = "Ex";
                }
                else if (value == 1)
                {
                    Type = "Sp";
                }
                else if (value == 2)
                {
                    Type = "Su";
                }
                else
                {
                    Type = "";
                }
            }
        }

        public object Clone()
        {
            SpecialAbility s = new SpecialAbility();
            s.Name = Name;
            s.Text = Text;
            s.Type = Type;
            s.ConstructionPoints = ConstructionPoints;

            return s;

        }
    }
}
