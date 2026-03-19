/*
 *  ArmorWeaponChart.cs
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
    public class ArmorWeaponChart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _weight;
        private string _name;
        private string _cost;
        private string _materials;
        private string _type;

        private static List<ArmorWeaponChart> _chart;

        private static Dictionary<string, int> _totalWeights;

        static ArmorWeaponChart()
        {
            try
            {
                _chart = XmlListLoader<ArmorWeaponChart>.Load("ArmorWeaponChart.xml");

                _totalWeights = new Dictionary<string, int>();

                foreach (ArmorWeaponChart chart in _chart)
                {
                    int weight = int.Parse(chart._weight);

                    if (!_totalWeights.ContainsKey(chart._type))
                    {
                        _totalWeights[chart._type] = weight;
                    }
                    else
                    {
                        _totalWeights[chart.Type] = _totalWeights[chart.Type] + weight;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public string Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Weight")); }
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
        public string Materials
        {
            get => _materials;
            set
            {
                if (_materials != value)
                {
                    _materials = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Materials")); }
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

        public static List<ArmorWeaponChart> Chart => _chart;

        public static Dictionary<string, int> TotalWeights => _totalWeights;
    }
}
