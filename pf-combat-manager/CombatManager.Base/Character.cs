/*
 *  Character.cs
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

using System.Runtime.Serialization;
 using CombatManager.PF2;
using CombatManager.Utilities;
using CombatManager.ViewModels;

namespace CombatManager
{
    
    [DataContract]
    public class Character : SimpleNotifyClass
    {
        public enum HpMode : int
        {
            Default = 0,
            Roll = 1,
            Max = 2
        }

        [XmlIgnore]
        public object UndoInfo { get; set; }

        private RulesSystem _rulesSystem;

        private string _name;
        private int _hp;
        private int _maxHp;
        private int _nonlethalDamage;
        private int _temporaryHp;
        private string _notes;
        private ObservableCollection<ActiveResource> _resources;

        private Guid _id;

        private bool _isMonster;
        private BaseMonster _monster;
        private bool _isBlank;
        private bool _isNotSet;



        private bool _isReadying;
        private bool _isDelaying;

        private bool _isHidden;
        private bool _isIdle;

        private uint? _color;


        private string _originalFilename;


        //unsaved data
        private bool _isActive;
        private int _currentInitiative;
        private int _initiativeTiebreaker;
        private bool _hasInitiativeChanged;
        private int _initiativeRolled;
        private bool _isConditionsOpen;
        private bool _isOtherHpOpen;

        private InitiativeCount _initiativeCount;

        private Character _initiativeLeader;
        private ObservableCollection<Character> _initiativeFollowers;
        private Guid? _initiativeLeaderId;

        private CharacterAdjuster _adjuster;


        Dictionary<string, bool> _knownConditions = new Dictionary<string, bool>();

        private static Random _rand = new Random();


        public Character()
        {
            this.InitiativeTiebreaker = _rand.Next();
            _monster = Monster.BlankMonster();
            _isNotSet = true;
            _monster.PropertyChanged += Monster_PropertyChanged;
            Hp = _monster.Hp;
            MaxHp = _monster.Hp;
            _initiativeFollowers = new ObservableCollection<Character>();
            _initiativeFollowers.CollectionChanged += initiativeFollowers_CollectionChanged;
            _resources = new ObservableCollection<ActiveResource>();

        }

  

        public Character(string name) : this()
        {
            this._name = name;
            _monster.Name = name;
			
        }

        public Character(Monster monster, bool rollHp) : this(monster, rollHp?HpMode.Roll:HpMode.Default)
        {

        }

        public Character(Monster monster, HpMode mode) : this()
        {
            this._monster = (Monster)monster.Clone();

            this._name = monster.Name;
            this._hp = monster.GetStartingHp(mode);
            this._maxHp = this._hp;
            this._isMonster = true;

            this._monster.ApplyDefaultConditions();


            this._monster.PropertyChanged += Monster_PropertyChanged;
            Resources = this._monster.TrackedResources;
            LoadResources();
			
        }

        public object Clone()
        {
            Character character = new Character();

            character._name = _name;
            character._hp = _hp;
            character._maxHp = _maxHp;
            character._notes = _notes;
            character.Id = Guid.NewGuid();
            foreach (ActiveResource r in _resources)
            {
                character._resources.Add(new ActiveResource(r));
            }

            character._isMonster = _isMonster;

            if (_monster == null)
            {
                character._monster = null;
            }
            else
            {
                if (_monster is Monster)
                {
                    character._monster = (BaseMonster)(((Monster)_monster).Clone());
                }
                else if (_monster is Pf2Monster)
                {
                    character._monster = (BaseMonster)(((Pf2Monster)_monster).Clone());
                }
                character._monster.PropertyChanged  += character.Monster_PropertyChanged;
            }

            character._isActive = _isActive;
            character._currentInitiative = _currentInitiative;
            character._hasInitiativeChanged = false;
            character._initiativeRolled = _initiativeRolled;
            character._isHidden = _isHidden;
            character._isIdle = _isIdle;
            character._color = _color;

            if (_initiativeCount != null)
            {
                character.InitiativeCount = new InitiativeCount(InitiativeCount);
            }

            character._originalFilename = _originalFilename;

            return character;
        }

        public override string ToString()
        {
            return Name;
        }

        private void LoadResources()
        {

            IEnumerable<ActiveResource> res = _monster.LoadResources();

            foreach (var r in res)
            {
                _resources.Add(r);
            }
        }


        public void Stabilize()
        {
            RemoveConditionByName("dying");
            AddConditionByName("stable");
        }

        [XmlIgnore]
        public bool IsDead => HasCondition("Dead");

        [XmlIgnore]
        public bool IsDying => HasCondition("Dying");


        [XmlIgnore]
        public BaseMonster Stats
        {
            get
            {

                if (_monster is Monster)
                {
                    return (Monster) _monster;
                }
                else
                {
                    return null;
                
                }
            } 
        }

        [XmlIgnore]
        public Pf2Monster Pf2Stats
        {
            get
            {

                if (_monster is Pf2Monster)
                {
                    return (Pf2Monster)_monster;
                }
                else
                {
                    return null;

                }
            }
        }

        [XmlIgnore]
        public RulesSystem RulesSystem
        {
            get => _rulesSystem;
            set
            {
                if (_rulesSystem != value)
                {
                    _rulesSystem = value;
                    Notify();
                }

            }
            
        }

        [DataMember]
        public int RulesSystemInt
        {
            get => (int)RulesSystem;
            set => RulesSystem = (RulesSystem)value;
        }

        [DataMember]
        public string Name
        {
            get => this._name;
            set
            {
                
                if (this._name != value)
                {
                    this._name = value;
                    Notify();
                    Notify("HiddenName");

                    if (this.IsBlank)
                    {
                        if (_monster != null)
                        {
                            _monster.Name = _name;
                        }
                    }
                }
            }
        }


        [DataMember]
        public int Hp
        {
            get => this._hp;
            set
            {
                if (this._hp != value)
                {
                    this._hp = value;
                    Notify("HP");
                }
            }
        }


        [DataMember]
        public int MaxHp
        {
            get => this._maxHp;
            set
            {
                if (this._maxHp != value)
                {
                    this._maxHp = value;
                    Notify("MaxHP");

                    if (IsBlank)
                    {
                        if (_monster != null)
                        {
                            _monster.Hp = _maxHp;
                        }
                    }
                }
            }
        }

        [DataMember]
        public int NonlethalDamage
        {
            get => this._nonlethalDamage;
            set
            {
                if (this._nonlethalDamage != value)
                {
                    this._nonlethalDamage = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int TemporaryHp
        {
            get => this._temporaryHp;
            set
            {
                if (this._temporaryHp != value)
                {
                    this._temporaryHp = value;
                    Notify("TemporaryHP");
                }
            }
        }


        [DataMember]
        public bool IsMonster
        {
            get => this._isMonster;
            set
            {
                if (this._isMonster != value)
                {
                    this._isMonster = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return this._id;
            }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                    Notify("ID");
                }
            }
        }

        [DataMember]
        public bool IsBlank
        {
            get => this._isBlank;
            set
            {
                if (this._isBlank != value)
                {
                    this._isBlank = value;
                    Notify();
                }
            }
        }

        [DataMember]
        [XmlIgnore]
        public bool IsActive
        {
            get => this._isActive;
            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                    Notify();
                }
            }
        }


        [DataMember]
        public bool IsConditionsOpen
        {
            get => this._isConditionsOpen;
            set
            {
                if (this._isConditionsOpen != value)
                {
                    this._isConditionsOpen = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public bool IsOtherHpOpen
        {
            get => this._isOtherHpOpen;
            set
            {
                if (this._isOtherHpOpen != value)
                {
                    this._isOtherHpOpen = value;
                    Notify("IsOtherHPOpen");
                }
            }
        }


        [DataMember]
        public int CurrentInitiative
        {
            get => this._currentInitiative;
            set
            {
                if (this._currentInitiative != value)
                {
                    this._currentInitiative = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int InitiativeTiebreaker
        {
            get => this._initiativeTiebreaker;
            set
            {
                if (this._initiativeTiebreaker != value)
                {
                    this._initiativeTiebreaker = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public bool HasInitiativeChanged
        {
            get => this._hasInitiativeChanged;
            set
            {
                if (this._hasInitiativeChanged != value)
                {
                    this._hasInitiativeChanged = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int InitiativeRolled
        {
            get => this._initiativeRolled;
            set
            {
                if (this._initiativeRolled != value)
                {
                    this._initiativeRolled = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public InitiativeCount InitiativeCount
        {
            get => this._initiativeCount;
            set
            {
                if (this._initiativeCount != value)
                {
                    this._initiativeCount = value;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public Character InitiativeLeader
        {
            get => this._initiativeLeader;
            set
            {
                if (this._initiativeLeader != value)
                {
                    this._initiativeLeader = value;
                    _initiativeLeaderId = null;
                    Notify();
                }
            }
        }


        [DataMember]
        public Guid? InitiativeLeaderId
        {
            get
            {
                if (_initiativeLeader != null)
                {
                    return _initiativeLeader.Id;
                }
                else
                {
                    return _initiativeLeaderId;
                }
            }
            set
            {
                if (this._initiativeLeaderId != value)
                {
                    this._initiativeLeaderId = value;
                    Notify("InitiativeLeaderID");
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<Character> InitiativeFollowers => _initiativeFollowers;


        [XmlIgnore]
        public bool HasFollowers => InitiativeFollowers.Count > 0;


        [DataMember]
        public bool IsDelaying
        {
            get => _isDelaying;
            set
            {
                if (this._isDelaying != value)
                {
                    this._isDelaying = value;
                    Notify();
                    Notify("IsNotReadyingOrDelaying");
                }
            }
        }

        [DataMember]
        public bool IsReadying
        {
            get => _isReadying;
            set
            {
                if (this._isReadying != value)
                {
                    this._isReadying = value;
                    Notify();
                    Notify("IsNotReadyingOrDelaying");
                }
            }
        }

        [DataMember]
        public uint? Color
        {
            get => _color;
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                    Notify();
                }
            }
        }
		
		[XmlIgnore]
        public bool IsNotReadyingOrDelaying => !_isReadying && !_isDelaying;


        void initiativeFollowers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("HasFollowers");
        }

        [DataMember]
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public BaseMonster BaseMonster
        {
            get => _monster;
            set => SetMonster(value);
        }


        [DataMember]
        public Monster Monster
        {
            get
            {
                if (_monster is Monster)
                {
                    return (Monster)_monster;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    BaseMonster = value;
                }
            }
        }

        public static RulesSystem ? MonsterSystem(BaseMonster monster)
        {
            switch (monster)
            {
                case Pf2Monster _:
                    return RulesSystem.Pf2;
                case Monster _:
                    return RulesSystem.Pf1;
                default:
                    return null;

            }
        }


        [DataMember]
        public Pf2Monster Pf2Monster
        {
            get
            {
                if (_monster is Pf2Monster)
                {
                    return (Pf2Monster)_monster;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    BaseMonster = value;
                }
            }
        }

        private void SetMonster(BaseMonster value)
        {

            if (_monster != value)
            {
                RulesSystem? oldSystem = MonsterSystem(_monster);
                RulesSystem? newSystem = MonsterSystem(value);

                bool replacement = !_isNotSet;
                if (_monster != null)
                {
                    _monster.PropertyChanged -= Monster_PropertyChanged;
                    _monster.ActiveConditions.CollectionChanged -= ActiveConditions_CollectionChanged;

                }

                this._monster = value;

                if (_monster != null)
                {
                    _monster.PropertyChanged += Monster_PropertyChanged;
                    _monster.ActiveConditions.CollectionChanged += ActiveConditions_CollectionChanged;

                }

                if (oldSystem != null)
                {
                    NotifyMonsterSystem(oldSystem.Value);
                }
                if (newSystem != null && (oldSystem == null || oldSystem.Value != newSystem.Value))
                {
                    NotifyMonsterSystem(newSystem.Value);
                }

                if (replacement && _monster != null)
                {
                    MaxHp = _monster.Hp;
                    _monster.Notify("Init");
                }

                _isNotSet = false;

            }
        }

        private void NotifyMonsterSystem(RulesSystem system)
        {
            switch (system)
            {
                case RulesSystem.Pf1:
                    Notify("Monster");
                    Notify("Stats");
                    break;
                case RulesSystem.Pf2:
                    Notify("PF2Monster");
                    break;
            }
        }


        private void ActiveConditions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _knownConditions.Clear();
        }

        private void Monster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HP")
            {
                if (MaxHp != _monster.Hp)
                {
                    int diff = _monster.Hp - MaxHp;
                    MaxHp = _monster.Hp;
                    Hp += diff;
                }
            }
        }



        public bool HasCondition(string name)
        {
            bool res;
            if (!_knownConditions.TryGetValue(name, out res))
            {
                res = FindCondition(name) != null;
                _knownConditions[name] = res;
            }
            return res;
        }

        public void AddConditionByName(string name)
        {

            if (!HasCondition(name))
            {
                ActiveCondition c = new ActiveCondition();
                c.Condition = Condition.ByName(name);
                Stats.AddCondition(c);
            }
        }

        public ActiveCondition FindCondition(string name)
        {
            return _monster.FindCondition(name);
        }


        public IEnumerable<ActiveCondition> FindAllConditions(string name)
        {
            return Stats.ActiveConditions.Where<ActiveCondition>
                (a => string.Compare(a.Condition.Name, name, true) == 0);
        }

        public void RemoveConditionByName(string name)
        {
            List<ActiveCondition> list = new List<ActiveCondition>(FindAllConditions(name));
            foreach (ActiveCondition c in list)
            {
                Stats.RemoveCondition(c);
            }
        }

        [XmlIgnore]
        public int MinHp
        {
            get
            {
                int val = 0;

                if (Stats.Constitution != null)
                {
                    val = -Stats.Constitution.Value;
                }
                else if (Stats.Charisma != null)
                {
                    val = -Stats.Charisma.Value;
                }
                return val;

            }
        }

        public int AdjustHp(int val)
        {
            return AdjustHp(val, 0, 0);
        }


        public int AdjustHp(int val, int nlval, int tempval)
        {
            int oldHp = Hp + TemporaryHp;
            int adjust = val;

            if (NonlethalDamage > 0)
            {
                if (adjust > 0)
                {
                    NonlethalDamage = (adjust == NonlethalDamage||adjust > NonlethalDamage?0:NonlethalDamage-adjust);
                }
            }
            if (_temporaryHp > 0)
            {
                if (adjust < 0)
                {
                    if (-adjust <= _temporaryHp)
                    {
                        TemporaryHp += adjust;
                        adjust = 0;
                    }
                    else
                    {
                        adjust = adjust + TemporaryHp;
                        TemporaryHp = 0;
                    }
                }
            }

            TemporaryHp = Math.Max(TemporaryHp + tempval, 0);
            NonlethalDamage = Math.Max(NonlethalDamage +nlval, 0);
            Hp += adjust;

            int effectiveHp = Hp + TemporaryHp;

            if (oldHp > MinHp && effectiveHp <= MinHp)
            {
                RemoveConditionByName("staggered");
                RemoveConditionByName("disabled");
                RemoveConditionByName("dying");
                RemoveConditionByName("stable");
                RemoveConditionByName("unconscious");
                AddConditionByName("dead");
            }
            else if (oldHp == 0 && effectiveHp == 0)
            {
                RemoveConditionByName("unconscious");
                RemoveConditionByName("staggered");
                AddConditionByName("disabled");
            }
            else if (oldHp > 0 && effectiveHp == 0)
            {
                AddConditionByName("disabled");
                AddConditionByName("staggered");
            }

            else if (oldHp >= 0 && effectiveHp < 0)
            {
                if (!HasCondition("dead"))
                {
                    RemoveConditionByName("staggered");
                    RemoveConditionByName("disabled");
                    RemoveConditionByName("stable");
                    AddConditionByName("unconscious");
                    AddConditionByName("dying");
                }
            }
            else if (oldHp <= MinHp && effectiveHp > MinHp && effectiveHp < 0)
            {
                //AddConditionByName("dying");
                AddConditionByName("unconscious");
                AddConditionByName("stable");
                RemoveConditionByName("dead");
            }

            else if (oldHp < 0 && effectiveHp == 0)
            {
                RemoveConditionByName("unconscious");
                AddConditionByName("disabled");
                AddConditionByName("staggered");
                RemoveConditionByName("dying");
                RemoveConditionByName("dead");
                RemoveConditionByName("stable");
            }

            else if (oldHp <= 0 && effectiveHp > 0)
            {
                RemoveConditionByName("unconscious");
                RemoveConditionByName("disabled");
                RemoveConditionByName("staggered");
                RemoveConditionByName("dying");
                RemoveConditionByName("dead");
                RemoveConditionByName("stable");
            }
            else if(oldHp < 0 && effectiveHp > MinHp && adjust > 0)
            {
                RemoveConditionByName("dying");
                AddConditionByName("unconscious");
                AddConditionByName("stable");
            }

            else if (effectiveHp < oldHp && HasCondition("stable"))
            {
                RemoveConditionByName("stable");
                AddConditionByName("dying");
            }
            else if (effectiveHp > 0)
            {
                RemoveConditionByName("unconscious");
                RemoveConditionByName("disabled");
                RemoveConditionByName("staggered");
                RemoveConditionByName("dying");
                RemoveConditionByName("dead");
                RemoveConditionByName("stable");
            }
            if (_nonlethalDamage > 0 && _nonlethalDamage >= effectiveHp)
            {
                if ((!HasCondition("dying") || HasCondition("disabled")) && !HasCondition("dead"))
                {
                    if (_nonlethalDamage == effectiveHp)
                    {
                        RemoveConditionByName("unconscious");
                        AddConditionByName("staggered");
                    }
                    else
                    {
                        RemoveConditionByName("disabled");
                        RemoveConditionByName("staggered");
                        AddConditionByName("unconscious");
                    }
                }
            }
            //else
            //{
            //    RemoveConditionByName("staggered");
            //    RemoveConditionByName("unconscious");
            //}

            return Hp;
        }


        [DataMember]
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;
                    Notify();
                    Notify("HiddenName");
                    
                }
            }
        }


        [DataMember]
        public bool IsIdle
        {
            get => _isIdle;
            set
            {
                if (_isIdle != value)
                {
                    _isIdle = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public ObservableCollection<ActiveResource> Resources
        {
            get => _resources;
            set
            {
                if (_resources != value)
                {
                    _resources = value;

                    Notify();

                }
            }
        }

        [DataMember]
        public string OriginalFilename
        {
            get => this._originalFilename;
            set
            {

                if (this._originalFilename != value)
                {
                    this._originalFilename = value;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public string HiddenName
        {
            get
            {
                if (_isHidden)
                {
                    return "??????";
                }
                else
                {
                    return _name;
                }
            }
        }

        [XmlIgnore]
        public CharacterAdjuster Adjuster
        {
            get
            {
                if (_adjuster == null)
                {
                    _adjuster = new CharacterAdjuster(this);
                }
                return _adjuster;
            }
        }        

        public class CharacterAdjuster : INotifyPropertyChanged
        {

            public event PropertyChangedEventHandler PropertyChanged;

            private Character _c;

            public CharacterAdjuster(Character c)
            {
                _c = c;
                c.PropertyChanged += c_PropertyChanged;
            }

            void c_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                PropertyChanged.Call(this, e.PropertyName);
                
            }

            public int Hp
            {
                get => _c.Hp;
                set
                {
                    int change = value - _c.Hp;

                    _c.AdjustHp(change);

                }
            }

            public int NonlethalDamage
            {
                get => _c.NonlethalDamage;
                set
                {

                    int change = value - _c.NonlethalDamage;

                    _c.AdjustHp(0, change, 0);
                }
            }

            public int TemporaryHp
            {
                get => _c.TemporaryHp;
                set
                {

                    int change = value - _c.TemporaryHp;

                    _c.AdjustHp(0, 0, change);
                }
            }



        }

    }

    public static class EventExt
    {
        public static void Call(this PropertyChangedEventHandler propertyChanged, object sender, string property)
        {
            if (propertyChanged != null) { propertyChanged(sender, new PropertyChangedEventArgs(property)); }

        }
    }

}
