/*
 *  Rule.cs
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

using CombatManager.Interfaces;
using PolyhydraGames.Core.IOC;
using ReactiveUI;


namespace CombatManager
{
    public class Rule : ReactiveObject
    { 
        private int _id;
        private string _name;
        private string _details;
        private string _source;
        private string _type;
        private string _abilityType;
        private string _format;
        private string _location;
        private string _format2;
        private string _location2; 
        private bool _untrained;
        private string _ability; 
        private string _subtype;

        private bool _dbDetailsLoaded;

        private static List<Rule> _ruleList;
        private static SortedDictionary<string, string> _types;
        private static Dictionary<string, SortedDictionary<string, string>> _subtypes;

        private static bool _rulesLoaded;

        public static void LoadRules()
        {
            if (_rulesLoaded) return;
            List<Rule> set = XmlListLoader<Rule>.Load("RuleShort.xml");


            _types = new SortedDictionary<string, string>();
            _subtypes = new Dictionary<string, SortedDictionary<string, string>>();
            _ruleList = new List<Rule>();

            foreach (Condition c in Condition.Conditions)
            {
                if (c.Text == null) continue;
                Rule r = new Rule
                {
                    Name = c.Name,
                    Type = "Condition",
                    Source = "PFRPG Core",
                    Details = c.Text
                };
                set.Add(r);
            }

            foreach (Rule rule in set)
            {
                _ruleList.Add(rule);

                _types[rule.Type] = rule.Type;

                if (rule.Subtype is not { Length: > 0 }) continue;
                if (!_subtypes.ContainsKey(rule.Type))
                {
                    _subtypes[rule.Type] = new SortedDictionary<string, string>();
                }
                _subtypes[rule.Type][rule.Subtype] = rule.Subtype;
            }

            _rulesLoaded = true;


        }

        public static bool RulesLoaded => _rulesLoaded;


        public static List<Rule> Rules
        {
            get
            {
                if (_ruleList == null)
                {
                    LoadRules();
                }
                return _ruleList;
            }
        }

        public static Rule Find(string name, string type)
        {
            return Rule.Rules.FirstOrDefault<Rule>(a => (string.Compare(a.Name,name, true) == 0 && a.Type == type));
        }
        

        public static ICollection<string> Types
        {
            get
            {
                if (_ruleList == null)
                {
                    LoadRules();
                }
                return _types.Values;
            }
        }
        public static Dictionary<string, SortedDictionary<string, string> > Subtypes
        {
            get
            {
                if (_ruleList == null)
                {
                    LoadRules();
                }
                return _subtypes;
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged();
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
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Details
        {
            get 
            {

                if (_details == null && _id != null && !_dbDetailsLoaded)
                {
                    _dbDetailsLoaded = true;
                    _details = IOC.Get<IRulesService>().GetDetails(_id);
                }

                return _details; 
            

            }
            set
            {
                if (_details != value)
                {
                    _details = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    this.RaisePropertyChanged();
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
                    this.RaisePropertyChanged();
                }
            }
        }

        public string AbilityType
        {
            get => _abilityType;
            set
            {
                if (_abilityType != value)
                {
                    _abilityType = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Format
        {
            get => _format;
            set
            {
                if (_format != value)
                {
                    _format = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Format2
        {
            get => _format2;
            set
            {
                if (_format2 != value)
                {
                    _format2 = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Location2
        {
            get => _location2;
            set
            {
                if (_location2 != value)
                {
                    _location2 = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public bool Untrained
        {
            get => _untrained;
            set
            {
                if (_untrained != value)
                {
                    _untrained = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Ability
        {
            get => _ability;
            set
            {
                if (_ability != value)
                {
                    _ability = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Subtype
        {
            get => _subtype;
            set
            {
                if (_subtype != value)
                {
                    _subtype = value;
                    this.RaisePropertyChanged();
                }
            }
        }




    }
}
