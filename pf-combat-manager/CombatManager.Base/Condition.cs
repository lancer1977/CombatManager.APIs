/*
 *  Condition.cs
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

using CombatManager.ViewModels;

namespace CombatManager
{
    public enum ConditionType
    {
        Condition,
        Spell,
        Afflicition,
        Custom
    }

    public class FavoriteCondition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ConditionType _type;
        private string _name;


        public FavoriteCondition()
        {

        }

        public FavoriteCondition(Condition c)
        {
            this._name = c.Name;
            this._type = c.Type;
        }

        public ConditionType Type
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



    }
    

    public class Condition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static List<Condition> _conditions;
        private static List<Condition> _customConditions;
        private static List<FavoriteCondition> _favoriteConditions;
        private static List<FavoriteCondition> _recentCondtions;

        private static bool _monsterConditionsLoaded;

        private const int RecentLength = 8;

        public static void LoadConditions()
        {
            _conditions = XmlListLoader<Condition>.Load("Condition.xml");

            LoadSpellConditions();
#if !MONO
            LoadMonsterConditions();
#endif
            LoadCustomConditions();
            LoadFavoriteConditions();
            LoadRecentConditions();

        }

        private static void LoadCustomConditions()
        {

           _customConditions = XmlListLoader<Condition>.Load("CustomConditions.xml", true);
           if (_customConditions == null)
           {
               _customConditions = new List<Condition>();
           }
        }

        public static void SaveCustomConditions()
        {
            XmlListLoader<Condition>.Save(_customConditions, "CustomConditions.xml", true);
        }

        private static void LoadRecentConditions()
        {

            _recentCondtions = XmlListLoader<FavoriteCondition>.Load("RecentConditions.xml", true);
            if (_recentCondtions == null)
            {
                _recentCondtions = new List<FavoriteCondition>();
            }
        }

        public static void SaveRecentConditions()
        {
            XmlListLoader<FavoriteCondition>.Save(_recentCondtions, "RecentConditions.xml", true);
        }


        private static void LoadSpellConditions()
        {

            foreach (Spell spell in Spell.Spells)
            {
                Condition c = new Condition();
                c.Name = spell.name;
                c.Spell = spell;
                if (spell.Bonus != null)
                {
                    c.Image = "scrolleffect";
                }
                else
                {
                    c.Image = "scroll";
                }

                Conditions.Add(c);

            }
        }

        public static void LoadMonsterConditions()
        {
            if (!MonsterConditionsLoaded)
            {
                int success = 0;
                int failure = 0;

                foreach (Monster monster in Monster.Monsters)
                {

                    if (monster.SpecialAbilitiesList != null)
                    {
                        //Monster.ParseUsableConditions();
                    }
                }

                if (failure > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Afflictions:  Succeeded: " + success + " Failed: " + failure);
                }

                _monsterConditionsLoaded = true;
            }

        }

        public static Condition FindCondition(string name)
        {
            Condition c = Conditions.Find(delegate(Condition cond)
                {
                    return cond.Name == name;
                });

            return c;
        }

        private static void LoadFavoriteConditions()
        {
            List<FavoriteCondition> list = XmlListLoader<FavoriteCondition>.Load("FavoriteConditions.xml", true);

            if (list != null)
            {
                _favoriteConditions = list;
            }
            else
            {
                _favoriteConditions = new List<FavoriteCondition>();

                //add default entries;
                _favoriteConditions.Add(new FavoriteCondition(ByName("Flat-Footed")));
                _favoriteConditions.Add(new FavoriteCondition(ByName("Grappled")));
                _favoriteConditions.Add(new FavoriteCondition(ByName("Pinned")));
                _favoriteConditions.Add(new FavoriteCondition(ByName("Prone")));


            }
        }

        public static void SaveFavoriteConditions()
        {
            List<FavoriteCondition> list = new List<FavoriteCondition>(FavoriteConditions);
            XmlListLoader<FavoriteCondition>.Save(list, "FavoriteConditions.xml", true);
        }


        static Condition()
        {
        }


        private string _name;
        private string _text;
        private string _image;
        private Spell _spell;
        private Affliction _affliction;
        private ConditionBonus _bonus;
        private bool _custom;


        public Condition()
        {

        }

        public Condition(Condition c)
        {
            _name = c._name;
            _text = c._text;
            _image = c._image;
            _spell = c._spell;
            _custom = c._custom;
            if (c._bonus != null)
            {
                _bonus = new ConditionBonus(c._bonus);
            }
            if (c._affliction != null)
            {
                _affliction = (Affliction)c._affliction.Clone();
            }
        }

        public object Clone()
        {
            return new Condition(this);
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

        public string Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Image")); }
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

        public Affliction Affliction
        {
            get => _affliction;
            set
            {
                if (_affliction != value)
                {
                    _affliction = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Affliction")); }
                }
            }

        }


        public bool Custom
        {
            get => _custom;
            set
            {
                if (_custom != value)
                {
                    _custom = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Custom")); }
                }
            }

        }


        public ConditionBonus Bonus
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


        public static List<Condition> Conditions
        {
            get
            {
                if (_conditions == null)
                {
                    LoadConditions();
                }
                return _conditions;
            }
        }
        public static bool MonsterConditionsLoaded => _monsterConditionsLoaded;

        public static List<Condition> CustomConditions
        {
            get
            {
                if (_customConditions == null)
                {
                    LoadConditions();
                }
                return _customConditions;
            }
        }
        public static List<FavoriteCondition> FavoriteConditions
        {
            get
            {
                if (_favoriteConditions == null)
                {
                    LoadConditions();
                }
                return _favoriteConditions;
            }
        }
        public static List<FavoriteCondition> RecentConditions
        {
            get
            {
                if (_recentCondtions == null)
                {
                    LoadConditions();
                }
                return _recentCondtions;
            }
        }
        public static void PushRecentCondition(Condition c)
        {
            FavoriteCondition f = new FavoriteCondition(c);

            if (!HasFavorite(f))
            {
                int index = RecentIndex(f);

                if (index == -1)
                {
                    //push back
                    List<FavoriteCondition> list = new List<FavoriteCondition>();
                    list.Add(f);
                    for (int i = 0; i < _recentCondtions.Count && i < RecentLength - 1; i++)
                    {
                        list.Add(_recentCondtions[i]);
                    }
                    _recentCondtions = list;
                }
                else if (index > 0)
                {
                    _recentCondtions.RemoveAt(index);
                    _recentCondtions.Insert(0, f);
                }
            }
            SaveRecentConditions();
        }


        public static Condition ByName(string name)
        {
            return Conditions.FirstOrDefault(a => string.Compare(a.Name, name, true) == 0);
        }

        public static Condition ByNameCustom(string name)
        {

            return _customConditions.FirstOrDefault(a => string.Compare(a.Name, name, true) == 0);
        }

        public static Condition FromFavorite(FavoriteCondition fc)
        {
            if (fc.Type == ConditionType.Custom)
            {
                return _customConditions.FirstOrDefault(a => string.Compare(a.Name, fc.Name, true) == 0);
            }
            else
            {
                return _conditions.FirstOrDefault(a => (string.Compare(a.Name, fc.Name, true) == 0) &&
                    (fc.Type == a.Type));
            }
        }

        public static bool HasFavorite(FavoriteCondition fc)
        {
            return _favoriteConditions.FirstOrDefault(a => string.Compare(a.Name, fc.Name, true) == 0 && a.Type == fc.Type) != null;
        }

        public static int RecentIndex(FavoriteCondition fc)
        {
            for (int i = 0; i < _recentCondtions.Count; i++)
            {
                FavoriteCondition r = _recentCondtions[i];
                if (string.Compare(r.Name, fc.Name, true) == 0 && r.Type == fc.Type)
                {
                    return i;
                }
            }

            return -1;
        }

        [XmlIgnore]
        public ConditionType Type
        {
            get
            {
                if (Spell != null)
                {
                    return ConditionType.Spell;
                }
                else if (Affliction != null)
                {
                    return ConditionType.Afflicition;
                }
                else
                {
                    return Custom ? ConditionType.Custom : ConditionType.Condition;
                }

            }
        }

    }
}
