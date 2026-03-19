/*
 *  ExportData.cs
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
    public class ExportData : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Monster> _monsters = new List<Monster>();
        private List<Spell> _spells = new List<Spell>();
        private List<Feat> _feats = new List<Feat>();
        private List<Condition> _conditions = new List<Condition>();

        public List<Monster> Monsters
        {
            get => _monsters;
            set
            {
                if (_monsters != value)
                {
                    _monsters = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Monsters")); }
                }
            }
        }
        public List<Spell> Spells
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
        public List<Feat> Feats
        {
            get => _feats;
            set
            {
                if (_feats != value)
                {
                    _feats = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Feats")); }
                }
            }
        }
        public List<Condition> Conditions
        {
            get => _conditions;
            set
            {
                if (_conditions != value)
                {
                    _conditions = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Conditions")); }
                }
            }
        }

        public static ExportData DataFromDBs()
        {
            ExportData data = new ExportData();

            foreach (Monster m in MonsterDb.Db.Monsters)
            {
                data.Monsters.Add(m);
            }
            data.Monsters.Sort((a, b) => a.Name.CompareTo(b.Name));
            foreach (Spell s in Spell.DbSpells)
            {
                data.Spells.Add(s);
            }
            data.Feats.Sort((a, b) => a.Name.CompareTo(b.Name));
            foreach (Feat f in Feat.DbFeats)
            {
                data.Feats.Add(f);
            }
            data.Feats.Sort((a, b) => a.Name.CompareTo(b.Name));
            foreach (Condition c in Condition.CustomConditions)
            {
                data.Conditions.Add(c);
            }
            data.Conditions.Sort((a, b) => a.Name.CompareTo(b.Name));


            return data;
        }

        public void Append(ExportData data)
        {
            foreach (Monster m in data.Monsters)
            {
                _monsters.Add(m);
            }
            foreach (Spell s in data.Spells)
            {
                _spells.Add(s);
            }
            foreach (Feat f in data.Feats)
            {
                _feats.Add(f);
            }
            foreach (Condition c in data.Conditions)
            {
                _conditions.Add(c);
            }
        }

    }
}
