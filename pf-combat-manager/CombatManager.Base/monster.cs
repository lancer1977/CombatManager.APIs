/*
 *  monster.cs
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

using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using CombatManager.Database;
using CombatManager.Utilities;
using CombatManager.ViewModels;
using PolyhydraGames.Extensions;
using ReactiveUI.Fody.Helpers;
using static CombatManager.Character;

namespace CombatManager
{

    public class MonsterParseException : Exception
    {
        public MonsterParseException(string message)
            : base(message)
        {

        }

        public MonsterParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }

    [DataContract]
    public class Monster : BaseMonster
    {

        public void NotifyPropertyChanged(string property)
        {
            Notify(property);

        }

        private string _xp;
        private string _race;
        private string _className;
        private string _type;
        private string _subType;
        private string _senses;
        private string _ac;
        private string _acMods;
        private string _hd;
        private string _saves;
        private string _saveMods;
        private string _dr;
        private string _sr;
        private string _melee;
        private string _ranged;
        private string _space;
        private string _reach;
        private string _specialAttacks;
        private string _spellLikeAbilities;
        private string _abilitiyScores;
        private int _baseAtk;
        private string _cmb;
        private string _cmd;
        private string _feats;
        private string _skills;
        private string _racialMods;
        private string _sq;
        private string _environment;
        private string _organization;
        private string _treasure;
        private string _descriptionVisual;
        private string _group;
        private string _isTemplate;
        private string _specialAbilities;
        private string _description;
        private string _fullText;
        private string _gender;
        private string _bloodline;
        private string _prohibitedSchools;
        private string _beforeCombat;
        private string _duringCombat;
        private string _morale;
        private string _gear;
        private string _otherGear;
        private string _vulnerability;
        private string _note;
        private string _characterFlag;
        private string _companionFlag;
        private string _fly;
        private string _climb;
        private string _burrow;
        private string _swim;
        private string _land;
        private string _templatesApplied;
        private string _offenseNote;
        private string _baseStatistics;
        private string _spellsPrepared;
        private string _spellDomains;
        private string _aura;
        private string _defensiveAbilities;
        private string _spellsKnown;
        private string _speedMod;
        private string _monsterSource;
        private string _extractsPrepared;
        private string _ageCategory;
        private bool _dontUseRacialHd;
        private string _variantParent;
        private bool _npc;
        private string _descHtml;
        private int? _mr;
        private string _mythic;

        private bool _statsParsed;
        private int? _strength;
        private int? _dexterity;
        private int? _constitution;
        private int? _intelligence;
        private int? _wisdom;
        private int? _charisma;

        private bool _specialAblitiesParsed;
        private ObservableCollection<SpecialAbility> _specialAbilitiesList;

        private bool _acParsed;
        private int _fullAc;
        private int _touchAc;
        private int _flatFootedAc;
        private int _naturalArmor;
        private int _shield;
        private int _armor;
        private int _dodge;
        private int _deflection;

        private bool _skillsParsed;
        private bool _skillValuesMayNeedUpdate;
        private SortedDictionary<string, SkillValue> _skillValueDictionary;
        private List<SkillValue> _skillValueList;

        private bool _featsParsed;
        private List<string> _featsList;

        private static Dictionary<string, Stat> _skillsList;
        private static Dictionary<string, SkillInfo> _skillsDetails;


        private ObservableCollection<Condition> _usableConditions;
        private bool _usableConditionsParsed;

        private bool _loseDexBonus;
        private bool _dexZero;
        private int? _preLossDex;
        private bool _strZero;
        private int? _preLossStr;

        private ObservableCollection<SpellBlockInfo> _spellLikeAbilitiesBlock;
        private ObservableCollection<SpellBlockInfo> _spellsKnownBlock;
        private ObservableCollection<SpellBlockInfo> _spellsPreparedBlock;

        //multiple system allowance
        RulesSystem _rulesSystem;

        private struct DragonColorInfo
        {
            public string Element;
            public string WeaponType;
            public int Distance;

            public DragonColorInfo(string element, string weaponType, int distance)
            {
                this.Element = element;
                this.WeaponType = weaponType;
                this.Distance = distance;
            }
        }


        private static Dictionary<string, int> _xpValues;
        private static Dictionary<string, bool> _thrownAttacks;
        private static Dictionary<int, int> _lowCrToIntChart;
        private static Dictionary<int, int> _intToLowCrChart;
        private static Dictionary<string, int> _flyQualityList;
        private static Dictionary<string, DragonColorInfo> _dragonColorList;
        private static SortedDictionary<string, string> _weaponNameList;
        private static Dictionary<CreatureType, string> _creatureTypeNames;
        private static List<string> _creatureTypeNamesList;
        private static Dictionary<string, CreatureType> _creatureTypes;


        private MonsterAdjuster _adjuster;



        public class SkillInfo
        {
            public string Name { get; set; }
            public Stat Stat { get; set; }
            public List<string> Subtypes { get; set; }
            public bool TrainedOnly { get; set; }
        }

        public enum FlyQuality
        {
            Clumsy = 0,
            Poor = 1,
            Average = 2,
            Good = 3,
            Perfect = 4
        }
        public enum ManeuverType
        {
            BullRush = 0,
            DirtyTrick,
            Disarm,
            Drag,
            Grapple,
            Overrun,
            Reposition,
            Steal,
            Sunder,
            Trip
        }

        static Monster()
        {
            CombatManeuvers = new List<string>();

            foreach (var maneuver in (ManeuverType[])Enum.GetValues(typeof(ManeuverType)))
            {
                CombatManeuvers.Add(ManeuverName(maneuver));
            }



            _skillsList = new Dictionary<string, Stat>(new InsensitiveEqualityCompararer());

            _skillsList["Acrobatics"] = Stat.Dexterity;
            _skillsList["Appraise"] = Stat.Intelligence;
            _skillsList["Bluff"] = Stat.Charisma;
            _skillsList["Climb"] = Stat.Strength;
            _skillsList["Craft"] = Stat.Intelligence;
            _skillsList["Diplomacy"] = Stat.Charisma;
            _skillsList["Disable Device"] = Stat.Dexterity;
            _skillsList["Disguise"] = Stat.Charisma;
            _skillsList["Escape Artist"] = Stat.Dexterity;
            _skillsList["Fly"] = Stat.Dexterity;
            _skillsList["Handle Animal"] = Stat.Charisma;
            _skillsList["Heal"] = Stat.Wisdom;
            _skillsList["Intimidate"] = Stat.Charisma;
            _skillsList["Knowledge"] = Stat.Intelligence;
            _skillsList["Linguistics"] = Stat.Intelligence;
            _skillsList["Perception"] = Stat.Wisdom;
            _skillsList["Perform"] = Stat.Charisma;
            _skillsList["Profession"] = Stat.Wisdom;
            _skillsList["Ride"] = Stat.Dexterity;
            _skillsList["Sense Motive"] = Stat.Wisdom;
            _skillsList["Sleight of Hand"] = Stat.Dexterity;
            _skillsList["Spellcraft"] = Stat.Intelligence;
            _skillsList["Stealth"] = Stat.Dexterity;
            _skillsList["Survival"] = Stat.Wisdom;
            _skillsList["Swim"] = Stat.Strength;
            _skillsList["Use Magic Device"] = Stat.Charisma;


            _skillsDetails = new Dictionary<string, SkillInfo>(new InsensitiveEqualityCompararer());
            foreach (KeyValuePair<string, Stat> item in _skillsList)
            {
                SkillInfo info = new SkillInfo();
                info.Name = item.Key;
                info.Stat = item.Value;
                _skillsDetails.Add(info.Name, info);
            }

            _skillsDetails["Disable Device"].TrainedOnly = true;
            _skillsDetails["Handle Animal"].TrainedOnly = true;
            _skillsDetails["Profession"].TrainedOnly = true;
            _skillsDetails["Sleight of Hand"].TrainedOnly = true;
            _skillsDetails["Spellcraft"].TrainedOnly = true;
            _skillsDetails["Use Magic Device"].TrainedOnly = true;

            SkillInfo know = _skillsDetails["Knowledge"];
            know.TrainedOnly = true;
            know.Subtypes = new List<string>();
            know.Subtypes.Add("Arcana");
            know.Subtypes.Add("Dungeoneering");
            know.Subtypes.Add("Engineering");
            know.Subtypes.Add("Geography");
            know.Subtypes.Add("History");
            know.Subtypes.Add("Local");
            know.Subtypes.Add("Nature");
            know.Subtypes.Add("Nobility");
            know.Subtypes.Add("Planes");
            know.Subtypes.Add("Religion");

            SkillInfo craft = _skillsDetails["Craft"];
            craft.Subtypes = new List<string>();
            craft.Subtypes.Add("Alchemy");
            craft.Subtypes.Add("Armor");
            craft.Subtypes.Add("Baskets");
            craft.Subtypes.Add("Blacksmith");
            craft.Subtypes.Add("Books");
            craft.Subtypes.Add("Bows");
            craft.Subtypes.Add("Calligraphy");
            craft.Subtypes.Add("Carpentry");
            craft.Subtypes.Add("Cloth");
            craft.Subtypes.Add("Clothing");
            craft.Subtypes.Add("Gemcutting");
            craft.Subtypes.Add("Glass");
            craft.Subtypes.Add("Jewelry");
            craft.Subtypes.Add("Leather");
            craft.Subtypes.Add("Locks");
            craft.Subtypes.Add("Painting");
            craft.Subtypes.Add("Pottery");
            craft.Subtypes.Add("Rope");
            craft.Subtypes.Add("Sculpture");
            craft.Subtypes.Add("Ships");
            craft.Subtypes.Add("Shoes");
            craft.Subtypes.Add("Stonemasonry");
            craft.Subtypes.Add("Traps");
            craft.Subtypes.Add("Weapons");

            SkillInfo perform = _skillsDetails["Perform"];

            perform.Subtypes = new List<string>();
            perform.Subtypes.Add("Act");
            perform.Subtypes.Add("Comedy");
            perform.Subtypes.Add("Dance");
            perform.Subtypes.Add("Keyboard Instruments");
            perform.Subtypes.Add("Oratory");
            perform.Subtypes.Add("Percussion Instruments");
            perform.Subtypes.Add("Sing");
            perform.Subtypes.Add("String Instruments");
            perform.Subtypes.Add("Wind Instruments");


            SkillInfo profession = _skillsDetails["Profession"];
            profession.Subtypes = new List<string>();
            profession.Subtypes.Add("Architect");
            profession.Subtypes.Add("Baker");
            profession.Subtypes.Add("Barkeep");
            profession.Subtypes.Add("Barmaid");
            profession.Subtypes.Add("Barrister");
            profession.Subtypes.Add("Brewer");
            profession.Subtypes.Add("Butcher");
            profession.Subtypes.Add("Clerk");
            profession.Subtypes.Add("Cook");
            profession.Subtypes.Add("Courtesean");
            profession.Subtypes.Add("Driver");
            profession.Subtypes.Add("Engineer");
            profession.Subtypes.Add("Farmer");
            profession.Subtypes.Add("Fisherman");
            profession.Subtypes.Add("Fortune-Teller");
            profession.Subtypes.Add("Gambler");
            profession.Subtypes.Add("Gardener");
            profession.Subtypes.Add("Herbalist");
            profession.Subtypes.Add("Innkeeper");
            profession.Subtypes.Add("Librarian");
            profession.Subtypes.Add("Medium");
            profession.Subtypes.Add("Merchant");
            profession.Subtypes.Add("Midwife");
            profession.Subtypes.Add("Miller");
            profession.Subtypes.Add("Miner");
            profession.Subtypes.Add("Porter");
            profession.Subtypes.Add("Sailor");
            profession.Subtypes.Add("Scribe");
            profession.Subtypes.Add("Shepherd");
            profession.Subtypes.Add("Soldier");
            profession.Subtypes.Add("Soothsayer");
            profession.Subtypes.Add("Stable Master");
            profession.Subtypes.Add("Tanner");
            profession.Subtypes.Add("Torturer");
            profession.Subtypes.Add("Trapper");
            profession.Subtypes.Add("Woodcutter");



            try
            {
                _xpValues = new Dictionary<string, int>();
                _xpValues.Add("1/8", 50);
                _xpValues.Add("1/6", 65);
                _xpValues.Add("1/4", 100);
                _xpValues.Add("1/3", 135);
                _xpValues.Add("1/2", 200);

                _lowCrToIntChart = new Dictionary<int, int>();
                _lowCrToIntChart.Add(8, -4);
                _lowCrToIntChart.Add(6, -3);
                _lowCrToIntChart.Add(4, -2);
                _lowCrToIntChart.Add(3, -1);
                _lowCrToIntChart.Add(2, 0);

                _intToLowCrChart = new Dictionary<int, int>();


                foreach (KeyValuePair<int, int> pair in _lowCrToIntChart)
                {
                    _intToLowCrChart.Add(pair.Value, pair.Key);
                }

                _thrownAttacks = new Dictionary<string, bool>();
                _thrownAttacks.Add("rock", true);
                _thrownAttacks.Add("dagger", false);
                _thrownAttacks.Add("club", false);
                _thrownAttacks.Add("spear", false);
                _thrownAttacks.Add("shortspear", false);
                _thrownAttacks.Add("dart", false);
                _thrownAttacks.Add("javelin", false);
                _thrownAttacks.Add("throwing axe", false);
                _thrownAttacks.Add("light hammer", false);
                _thrownAttacks.Add("trident", false);
                _thrownAttacks.Add("shuriken", false);
                _thrownAttacks.Add("net", false);



                //fly quality
                _flyQualityList = new Dictionary<string, int>();
                _flyQualityList.Add("clumsy", 0);
                _flyQualityList.Add("poor", 1);
                _flyQualityList.Add("average", 2);
                _flyQualityList.Add("good", 3);
                _flyQualityList.Add("perfect", 4);

                //elements
                _dragonColorList = new Dictionary<string, DragonColorInfo>();
                _dragonColorList.Add("black", new DragonColorInfo("acid", "line", 60));
                _dragonColorList.Add("blue", new DragonColorInfo("electricity", "line", 60));
                _dragonColorList.Add("brass", new DragonColorInfo("fire", "line", 60));
                _dragonColorList.Add("bronze", new DragonColorInfo("electricity", "line", 60));
                _dragonColorList.Add("copper", new DragonColorInfo("acid", "line", 60));
                _dragonColorList.Add("gold", new DragonColorInfo("fire", "cone", 30));
                _dragonColorList.Add("green", new DragonColorInfo("acid", "cone", 30));
                _dragonColorList.Add("red", new DragonColorInfo("fire", "cone", 30));
                _dragonColorList.Add("silver", new DragonColorInfo("cold", "cone", 30));
                _dragonColorList.Add("white", new DragonColorInfo("cold", "cone", 30));

                LoadWeaponNames();

                _creatureTypeNames = new Dictionary<CreatureType, string>();

                _creatureTypeNames[CreatureType.Aberration] = "aberration";
                _creatureTypeNames[CreatureType.Animal] = "animal";
                _creatureTypeNames[CreatureType.Construct] = "construct";
                _creatureTypeNames[CreatureType.Dragon] = "dragon";
                _creatureTypeNames[CreatureType.Fey] = "fey";
                _creatureTypeNames[CreatureType.Humanoid] = "humanoid";
                _creatureTypeNames[CreatureType.MagicalBeast] = "magical beast";
                _creatureTypeNames[CreatureType.MonstrousHumanoid] = "monstrous humanoid";
                _creatureTypeNames[CreatureType.Ooze] = "ooze";
                _creatureTypeNames[CreatureType.Outsider] = "outsider";
                _creatureTypeNames[CreatureType.Plant] = "plant";
                _creatureTypeNames[CreatureType.Undead] = "undead";
                _creatureTypeNames[CreatureType.Vermin] = "vermin";

                _creatureTypes = new Dictionary<string, CreatureType>();
                foreach (KeyValuePair<CreatureType, string> name in _creatureTypeNames)
                {
                    _creatureTypes[name.Value] = name.Key;
                }


                _creatureTypeNamesList = new List<string>(_creatureTypeNames.Values);
                _creatureTypeNamesList.Sort();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }



        public static List<string> CreatureTypeNames => _creatureTypeNamesList;

        private static void LoadWeaponNames()
        {
            _weaponNameList = new SortedDictionary<string, string>();

            try
            {
                foreach (Weapon wp in Weapon.Weapons.Values)
                {
                    if (!wp.Natural)
                    {
                        string wpName = wp.Name.ToLower();
                        _weaponNameList[wpName] = wpName;
                    }
                }

            }
            catch (Exception)
            {
            }


        }

        static SortedDictionary<double, string> _crs;

        public static SortedDictionary<double, string> CrList
        {
            get
            {
                if (_crs == null)
                {

                    _crs = new SortedDictionary<double, string>();


                    _crs[1.0 / 8.0] = "1/8";
                    _crs[1.0 / 6.0] = "1/6";
                    _crs[1.0 / 4.0] = "1/4";
                    _crs[1.0 / 3.0] = "1/3";
                    _crs[1.0 / 2.0] = "1/2";

                    for (int i = 1; i < 31; i++)
                    {
                        _crs[i] = i.ToString();
                    }


                }

                return _crs;
            }
        }

        public Monster()
        {
            _skillValueDictionary = new SortedDictionary<string, SkillValue>(StringComparer.OrdinalIgnoreCase);
            _skillValueList = new List<SkillValue>(); 
        }

        public static Monster BlankMonster()
        {
            Monster m = new Monster();

            m._abilitiyScores = "Str 10, Dex 10, Con 10, Int 10, Wis 10, Cha 10";
            m.ParseStats();

            m.Ac = "10, touch 10, flat-footed 10";
            m.AcMods = "";

            m.Cmb = "+0";
            m.Cmd = "10";

            m.Hp = 5;
            m.Hd = "1d8";

            m.Ref = 0;
            m.Fort = 0;
            m.Will = 0;

            m.Cr = "1";
            m.Xp = GetCrValue(m.Cr).ToString();

            m.Alignment = "N";
            m.Size = "Medium";
            m.Type = "humanoid";

            m.Init = 0;
            m.Senses = "Perception +0";

            m.Speed = "30 ft.";

            return m;
        }

        public void CopyFrom(Monster m)
        {
            BaseMonsterCopy(m);

            ActiveConditions.Clear();
            foreach (ActiveCondition c in ((BaseMonster)m).ActiveConditions)
            {
                ActiveConditions.Add(new ActiveCondition(c));
            }

            UsableConditions.Clear();
            foreach (Condition c in m.UsableConditions)
            {
                UsableConditions.Add(new Condition(c));
            }

            DexZero = m.DexZero;
            _detailsId = m.DetailsId;
            Name = m.Name;
            Xp = m._xp;
            Race = m._race;
            _className = m._className;
            Type = m._type;
            SubType = m._subType;
            DualInit = m.DualInit;
            Senses = m._senses;
            Ac = m._ac;
            AcMods = m._acMods;
            Hd = m._hd;
            Saves = m._saves;
            SaveMods = m._saveMods;
            Dr = m._dr;
            Sr = m._sr;
            Melee = m._melee;
            Ranged = m._ranged;
            Space = m._space;
            Reach = m._reach;
            SpecialAttacks = m._specialAttacks;
            SpellLikeAbilities = m._spellLikeAbilities;
            AbilitiyScores = m._abilitiyScores;
            BaseAtk = m._baseAtk;
            Cmb = m._cmb;
            Cmd = m._cmd;
            Feats = m._feats;
            Skills = m._skills;
            RacialMods = m._racialMods;
            Sq = m._sq;
            Environment = m._environment;
            Organization = m._organization;
            Treasure = m._treasure;
            DescriptionVisual = m._descriptionVisual;
            Group = m.Group;
            IsTemplate = m._isTemplate;
            SpecialAbilities = m._specialAbilities;
            Description = m._description;
            FullText = m._fullText;
            Gender = m._gender;
            Bloodline = m._bloodline;
            ProhibitedSchools = m._prohibitedSchools;
            BeforeCombat = m._beforeCombat;
            DuringCombat = m._duringCombat;
            Morale = m._morale;
            Gear = m._gear;
            OtherGear = m._otherGear;
            Vulnerability = m._vulnerability;
            Note = m._note;
            CharacterFlag = m._characterFlag;
            CompanionFlag = m._companionFlag;
            Fly = m._fly;
            Climb = m._climb;
            Burrow = m._burrow;
            Swim = m._swim;
            Land = m._land;
            TemplatesApplied = m._templatesApplied;
            OffenseNote = m._offenseNote;
            BaseStatistics = m._baseStatistics;
            SpellsPrepared = m._spellsPrepared;
            SpellDomains = m._spellDomains;
            Aura = m._aura;
            DefensiveAbilities = m._defensiveAbilities;
            SpellsKnown = m._spellsKnown;
            SpeedMod = m._speedMod;
            MonsterSource = m._monsterSource;
            ExtractsPrepared = m._extractsPrepared;
            AgeCategory = m._ageCategory;
            DontUseRacialHd = m._dontUseRacialHd;
            VariantParent = m._variantParent;
            Npc = m._npc;
            DescHtml = m._descHtml;
            Mythic = m._mythic;
            Mr = m._mr;
            StatsParsed = m._statsParsed;
            Strength = m._strength;
            Dexterity = m._dexterity;
            Constitution = m._constitution;
            Intelligence = m._intelligence;
            Wisdom = m._wisdom;
            Charisma = m._charisma;

            SpecialAblitiesParsed = m._specialAblitiesParsed;
            if (m._specialAbilitiesList != null)
            {
                _specialAbilitiesList = new ObservableCollection<SpecialAbility>();
                foreach (SpecialAbility ability in m._specialAbilitiesList)
                {
                    _specialAbilitiesList.Add((SpecialAbility)ability.Clone());
                }
            }

            AcParsed = m._acParsed;
            FullAc = m._fullAc;
            TouchAc = m._touchAc;
            FlatFootedAc = m._flatFootedAc;
            NaturalArmor = m._naturalArmor;
            Armor = m._armor;
            Dodge = m._dodge;
            Shield = m._shield;
            Deflection = m._deflection;

            _skillsParsed = m._skillsParsed;
            if (m._skillsParsed)
            {
                _skillValueDictionary.Clear();

                foreach (SkillValue skillValue in m._skillValueDictionary.Values)
                {

                    _skillValueDictionary[skillValue.FullName] = (SkillValue)skillValue.Clone();
                }
                _skillValueList.Clear();
                foreach (SkillValue skillValue in m._skillValueList)
                {
                    _skillValueList.Add(skillValue);
                }

            }

            _featsParsed = m._featsParsed;
            if (_featsList != null)
            {
                _featsList = new List<string>(m._featsList);
            }

            if (m._spellsPreparedBlock != null)
            {
                _spellsPreparedBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in m._spellsPreparedBlock)
                {
                    _spellsPreparedBlock.Add(new SpellBlockInfo(info));
                }
            }
            if (m._spellsKnownBlock != null)
            {
                _spellsKnownBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in m._spellsKnownBlock)
                {
                    _spellsKnownBlock.Add(new SpellBlockInfo(info));
                }
            }
            if (m._spellLikeAbilitiesBlock != null)
            {
                _spellLikeAbilitiesBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in m._spellLikeAbilitiesBlock)
                {
                    _spellLikeAbilitiesBlock.Add(new SpellBlockInfo(info));
                }
            }
            TrackedResources.Clear();
            if (m.TrackedResources != null)
            {
                foreach (var tresource in m.TrackedResources)
                {
                    TrackedResources.Add(new ActiveResource(tresource));
                }
            }

            ((BaseDbClass)this).DbLoaderId = ((BaseDbClass)m).DbLoaderId;

        }

        public object Clone()
        {
            Monster m = new Monster();
            BaseMonsterClone(m);
            BaseClone(m);

            m._detailsId = _detailsId;
            m._xp = _xp;
            m._race = _race;
            m._className = _className;
            m._type = _type;
            m._subType = _subType;
            m.DualInit = DualInit;
            m._senses = _senses;
            m._ac = _ac;
            m._acMods = _acMods;
            m._hd = _hd;
            m._saves = _saves;
            m._saveMods = _saveMods;
            m._dr = _dr;
            m._sr = _sr;
            m._melee = _melee;
            m._ranged = _ranged;
            m._space = _space;
            m._reach = _reach;
            m._specialAttacks = _specialAttacks;
            m._spellLikeAbilities = _spellLikeAbilities;
            m._abilitiyScores = _abilitiyScores;
            m._baseAtk = _baseAtk;
            m._cmb = _cmb;
            m._cmd = _cmd;
            m._feats = _feats;
            m._skills = _skills;
            m._racialMods = _racialMods;
            m._sq = _sq;
            m._environment = _environment;
            m._organization = _organization;
            m._treasure = _treasure;
            m._descriptionVisual = _descriptionVisual;
            m._group = _group;
            m._isTemplate = _isTemplate;
            m._specialAbilities = _specialAbilities;
            m._description = _description;
            m._fullText = _fullText;
            m._gender = _gender;
            m._bloodline = _bloodline;
            m._prohibitedSchools = _prohibitedSchools;
            m._beforeCombat = _beforeCombat;
            m._duringCombat = _duringCombat;
            m._morale = _morale;
            m._gear = _gear;
            m._otherGear = _otherGear;
            m._vulnerability = _vulnerability;
            m._note = _note;
            m._characterFlag = _characterFlag;
            m._companionFlag = _companionFlag;
            m._fly = _fly;
            m._climb = _climb;
            m._burrow = _burrow;
            m._swim = _swim;
            m._land = _land;
            m._templatesApplied = _templatesApplied;
            m._offenseNote = _offenseNote;
            m._baseStatistics = _baseStatistics;
            m._spellsPrepared = _spellsPrepared;
            m._spellDomains = _spellDomains;
            m._aura = _aura;
            m._defensiveAbilities = _defensiveAbilities;
            m._spellsKnown = _spellsKnown;
            m._speedMod = _speedMod;
            m._monsterSource = _monsterSource;
            m._extractsPrepared = _extractsPrepared;
            m._ageCategory = _ageCategory;
            m._dontUseRacialHd = _dontUseRacialHd;
            m._variantParent = _variantParent;
            m._npc = _npc;
            m._descHtml = _descHtml;
            m._mr = _mr;
            m._mythic = _mythic;


            m._statsParsed = _statsParsed;

            m._specialAblitiesParsed = _specialAblitiesParsed;
            if (_specialAbilitiesList != null)
            {
                m._specialAbilitiesList = new ObservableCollection<SpecialAbility>();
                foreach (SpecialAbility ability in _specialAbilitiesList)
                {
                    m._specialAbilitiesList.Add((SpecialAbility)ability.Clone());
                }
            }

            m._acParsed = _acParsed;
            m._fullAc = _fullAc;
            m._touchAc = _touchAc;
            m._flatFootedAc = _flatFootedAc;
            m._naturalArmor = _naturalArmor;
            m._armor = _armor;
            m._dodge = _dodge;
            m._shield = _shield;
            m._deflection = _deflection;

            m._skillsParsed = _skillsParsed;
            if (_skillsParsed)
            {
                m._skillValueDictionary = new SortedDictionary<string, SkillValue>(StringComparer.OrdinalIgnoreCase);
                foreach (SkillValue skillValue in _skillValueDictionary.Values)
                {

                    m._skillValueDictionary[skillValue.FullName] = (SkillValue)skillValue.Clone();
                }
            }

            m._featsParsed = _featsParsed;
            if (_featsList != null)
            {
                m._featsList = new List<string>(_featsList);
            }

            if (_spellsPreparedBlock != null)
            {
                m._spellsPreparedBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in _spellsPreparedBlock)
                {
                    m._spellsPreparedBlock.Add(new SpellBlockInfo(info));
                }
            }
            if (_spellsKnownBlock != null)
            {
                m._spellsKnownBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in _spellsKnownBlock)
                {
                    m._spellsKnownBlock.Add(new SpellBlockInfo(info));
                }
            }
            if (_spellLikeAbilitiesBlock != null)
            {
                m._spellLikeAbilitiesBlock = new ObservableCollection<SpellBlockInfo>();
                foreach (SpellBlockInfo info in _spellLikeAbilitiesBlock)
                {
                    m._spellLikeAbilitiesBlock.Add(new SpellBlockInfo(info));
                }
            }
            if (TrackedResources != null)
            {
                foreach (ActiveResource tresource in TrackedResources)
                {
                    m.TrackedResources.Add(new ActiveResource(tresource));
                }
            }


            ((BaseDbClass)m).DbLoaderId = ((BaseDbClass)this).DbLoaderId;

            return m;
        }

        public static List<Monster> FromFile(string filename)
        {
            List<Monster> returnMonsters = null;
            try
            {
                if (filename.IsZipFile())
                {
                    returnMonsters = FromHeroLabZip(filename);
                }
                else
                {

                    using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {

                        XDocument doc = XDocument.Parse(new StreamReader(stream).ReadToEnd());


                        //look for herolab file
                        XElement it = doc.Root;

                        if (it.Name == "document")
                        {
                            string sig = it.Attribute("signature").Value;

                            if (sig == "Hero Lab Portfolio")
                            {
                                XElement prod = it.Element("product");

                                if (prod != null)
                                {

                                    int major = 0;
                                    int minor = 0;
                                    int patch = 0;


                                    major = GetAttributeIntValue(prod, "major");
                                    minor = GetAttributeIntValue(prod, "minor");
                                    patch = GetAttributeIntValue(prod, "patch");

                                    if (!CheckVersion(major, minor, patch, 3, 6, 7))
                                    {
                                        throw new MonsterParseException("Combat Manager requires files from a newer version of HeroLab." +
                                            "\r\nUpgrade HeroLab to the newest version, reload the file and save the file.");
                                    }

                                    returnMonsters = FromHeroLabFile(doc);
                                }
                            }
                        }
                        else
                        {
                            //look for PCGen file
                            //"/group-set/groups/group/combatants"
                            XElement el = doc.Root;
                            if (el.Name == "group-set")
                            {
                                el = el.Element("groups");
                                if (el != null)
                                {
                                    el = el.Element("group");
                                    if (el != null)
                                    {

                                        el = el.Element("combatants");
                                    }

                                    if (el != null)
                                    {
                                        returnMonsters = FromPcGenExportFile(doc);
                                    }
                                }
                            }
                        }





                        if (returnMonsters == null)
                        {
                            throw new MonsterParseException("Unrecognized file format");
                        }
                    }


                }
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new MonsterParseException("CombatManager was not able to read the file.", ex);
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new MonsterParseException("CombatManger was not able to understand the file.", ex);
            }
            catch (FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new MonsterParseException("CombatManger was not able to understand the file.", ex);
            }
            catch (XmlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new MonsterParseException("CombatManger was not able to understand the file.", ex);
            }


            return returnMonsters;
        }

        private static bool CheckVersion(int major, int minor, int patch, int checkMajor, int checkMinor, int checkPatch)
        {
            if (major > checkMajor)
            {
                return true;
            }
            else if (major == checkMajor)
            {
                if (minor > checkMinor)
                {
                    return true;
                }
                else if (minor == checkMinor)
                {
                    if (patch >= checkPatch)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        private static List<Monster> FromHeroLabFile(XDocument doc)
        {
            List<Monster> monsters = new List<Monster>();


            //attempt to get the stats block
            foreach (XElement heroElement in doc.Root.Element("portfolio").Elements("hero"))
            {



                Monster monster = new Monster();

                ((BaseMonster)monster).Name = heroElement.Attribute("heroname").Value;


                XElement statblock = heroElement.Element("statblock");

                if (statblock != null)
                {
                    string statsblock = statblock.Value;

                    ImportHeroLabBlock(statsblock, monster);

                    monsters.Add(monster);
                }




            }

            return monsters;

        }



        private static List<Monster> FromHeroLabZip(string filename)
        {
            List<Monster> monsters = new List<Monster>();
            if (filename.IsZipFile())
            {
                return monsters;
            }

            using (var hlFile = ZipFile.OpenRead(filename))
            {
                var txtresult = from currentry in hlFile.Entries
                                where Path.GetDirectoryName(currentry.FullName) == "statblocks_text"
                                where !string.IsNullOrEmpty(currentry.Name)
                                select currentry;
                foreach (var entry in txtresult)
                {
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
#if MONO
                        using (var r = new StreamReader(entry.Open(), Encoding.GetEncoding("utf-8")))
                        {
#else
                        using (var r = new StreamReader(entry.Open(), Encoding.GetEncoding("windows-1252")))
                        {
#endif
                            var block = r.ReadToEnd();
                            var xmlresult = from currentry in hlFile.Entries
                                            where Path.GetDirectoryName(currentry.FullName) == "statblocks_xml"
                                            where currentry.Name == entry.Name.Replace(".txt", ".xml")
                                            select currentry;

                            XDocument doc = null;
                            if (xmlresult.FirstOrDefault() != null)
                            {
                                doc = XDocument.Load(new StreamReader(xmlresult.FirstOrDefault().Open()));
                            }

                            var monster = new Monster();
                            ImportHeroLabBlock(block, doc, monster, true);
                            monsters.Add(monster);
                        }

                    }
                }
            }
            return monsters;
        }

        private static List<Monster> FromPcGenExportFile(XDocument doc)
        {
            List<Monster> monsters = new List<Monster>();


            //attempt to get the stats block

            ///group-set/groups/group/combatants/combatant
            XElement combatant = doc.Root.Element("groups");
            combatant = combatant.Element("group");
            combatant = combatant.Element("combatants");
            combatant = combatant.Element("combatant");
            if (combatant != null)
            {

                Monster monster = new Monster();

                //get name
                ((BaseMonster)monster).Name = combatant.Element("name").Value;

                Match m = null;

                //get type
                XElement it = combatant.Element("fullType");
                if (it != null)
                {
                    Regex regFull = new Regex("(?<align>" + AlignString + ")( )+(?<size>" + SizesString + ")( )*\r?\n?" +
                         "(?<type>" + TypesString + ")", RegexOptions.IgnoreCase);

                    m = regFull.Match(it.Value);

                    if (m.Success)
                    {
                        monster.Alignment = m.Groups["align"].Value;
                        if (monster.Alignment == "NN")
                        {
                            monster.Alignment = "N";
                        }
                        monster.Size = m.Groups["size"].Value;
                        monster.Type = m.Groups["type"].Value;
                    }
                }

                it = combatant;

                //get hp
                monster.Hp = GetElementIntValue(it, "hitPoints");
                string hd = GetElementStringValue(it, "hitDice");

                hd = Regex.Replace(hd, "\\([0-9]+ hp\\)", "");
                hd = Regex.Replace(hd, "\\(|\\)", "");
                hd = "(" + hd.Trim() + ")";
                monster.Hd = hd;

                //get stats
                string abilityScores = GetElementStringValue(it, "fullAbilities");

                string cha = GetElementStringValue(it, "cha");
                Regex regInt = new Regex("(?<val>[0-9]+) ");
                m = regInt.Match(cha);
                int chaInt;
                if (int.TryParse(m.Groups["val"].Value, out chaInt))
                {
                    abilityScores = Regex.Replace(abilityScores, "Cha (?<val>[0-9]+)",
                        delegate (Match ma)
                        {
                            return "Cha " + chaInt;
                        }
                    );
                }

                monster._abilitiyScores = abilityScores;




                monster.Cr = GetElementStringValue(it, "challengeRating");

                monster.Xp = GetCrValue(monster.Cr).ToString();

                monster.Init = GetElementIntValue(it, "init-modifier");

                string ac = GetElementStringValue(it, "fullArmorClass");
                if (ac != null)
                {
                    ac = ac.Replace(":", "");
                    monster.Ac = ac;
                }

                monster.ParseAc();


                monster.Fort = GetElementIntValue(it, "fortSave");
                monster.Ref = GetElementIntValue(it, "reflexSave");
                monster.Will = GetElementIntValue(it, "willSave");

                //load skills
                string skills = GetElementStringValue(it, "skills");
                skills = Regex.Replace(skills, ";( )*\r\n", ", ").Trim().Trim(new char[] { ',' });
                monster._skills = skills;


                //BAB, CMB, CMD
                SizeMods mods = SizeMods.GetMods(SizeMods.GetSize(monster.Size));
                monster.BaseAtk = GetElementIntValue(it, "baseAttack");


                monster.Cmb = CmStringUtilities.PlusFormatNumber(monster.BaseAtk + AbilityBonus(monster.Strength) +
                    mods.Combat);

                monster.Cmd = (monster.BaseAtk + mods.Combat + AbilityBonus(monster.Strength) + AbilityBonus(monster.Dexterity) + 10).ToString();


                string feats = GetElementStringValue(it, "feats");
                if (feats != null)
                {
                    feats = FixPcGenFeatList(feats);
                }
                monster._feats = feats;

                //load and fix speed
                string speed = GetElementStringValue(it, "speed");
                speed = Regex.Replace(speed, "Walk ", "");
                monster.Speed = speed.ToLower();



                //load senses
                string senses = GetElementStringValue(it, "senses").ToLower();

                //fix low light vision
                senses = Regex.Replace(senses, "low-light,", "low-light vision,");
                senses = Regex.Replace(senses, "^ *, perception", "Perception");

                //remove unneeded brackets
                senses = Regex.Replace(senses, "\\((?<val>[0-9]+) ft.\\)", delegate (Match ma)
                    {
                        return ma.Groups["val"].Value + " ft.";
                    }
                );

                //add perception
                Regex regSense = new Regex(", Listen (\\+|-)[0-9]+, Spot (\\+|-)[0-9]+", RegexOptions.IgnoreCase);
                int perception = 0;
                if (monster.SkillValueDictionary.ContainsKey("Perception"))
                {
                    perception = monster.SkillValueDictionary["Perception"].Mod;
                }
                senses = regSense.Replace(senses, "; Perception " + CmStringUtilities.PlusFormatNumber(perception));

                //set senses
                monster.Senses = senses;


                monster.SpecialAttacks = GetElementStringValue(it, "specialAttacks");

                string gear = GetElementStringValue(it, "possessions");
                if (gear != null)
                {
                    gear = Regex.Replace(gear, "\r\n", "");
                    gear = Regex.Replace(gear, "[\\];[]+ *", ", ");
                    monster._gear = gear;
                }


                string attacks = GetElementStringValue(it, "attack");
                attacks = Regex.Replace(attacks, "(\r?\n)|:|(\r)", "");

                List<string> meleeStrings = new List<string>();
                List<string> rangedStrings = new List<string>();

                Regex regAttack = new Regex("(?<weapon>[ ,\\p{L}0-9+]+)( \\((?<sub>[-+ \\p{L}0-9/]+)\\))? (?<bonus>(N/A|([-+/0-9]+))) (?<type>(melee|ranged)) (?<dmg>\\([0-9]+d[0-9]+(\\+[0-9]+)?(/[0-9]+-20)?(/x[0-9]+)?\\))");

                foreach (Match ma in regAttack.Matches(attacks))
                {
                    string bonus = ma.Groups["bonus"].Value;

                    if (bonus == "N/A")
                    {
                        bonus = "+0";
                    }

                    string weaponname = ma.Groups["weapon"].Value.Trim();

                    if (string.Compare(weaponname, "Longbow", true) == 0)
                    {
                        if (ma.Groups["sub"].Success)
                        {
                            string sub = ma.Groups["sub"].Value;

                            if (Regex.Match(sub, "composite", RegexOptions.IgnoreCase).Success)
                            {
                                weaponname = "composite longbow";
                            }
                        }
                    }

                    string attack = weaponname + " " + bonus +
                        " " + ma.Groups["dmg"].Value;

                    if (ma.Groups["type"].Value == "melee")
                    {
                        meleeStrings.Add(attack.Trim());
                    }
                    else
                    {
                        rangedStrings.Add(attack.Trim());
                    }
                }

                string melee = "";

                foreach (string attack in meleeStrings)
                {
                    if (melee.Length > 0)
                    {
                        melee += " or ";
                    }
                    melee += attack;
                }

                monster.Melee = melee;


                string ranged = "";

                foreach (string attack in rangedStrings)
                {
                    if (ranged.Length > 0)
                    {
                        ranged += " or ";
                    }
                    ranged += attack;
                }

                monster.Ranged = ranged;


                monsters.Add(monster);


            }
            return monsters;
        }

 

        public static List<string> MonsterDbFields =>
            new() {
                "Description",
                "Description_Visual",
                "SpecialAbilities",
                "BeforeCombat",
                "DuringCombat",
                "Morale",
                "Gear",
                "OtherGear",
                "Feats",
                "SpecialAttacks",
                "SpellLikeAbilities",
                "SpellsKnown",
                "SpellsPrepared",
                "Skills"};

        static int GetElementIntValue(XElement it, string name)
        {
            int value = 0;
            XElement el = it.Element(name);
            if (el != null)
            {
                try
                {
                    value = int.Parse(el.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            return value;
        }

        static int? GetElementIntNullValue(XElement it, string name)
        {
            int? value = null;
            XElement el = it.Element(name);
            if (el != null && el.Value != "")
            {
                try
                {
                    value = int.Parse(el.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            return value;
        }

        static string GetElementStringValue(XElement it, string name)
        {
            string text = null;
            XElement el = it.Element(name);
            if (el != null)
            {
                text = el.Value;
            }
            return text;
        }

        static int GetAttributeIntValue(XElement it, string name)
        {
            int value = 0;
            XAttribute el = it.Attribute(name);
            if (el != null)
            {
                value = int.Parse(el.Value);
            }
            return value;
        }


        static string GetAttributeStringValue(XElement it, string name)
        {
            string text = null;
            XAttribute el = it.Attribute(name);
            if (el != null)
            {
                text = el.Value;
            }
            return text;
        }

        private static string HeroLabStatRegexString(string stat)
        {
            return StringCapitalizer.Capitalize(stat) + " ([0-9]+/)?(?<" + stat.ToLower() + ">([0-9]+|-))";
        }

        private static void ImportHeroLabBlock(string statsblock, Monster monster, bool readNameBlock = false)
        {
            ImportHeroLabBlock(statsblock, null, monster, readNameBlock);
        }

        private static void ImportHeroLabBlock(string statsblock, XDocument doc, Monster monster,
            bool readNameBlock = false)
        {
            statsblock = statsblock.Replace('×', 'x');
            statsblock = statsblock.Replace("Ã—", "x");
            statsblock = statsblock.Replace("â€“", "-");
            statsblock = statsblock.Replace("â€”", "-");
            statsblock = statsblock.Replace("\n", "\r\n");
            statsblock = statsblock.Replace("\r\r\n", "\r\n");


            if (readNameBlock)
            {

                string name = "";

                Regex nameRegex = new Regex("(--------------------\r\n)?(?<name>.+?)(\t| +)CR");
                Match sm = nameRegex.Match(statsblock);
                if (sm.Success)
                {
                    name = sm.Groups["name"].Value;
                }
                else
                {
                    StringReader reader = new StringReader(statsblock);
                    name = reader.ReadLine();
                    int loc = name.IndexOf('\t');
                    if (loc != -1)
                    {
                        name = name.Substring(0, loc);
                    }
                }


                ((BaseMonster)monster).Name = name;
            }


            //System.Diagnostics.Debug.WriteLine(statsblock);


            string strStatSeparator = ",[ ]+";

            //get stats
            string statsRegStr = HeroLabStatRegexString("Str") + strStatSeparator +
                                 HeroLabStatRegexString("Dex") + strStatSeparator +
                                 HeroLabStatRegexString("Con") + strStatSeparator +
                                 HeroLabStatRegexString("Int") + strStatSeparator +
                                 HeroLabStatRegexString("Wis") + strStatSeparator +
                                 HeroLabStatRegexString("Cha");


            Regex regStats = new Regex(statsRegStr);



            Match m = regStats.Match(statsblock);
            if (m.Success)
            {
                monster.AbilitiyScores = "Str " + m.Groups["str"].Value +
                                         ", Dex " + m.Groups["dex"].Value +
                                         ", Con " + m.Groups["con"].Value +
                                         ", Int " + m.Groups["int"].Value +
                                         ", Wis " + m.Groups["wis"].Value +
                                         ", Cha " + m.Groups["cha"].Value;
            }
            else
            {
                string statCollection = "";
                const string regstatsRegStrength = ("Str ([0-9]+/)?(?<str>([0-9]+|-))");
                const string regstatsRegDexterity = ("Dex ([0-9]+/)?(?<dex>([0-9]+|-))");
                const string regstatsRegConstitution = ("Con ([0-9]+/)?(?<con>([0-9]+|-))");
                const string regstatsRegIntelligence = ("Int ([0-9]+/)?(?<int>([0-9]+|-))");
                const string regstatsRegWisdom = ("Wis ([0-9]+/)?(?<wis>([0-9]+|-))");
                const string regstatsRegCharisma = ("Cha ([0-9]+/)?(?<cha>([0-9]+|-))");

                regStats = new Regex(regstatsRegStrength);
                Match regexMatchStr = regStats.Match(statsblock);
                statCollection = regexMatchStr.Success ? "Str " + regexMatchStr.Groups["str"].Value : "Str -";

                regStats = new Regex(regstatsRegDexterity);
                Match regexMatchDex = regStats.Match(statsblock);
                statCollection = statCollection +
                                 (regexMatchDex.Success ? ", Dex " + regexMatchDex.Groups["dex"].Value : ", Dex -");

                regStats = new Regex(regstatsRegConstitution);
                Match regexMatchCon = regStats.Match(statsblock);
                statCollection = statCollection +
                                 (regexMatchCon.Success ? ", Con " + regexMatchCon.Groups["con"].Value : ", Con -");

                regStats = new Regex(regstatsRegIntelligence);
                Match regexMatchInt = regStats.Match(statsblock);
                statCollection = statCollection +
                                 (regexMatchInt.Success ? ", Int " + regexMatchInt.Groups["int"].Value : ", Int -");

                regStats = new Regex(regstatsRegWisdom);
                Match regexMatchWis = regStats.Match(statsblock);
                statCollection = statCollection +
                                 (regexMatchWis.Success ? ", Wis " + regexMatchWis.Groups["wis"].Value : ", Wis -");

                regStats = new Regex(regstatsRegCharisma);
                Match regexMatchCha = regStats.Match(statsblock);
                statCollection = statCollection +
                                 (regexMatchCha.Success ? ", Cha " + regexMatchCha.Groups["cha"].Value : ", Cha -");

                monster.AbilitiyScores = statCollection;

            }

            XElement characterElement = null;
            if (doc != null)
            {
                characterElement = doc.Element("document").Element("public").Element("character");
            } 

            Regex regCr = new Regex("CR (?<cr>[0-9]+(/[0-9]+)?)\r\n");
            m = regCr.Match(statsblock);
            if (m.Success)
            {
                monster.Cr = m.Groups["cr"].Value;

                monster.Xp = GetCrValue(monster.Cr).ToString();
            }
            else if (doc != null)
            {
                XElement cr = characterElement.Element("challengerating");
                string crText = cr?.Attribute("text")?.Value;
                if (crText != null)
                {
                    monster.Cr = crText.Substring(3);
                }
                XElement xp = characterElement.Element("xpaward");
                monster.Xp = xp?.Attribute("value")?.Value;
            }
            else
            {
                monster.Cr = "0";
                monster._xp = "0";
            }

            

            if (doc != null)
            {
                var reources = doc.Descendants("trackedresource");
                foreach (var resource in reources.Select(r => new ActiveResource
                {
                    Name = r.Attribute("name").Value,
                    Current = int.Parse(r.Attribute("left").Value),
                    Max = int.Parse(r.Attribute("max").Value)
                })) monster.TrackedResources.Add(resource);
            }

            //CN Medium Humanoid (Orc)
            string sizesText = SizesString;

            string typesText = TypesString;



            Regex regeAlSizeType = new Regex("(?<align>" + AlignString + ") (?<size>" + sizesText +
                                             ") (?<type>" + typesText + ") *(\\(|\r\n)");
            m = regeAlSizeType.Match(statsblock);

            if (m.Success)
            {
                monster.Alignment = m.Groups["align"].Value;
                if (monster.Alignment == "NN")
                {
                    monster.Alignment = "N";
                }
                monster.Size = m.Groups["size"].Value;
                monster.Type = m.Groups["type"].Value.ToLower();
            }
            else
            {
                monster.Alignment = "N";
                monster.Size = "Medium";
                monster.Type = "humanoid";
            }
            if (doc != null)
            {

                var charxElement = doc.Element("document").Element("public").Element("character");
                monster.Class = charxElement.Element("classes").Attribute("summary").Value;
                monster.Race = charxElement.Element("race").Attribute("racetext").Value;
                monster.Gender = charxElement.Element("personal").Attribute("gender")?.Value;
            }

            //init, senses, perception

            //Init +7; Senses Darkvision (60 feet); Perception +2
            Regex regSense = new Regex("Init (?<init>(\\+|-)[0-9]+)(/(?<dualinit>(\\+|-)[0-9]+), dual initiative)?(; Senses )((?<senses>.+)(;|,) )?Perception (?<perception>(\\+|-)[0-9]+)");
            m = regSense.Match(statsblock);

            if (m.Groups["init"].Success)
            {
                monster.Init = int.Parse(m.Groups["init"].Value);
            }
            else
            {
                monster.Init = 0;
            }
            if (m.Groups["dualinit"].Success)
            {
                monster.DualInit = int.Parse(m.Groups["dualinit"].Value);
            }
            else
            {
                monster.DualInit = null;
            }
            monster.Senses = "";
            if (m.Groups["senses"].Success)
            {
                monster.Senses += m.Groups["senses"].Value + "; ";
            }
            int perception = 0;

            if (m.Groups["perception"].Success)
            {
                perception = int.Parse(m.Groups["perception"].Value);
            }

            monster.Senses += "Perception " + CmStringUtilities.PlusFormatNumber(perception);

            Regex regArmor = new Regex("(?<ac>AC -?[0-9]+, touch -?[0-9]+, flat-footed -?[0-9]+) +(?<mods>\\([-\\p{L}0-9, +]+\\))?", RegexOptions.IgnoreCase);
            m = regArmor.Match(statsblock);
            monster.Ac = m.Groups["ac"].Value;
            if (m.Groups["mods"].Success)
            {
                monster.AcMods = m.Groups["mods"].Value;
            }
            else
            {
                monster.AcMods = "";
            }

            Regex regHp = new Regex(@"(hp (?<hp>[0-9]+)) ((?<hd>\([-+0-9d]+\))|(\(\))|\(\d+ HD; (?<hd>[-+0-9d]+)\))");

            m = regHp.Match(statsblock);
            if (m.Groups["hp"].Success)
            {
                monster.Hp = int.Parse(m.Groups["hp"].Value);
            }
            else
            {
                monster.Hp = 0;
            }
            if (m.Groups["hd"].Success)
            {
                monster.Hd = m.Groups["hd"].Value;
            }
            else
            {
                monster.Hd = "0d0";
            }

            Regex regSave = new Regex("Fort (?<fort>[-+0-9]+)( \\([-+0-9]+bonus vs. [- \\p{L}]+\\))?, Ref (?<ref>[-+0-9]+), Will (?<will>[-+0-9]+)");
            m = regSave.Match(statsblock);
            if (m.Success)
            {
                monster.Fort = int.Parse(m.Groups["fort"].Value);
                monster.Ref = int.Parse(m.Groups["ref"].Value);
                monster.Will = int.Parse(m.Groups["will"].Value);
            }
            else
            {
                monster.Fort = 0;
                monster.Ref = 0;
                monster.Will = 0;
            }

            int defStart = m.Index + m.Length;

            Regex endLine = new Regex("(?<line>.+)");
            m = endLine.Match(statsblock, defStart + 1);
            string defLine = m.Value;

            //string da = FixHeroLabDefensiveAbilities(GetLine("Defensive Abilities", statsblock, true));
            monster.DefensiveAbilities = GetItem("Defensive Abilities", defLine, true);
            monster.Resist = GetItem("Resist", defLine, false);
            monster.Immune = GetItem("Immune", defLine, false);
            monster.Sr = GetItem("SR", defLine, false);

            /*Regex regResist = new Regex("(?<da>.+); Resist (?<resist>.+)");

            if (da != null)
            {
                m = regResist.Match(da);

                if (m.Success)
                {
                    monster.DefensiveAbilities = m.Groups["da"].Value;
                    monster.Resist = m.Groups["resist"].Value;
                }
                else
                {
                    monster.DefensiveAbilities = da;
                }
            }*/

            monster.Weaknesses = GetLine("Weaknesses", statsblock, false);


            monster.Speed = GetLine("Spd", statsblock, false);
            if (monster.Speed == null)
            {

                monster.Speed = GetLine("Speed", statsblock, false);
            }


            monster.SpecialAttacks = GetLine("Special Attacks", statsblock, true);

            Regex regBase = new Regex("Base Atk (?<bab>[-+0-9]+); CMB (?<cmb>[^;]+); CMD (?<cmd>.+)");
            m = regBase.Match(statsblock);

            if (m.Success)
            {
                monster.BaseAtk = int.Parse(m.Groups["bab"].Value);
                monster.Cmb = m.Groups["cmb"].Value;
                monster.Cmd = m.Groups["cmd"].Value;
            }
            else
            {
                monster.BaseAtk = 0;
                monster.Cmb = "+0";
                monster.Cmd = "10";
            }

            monster.Feats = FixHeroLabFeats(GetLine("Feats", statsblock, true));

            monster.Skills = GetLine("Skills", statsblock, true);

            monster.Languages = GetLine("Languages", statsblock, false);

            monster.Sq = GetLine("SQ", statsblock, true);
            if (monster.Sq != null)
            {
                monster.Sq = monster.Sq.ToLower();
            }
            monster.Gear = GetLine("Gear", statsblock, true);
            if (monster.Gear == null)
            {
                monster.Gear = GetLine("Combat Gear", statsblock, true);
            }
            string space = GetLine("Space", statsblock, true);
            if (space != null)
            {
                m = Regex.Match(space, "(?<space>[0-9]+ ?ft\\.)[;,] +Reach +(?<reach>[0-9]+ ?ft\\.)");
                if (m.Success)
                {
                    monster.Space = m.Groups["space"].Value;
                    monster.Reach = m.Groups["reach"].Value;
                }
            }




            Regex regLine = new Regex("(?<text>.+)\r\n");

            Regex regSpecial = new Regex("(SPECIAL ABILITIES|Special Abilities)\r\n(-+)\r\n(?<special>(.|\r|\n)+)((Created With Hero Lab)|(Hero Lab))");
            m = regSpecial.Match(statsblock);
            if (m.Success)
            {
                string special = m.Groups["special"].Value;

                //parse special string
                //fix template text
                special = Regex.Replace(special, "Granted by [- \\p{L}]+ heritage\\.\r\n\r\n", "");

                MatchCollection matches = regLine.Matches(special);

                Regex regWeaponTraining = new Regex("Weapon Training: (?<group>[\\p{L}]+) \\+(?<value>[0-9]+)");
                Regex regSr = new Regex("Spell Resistance \\((?<SR>[0-9]+)\\)");
                Regex regSpecialAbility = new Regex("(?<name>[-\\.+, \\p{L}0-9:]+)"
                    + "( \\((?<mod>[-+][0-9]+)\\))?"
                    + "( [0-9]+')?"
                    + "( \\(CMB (?<CMB>[0-9]+)\\))?"
                    + "( \\((?<AtWill>At will)\\))?"
                    + "( \\((?<daily>[0-9]+)/day\\))?"
                    + "( \\(DC (?<DC>[0-9]+)\\))?"
                    + "( \\((?<othertext>[0-9\\p{L}, /]+)\\))?"
                    + " \\((?<type>(Ex|Su|Sp)(, (?<cp>[0-9]+) CP)?)\\) (?<text>.+)");

                foreach (Match ma in matches)
                {
                    string text = ma.Groups["text"].Value;

                    //check for weapon training
                    Match lm = regWeaponTraining.Match(text);

                    if (lm.Success)
                    {
                        string group = lm.Groups["group"].Value;
                        int val = int.Parse(lm.Groups["value"].Value);

                        monster.AddWeaponTraining(group, val);
                    }
                    if (!lm.Success)
                    {
                        lm = regSr.Match(text);

                        if (lm.Success)
                        {
                            monster.Sr = lm.Groups["SR"].Value;

                        }
                    }
                    if (!lm.Success)
                    {
                        lm = regSpecialAbility.Match(text);

                        if (lm.Success)
                        {
                            SpecialAbility sa = new SpecialAbility();
                            sa.Name = lm.Groups["name"].Value;
                            sa.Type = lm.Groups["type"].Value;
                            sa.Text = lm.Groups["text"].Value;

                            if (lm.Groups["AtWill"].Success)
                            {
                                sa.Text += " (at will)";
                            }
                            else if (lm.Groups["Daily"].Success)
                            {
                                sa.Text += " (" + lm.Groups["Daily"].Value + "/day)";
                            }

                            if (lm.Groups["DC"].Success)
                            {
                                sa.Text += " (DC " + lm.Groups["DC"].Value + ")";
                            }


                            monster.SpecialAbilitiesList.Add(sa);

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(text);
                        }

                    }


                }
            }

            string endAttacks = "[\\p{L}()]+ Spells (Known|Prepared)|Special Attacks|([\\p{L}( )]+)?\\s?Spell-Like Abilities|-------|Space [0-9]";

            Regex regMelee = new Regex("\r\nMelee (?<melee>(.|\r|\n)+?)\r\n(Ranged|" + endAttacks + ")");

            m = regMelee.Match(statsblock);

            if (m.Success)
            {
                string attacks = m.Groups["melee"].Value;

                monster.Melee = FixHeroLabAttackString(attacks);

            }

            Regex regRanged = new Regex(
                "\r\nRanged (?<ranged>(.|\r|\n)+?)\r\n(" + endAttacks + ")");

            m = regRanged.Match(statsblock);

            if (m.Success)
            {
                string attacks = m.Groups["ranged"].Value;

                monster.Ranged = FixHeroLabAttackString(attacks);
            }

            MatchCollection mc = Regex.Matches(statsblock, @"(?<SLA>([\p{L}( )]+)?\s?Spell-Like Abilities (.|\r|\n)+?)(?=([\p{L}( )]+)?\s?Spells (Known|Prepared)|\r\n------| D Domain)");
            foreach (var collection in mc)
            {
                monster.SpellLikeAbilities = FixSpellString(collection.ToString().Trim());
            }

            Regex regSpells = new Regex(
                @"(?<spells>([\p{L}( )]+)?\s?Spells Known (.|\r|\n)+?)(?=([\p{L}( )]+)?\s?Spells Prepared|\r\n------| D Domain|Bloodline)");

            m = regSpells.Match(statsblock);
            if (m.Success)
            {
                string spells = m.Groups["spells"].Value;

                spells = FixSpellString(spells);

                monster.SpellsKnown = spells;
            }

            Regex regSpellsPrepared = new Regex(
                "\r\n(?<spells>\\w+? Spells Prepared (.|\r|\n)+?)(\r\n------| D Domain)");

            m = regSpellsPrepared.Match(statsblock);
            if (m.Success)
            {
                string spells = m.Groups["spells"].Value;

                spells = FixSpellString(spells);

                monster.SpellsPrepared = spells;

            }
            Regex regDomains = new Regex(@" D Domain spell; Domains (?<Domains>(.+?))(?=\r\n|\r\n------)");
            m = regDomains.Match(statsblock);
            if (m.Success)
            {
                monster.SpellDomains = m.Groups["Domains"].Value;
            }
            Regex regBloodline = new Regex(@" Bloodline (?<Bloodline>(.+?))(?=\r\n|\r\n------)");
            m = regBloodline.Match(statsblock);
            if (m.Success)
            {
                monster._bloodline = m.Groups["Bloodline"].Value;
            }

            if (characterElement != null)
            {

                var dr = characterElement.Element("damagereduction");
                if (dr != null)
                {
                    foreach (var drSpecial in dr.Elements("special"))
                    {
                        string drval = drSpecial.Attribute("shortname")?.Value;
                        string drtype;
                        int dramount;
                        if (FindDr(drval, out drtype, out dramount))

                        {
                            monster.Dr = AddDr(monster.Dr, drtype, dramount);

                        }
                    }
                }

            }
        }

        static private bool FindDr(string drval, out string drtype, out int val)
        {
            drtype = null;
            val = 0;
            bool success = false;
            if (drval != null)
            {
                int slash = drval.IndexOf('/');
                if (slash != -1)
                {
                    try
                    {
                        string [] splitstr = drval.Split('/');
                        string valstr = splitstr[0];
                        drtype = splitstr[1];
                        val = int.Parse(valstr);
                        success = true;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return success;
        }

        private static string FixSpellString(string spells)
        {
            spells = spells.Replace('—', '-');
            spells = spells.Replace("):", ")");
            spells = spells.Replace("\r\n", " ");

            //remove Sources
            spells = Regex.Replace(spells, @"\[\D+\]", "");
            return spells;

        }

        private static string TypesString
        {
            get
            {
                string typesText = "";
                bool firstType = true;
                foreach (string name in _creatureTypeNames.Values)
                {
                    if (!firstType)
                    {
                        typesText += "|";
                    }
                    else
                    {
                        firstType = false;
                    }

                    typesText += "(" + StringCapitalizer.Capitalize(name)
                      + "|" + name + ")";
                }

                return typesText;
            }
        }

        private static string SizesString
        {
            get
            {
                string sizesText = "";
                bool firstSize = true;
                foreach (MonsterSize size in Enum.GetValues(typeof(MonsterSize)))
                {
                    if (!firstSize)
                    {
                        sizesText += "|";
                    }
                    else
                    {
                        firstSize = false;
                    }

                    sizesText += "(" + SizeMods.GetSizeText(size)
                      + ")";
                }
                return sizesText;
            }
        }

        private static string AlignString => "(L|C|N)(G|E|N)?";

        private static string FixHeroLabAttackString(string text)
        {
            string attacks = text;

            string newAttacks = "";

            foreach (char c in attacks)
            {
                if (c == 65533 || c == '×')
                {
                    newAttacks += 'x';
                }
                else
                {
                    newAttacks += c;
                }
            }

            attacks = newAttacks;

            attacks = Weapon.ReplaceOriginalWeaponNames(attacks, false);

            attacks = attacks.ToLower();

            attacks = Regex.Replace(attacks, "and\r\n  ", "or");
            attacks = Regex.Replace(attacks, "or\r\n  ", "or");
            attacks = Regex.Replace(attacks, "/20/", "/");
            attacks = Regex.Replace(attacks, "/20\\)", ")");
            attacks = Regex.Replace(attacks, " \\(from armor\\)", "");




            return attacks;
        }

        private static string FixHeroLabFeats(string text)
        {

            string returnText = text;

            if (returnText != null)
            {
                returnText = Regex.Replace(returnText, " ([-+][0-9]+)(/[-+][0-9]+)*", "");

                returnText = Regex.Replace(returnText, " \\([0-9]+d[0-9]+\\)", "");
                returnText = Regex.Replace(returnText, " \\([0-9]+/day\\)", "");
            }

            return returnText;
        }

        private static string FixHeroLabDefensiveAbilities(string text)
        {
            string returnText = text;

            if (returnText != null)
            {
                returnText = Regex.Replace(returnText, " \\(Lv >=[0-9]+\\)", "");
            }

            return returnText;
        }

        private static string GetLine(string start, string text, bool bFix)
        {
            string returnText = null;

            Regex regLine = new Regex(start + " *(?<line>.+)");
            Match m = regLine.Match(text);

            if (m.Success)
            {
                returnText = m.Groups["line"].Value;
                returnText = returnText.Trim();

                if (bFix)
                {
                    returnText = FixHeroLabList(returnText);
                }
            }

            return returnText;

        }

        public static string GetItem(string start, string text, bool bFix)
        {
            string returnText = null;

            Regex regLine = new Regex("(; |^)" + start + " *(?<line>.+?)(;|$)");
            Match m = regLine.Match(text);

            if (m.Success)
            {
                returnText = m.Groups["line"].Value;
                returnText = returnText.Trim();

                if (bFix)
                {
                    returnText = FixHeroLabList(returnText);
                }
            }

            return returnText;
        }

        private static string FixHeroLabList(string text)
        {

            string returnText = text;

            if (returnText != null)
            {
                returnText = ReplaceHeroLabSpecialChar(returnText);
                returnText = Weapon.ReplaceOriginalWeaponNames(returnText, false);
                returnText = ReplaceColonItems(returnText);

            }


            return returnText;
        }

        private static string ReplaceHeroLabSpecialChar(string text)
        {
            string returnText = text;

            if (returnText != null)
            {
                returnText = returnText.Replace("&#151;", "-");
            }

            return returnText;

        }

        private static string ReplaceColonItems(string text)
        {
            Regex reg = new Regex(": (?<val>[-+/. \\p{L}0-9]+?)(?<mod> (\\+|-)[0-9]+)?((?<comma>,)|\r|\n|$)");

            return reg.Replace(text, delegate (Match m)
            {
                string val = " (" + m.Groups["val"] + ")";

                if (m.Groups["mod"].Success)
                {
                    val += m.Groups["mod"].Value;
                }


                if (m.Groups["comma"].Success)
                {
                    val += ",";
                }

                return val;
            });

        }

        private static string FixPcGenFeatList(string text)
        {
            string val = text;
                //TODO: fix this
                throw new NotImplementedException("FixPcGenFeatList broke");
            //foreach (KeyValuePair<string, string> pair in Feat.AltFeatMap)
            //{
            //    val = Regex.Replace(val, pair.Key, pair.Value);
            //}

            //return val;
        }

        //protected

        void ParseSpecialAbilities()
        {
            if (_specialAbilitiesList != null && _specialAbilitiesList.Count > 0)
            {
                _specialAblitiesParsed = true;
            }
            else
            {
                if (_specialAbilitiesList == null)
                {
                    _specialAbilitiesList = new ObservableCollection<SpecialAbility>();
                    _specialAbilitiesList.CollectionChanged += SpecialAbilitiesList_CollectionChanged;

                }
                if (_specialAbilities != null)
                {
                    string specText = _specialAbilities;

                    //trim off extra dragon info at end
                    Regex dragonTableRegex = new Regex("Age Category S pecial Abilities");
                    Match dragonMatch = dragonTableRegex.Match(specText);
                    if (dragonMatch.Success)
                    {
                        specText = specText.Substring(0, dragonMatch.Index);
                    }


                    List<string> list = new List<string>();
                    string specRegString = "\\((?<type>(Ex|Su|Sp))(, (?<cp>[0-9]+) CP)?\\):?";
                    Regex specFindRegex = new Regex("((?<startline>^)|\\.)[-\\p{L} ',]+" + specRegString);
                    Regex specRegex = new Regex(specRegString);
                    Match specFindMatch = specFindRegex.Match(specText);
                    List<int> locList = new List<int>();

                    while (specFindMatch.Success)
                    {
                        int index = specFindMatch.Index;
                        if (!specFindMatch.GroupSuccess("startline"))
                        {
                            index++;
                        }
                        locList.Add(index);

                        specFindMatch = specFindMatch.NextMatch();
                    }

                    for (int i = 0; i < locList.Count; i++)
                    {
                        if (i + 1 == locList.Count)
                        {
                            list.Add(specText.Substring(locList[i], specText.Length - locList[i]).Trim());
                        }
                        else
                        {

                            list.Add(specText.Substring(locList[i], locList[i + 1] - locList[i]).Trim());
                        }
                    }

                    foreach (string strSpec in list)
                    {
                        Match match = specRegex.Match(strSpec);

                        SpecialAbility spec = new SpecialAbility();

                        if (match.Success)
                        {
                            spec.Name = strSpec.Substring(0, match.Index).Trim();
                            spec.Type = match.Groups["type"].Value;
                            if (match.Groups["cp"].Success)
                            {
                                spec.ConstructionPoints = int.Parse(match.Groups["cp"].Value);
                            }
                            spec.Text = strSpec.Substring(match.Index + match.Length).Trim();
                        }
                        else
                        {
                            spec.Name = "";
                            spec.Type = "";
                            spec.Text = strSpec;
                        }


                        _specialAbilitiesList.Add(spec);

                    }
                    SpecialAblitiesParsed = true;
                }
            }

        }

        void SpecialAbilitiesList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SpecialAblitiesParsed = true;
            _usableConditionsParsed = false;
        }

        void ParseUsableConditions()
        {
            _usableConditions.Clear();
            foreach (SpecialAbility sa in SpecialAbilitiesList)
            {
                if (sa.Name == "Disease" || sa.Name == "Poison")
                {
                    Affliction a = Affliction.FromSpecialAbility(this, sa);


                    if (a != null)
                    {

                        Condition c = new Condition();
                        c.Name = a.Name;
                        c.Affliction = a;
                        if (sa.Name == "Disease")
                        {
                            c.Image = "disease";
                        }
                        else
                        {
                            c.Image = "poison";
                        }


                        _usableConditions.Add(c);
                    }

                }
            }
            _usableConditionsParsed = true;
        }

        void ParseAc()
        {
            Regex regExAc = new Regex("AC [0-9]+\\,");

            //match AC
            Match match = regExAc.Match(_ac);
            if (match.Success)
            {
                string acText = match.Value.Substring(3, match.Length - 4);

                int.TryParse(acText, out _fullAc);

            }
            else
            {
                regExAc = new Regex("^[0-9]+\\,");

                match = regExAc.Match(_ac);

                if (match.Success)
                {
                    string acText = match.Value.Substring(0, match.Length - 1);

                    int.TryParse(acText, out _fullAc);

                }
            }

            //match flat footed
            Regex regTouch = new Regex("ouch [0-9]+\\,");

            match = regTouch.Match(_ac);

            if (match.Success)
            {
                string text = match.Value.Substring(5, match.Length - 6);
                int.TryParse(text, out _touchAc);
            }


            //match flat footed
            Regex regFlatFooted = new Regex("ooted [0-9]+");

            match = regFlatFooted.Match(_ac);

            if (match.Success)
            {
                string flatText = match.Value.Substring(6);
                int.TryParse(flatText, out _flatFootedAc);
            }

            _naturalArmor = FindModifierNumber(_acMods, "natural");

            _shield = FindModifierNumber(_acMods, "shield");
            _armor = FindModifierNumber(_acMods, "armor");
            _dodge = FindModifierNumber(_acMods, "dodge");
            _deflection = FindModifierNumber(_acMods, "deflection");

            _acParsed = true;

        }

        private void ParseSkills()
        {
            _skillValueDictionary = new SortedDictionary<string, SkillValue>(StringComparer.OrdinalIgnoreCase);
            if (Skills != null)
            {
                Regex skillReg = new Regex("([ \\p{L}]+)( )(\\(([- \\p{L}]+)\\) )?((\\+|-)[0-9]+)");

                foreach (Match match in skillReg.Matches(_skills))
                {
                    SkillValue value = new SkillValue();


                    value.Name = match.Groups[1].Value.Trim();


                    if (match.Groups[3].Success)
                    {
                        value.Subtype = match.Groups[4].Value;

                    }

                    value.Mod = int.Parse(match.Groups[5].Value);

                    _skillValueDictionary[value.FullName] = value;


                }
            }

            _skillsParsed = true;
        }

        private void ParseFeats()
        {
            if (_featsList != null && _featsList.Count > 0)
            {
                FeatsParsed = true;
            }
            else
            {

                _featsList = new List<string>();

                if (Feats != null)
                {

                    Regex regFeat = new Regex("(^| )([- \\p{L}0-9]+( \\([- ,\\p{L}0-9]+\\))?)(\\*+)?(\\Z|,)");

                    foreach (Match m in regFeat.Matches(Feats))
                    {
                        string val = m.Groups[2].Value;

                        //remove B end of feat error
                        //hopefully no feat ever ends in a capital B
                        if (val[val.Length - 1] == 'B')
                        {
                            val = val.Substring(0, val.Length - 1);
                        }

                        _featsList.Add(val);



                    }
                    _featsParsed = true;
                }

                _featsList.Sort();

            }


        }

        private static int FindModifierNumber(string text, string modifierName)
        {
            int value = 0;
            if (text != null)
            {
                Regex regName = new Regex("(\\+|-)[0-9]+ " + modifierName);

                Match match = regName.Match(text);

                if (match.Success)
                {
                    string valText = match.Value;

                    Regex regSpace = new Regex(" ");
                    Match spaceMatch = regSpace.Match(valText);

                    string numText = valText.Substring(0, spaceMatch.Index);

                    int.TryParse(numText, out value);

                }
            }

            return value;
        }

        private static string ReplaceModifierNumber(string text, string modifierName, int value, bool addIfZero)
        {
            string valText = CmStringUtilities.PlusFormatNumber(value) + " " + modifierName;

            if (value == 0 && !addIfZero)
            {
                valText = "";
            }


            Regex regName = new Regex("(, )?(\\+|-)[0-9]+ " + modifierName + "(, )?", RegexOptions.IgnoreCase);

            string returnText = text;
            if (text != null || value != 0)
            {
                if (text == null)
                {
                    text = "";
                }

                Match match = regName.Match(text);
                if (match.Success)
                {
                    if (value == 0 && !addIfZero)
                    {
                        if (match.Groups[1].Success && match.Groups[3].Success)
                        {
                            valText = ", ";
                        }
                    }
                    else
                    {
                        valText = match.Groups[1] + valText + match.Groups[3];
                    }

                    returnText = regName.Replace(text, valText);
                }
                else
                {
                    if (text.Length == 0)
                    {
                        returnText = "(" + valText + ")";
                    }
                    else if (valText.Length > 0)
                    {
                        returnText = text.Insert(text.Length - 1, ", " + valText);
                    }
                }
            }

            return returnText != "()" ? returnText : "";
        }

        private static string ChangeCmd(string text, int diff)
        {
            string returnText = ChangeStartingNumber(text, diff);


            if (returnText != null)
            {
                Regex regVs = new Regex("([-\\+0-9/]+)( vs\\.)");

                returnText = regVs.Replace(returnText, delegate (Match m)
                                {
                                    int val = int.Parse(m.Groups[1].Value) + diff;

                                    return val.ToString() + m.Groups[2];
                                }
                            );
            }


            return returnText;
        }

        private string ChangeThrownDamage(string text, int diff, int diffPlusHalf)
        {
            if (text == null)
            {
                return null;
            }

            string returnText = text;

            foreach (KeyValuePair<string, bool> thrownAttack in _thrownAttacks)
            {


                Regex regAttack = new Regex(ViewModels.Attack.RegexString(thrownAttack.Key), RegexOptions.IgnoreCase);

                returnText = regAttack.Replace(returnText, delegate (Match m)
                {
                    ViewModels.Attack info = ViewModels.Attack.ParseAttack(m);

                    if (!info.AltDamage)
                    {
                        if (thrownAttack.Value)
                        {
                            info.Damage.Mod += diffPlusHalf;
                        }
                        else
                        {
                            info.Damage.Mod += diff;
                        }
                    }

                    return info.Text;

                });



            }


            return returnText;
        }

        public int GetSkillMod(string skill, string subtype)
        {


            SkillValue val = new SkillValue(skill);
            val.Subtype = subtype;

            if (SkillValueDictionary.ContainsKey(val.FullName))
            {
                return SkillValueDictionary[val.FullName].Mod;
            }

            else
            {
                Stat stat;
                if (SkillsList.TryGetValue(val.Name, out stat))
                {
                    return AbilityBonus(GetStat(stat));
                }
                else
                {
                    return 0;
                }

            }
        }

        public bool AddOrChangeSkill(string skill, int diff)
        {
            return AddOrChangeSkill(skill, null, diff);
        }

        public bool AddOrChangeSkill(string skill, string subtype, int diff)
        {
            bool added = false;


            SkillValue val = new SkillValue(skill);
            val.Subtype = subtype;

            if (SkillValueDictionary.ContainsKey(val.FullName))
            {
                ChangeSkill(val.FullName, diff);
            }
            else
            {

                val.Mod = diff;

                Stat stat;
                if (SkillsList.TryGetValue(val.Name, out stat))
                {
                    val.Mod += AbilityBonus(GetStat(stat));
                }

                SkillValueDictionary[val.FullName] = val;

                UpdateSkillFields(val);

                added = true;

            }



            return added;
        }

        public bool ChangeSkill(string skill, int diff)
        {

            SkillValue value;

            if (SkillValueDictionary.TryGetValue(skill, out value))
            {
                value.Mod += diff;

                UpdateSkillFields(value);
                return true;
            }
            else
            {
                return false;
            }

        }

        private static string SetSkillStringMod(string text, string skill, int val)
        {

            string returnText = text;

            Regex regName = new Regex(skill + " (\\([-\\p{L} ]+\\) )?(\\+|-)[0-9]+");

            Match match = regName.Match(text);
            if (match.Success)
            {
                Regex regMod = new Regex("(\\+|-)[0-9]+");

                Match modMatch = regMod.Match(match.Value);

                int newVal = val;

                string newText = CmStringUtilities.PlusFormatNumber(newVal);

                returnText = returnText.Remove(match.Index + modMatch.Index, modMatch.Length);
                returnText = returnText.Insert(match.Index + modMatch.Index, newText);
            }

            return returnText;
        }

        private static string ChangeSkillStringMod(string text, string skill, int diff)
        {
            return ChangeSkillStringMod(text, skill, diff, false);
        }

        private static string ChangeSkillStringMod(string text, string skill, int diff, bool add)
        {

            string returnText = text;

            Regex regName = new Regex(skill + " (\\([-\\p{L} ]+\\) )?(\\+|-)[0-9]+");

            bool added = false;

            if (returnText != null)
            {
                Match match = regName.Match(returnText);
                if (match.Success)
                {
                    Regex regMod = new Regex("(\\+|-)[0-9]+");

                    Match modMatch = regMod.Match(match.Value);

                    int oldVal = int.Parse(modMatch.Value);
                    int newVal = oldVal + diff;

                    string newText = CmStringUtilities.PlusFormatNumber(newVal);

                    returnText = returnText.Remove(match.Index + modMatch.Index, modMatch.Length);
                    returnText = returnText.Insert(match.Index + modMatch.Index, newText);
                    added = true;
                }
            }

            if (add && !added)
            {
                returnText = AddToStringList(returnText, skill + " " + CmStringUtilities.PlusFormatNumber(diff));
            }

            return returnText;
        }

        private void AddRacialSkillBonus(string type, int diff)
        {
            RacialMods = AddPlusModValue(RacialMods, type, diff);

            AddOrChangeSkill(type, diff);
        }

        private static string AddPlusModValue(string text, string type, int diff)
        {
            string returnText = text;
            if (returnText == null)
            {
                returnText = "";
            }

            Regex regVal = new Regex("((\\+|-)[0-9]+) " + type);

            bool replaced = false;

            returnText = regVal.Replace(returnText, delegate (Match m)
                {
                    int val = int.Parse(m.Groups[1].Value);

                    val += diff;
                    replaced = true;

                    return CmStringUtilities.PlusFormatNumber(val) + " " + type;
                });

            if (!replaced)
            {
                returnText = AddToStringList(returnText,
                    CmStringUtilities.PlusFormatNumber(diff) + " " + type);
            }

            return returnText;

        }

        protected void UpdateSkillFields(SkillValue skill)
        {
            if (skill.Name.CompareTo("Perception") == 0)
            {
                //Adjust perception
                _senses = SetSkillStringMod(Senses, skill.FullName, skill.Mod);
            }
        }

        public override int GetStartingHp(HpMode mode)
        {
            int outhp;

            if (mode == HpMode.Default || !TryParseHp(mode == HpMode.Max, out outhp))
            {
               return outhp = Hp;
            }
            return outhp;
        }

        public bool TryParseHp(bool max, out int hp)
        {
            hp = 0;

            DieRoll dr = DieRoll.FromString(Hd);
            if (dr != null)
            {
                if (max)
                {
                    hp = dr.Max;
                }
                else
                {
                    hp = dr.Roll().Total;
                }
                return true;
            }

            return false;
        }


        public override IEnumerable<ActiveResource> LoadResources()
        {
            List<ActiveResource> res = new List<ActiveResource>();

            if (SpecialAttacks != null)
            {
                //find rage
                Match m = Regex.Match(SpecialAttacks, "[Rr]age \\((?<count>[0-9]+) rounds?/ ?day\\)");
                if (m.Success)
                {
                    int count = int.Parse(m.Groups["count"].Value);
                    ActiveResource r = new ActiveResource() { Name = "Rage", Max = count, Current = count, Uses = count + " rounds/day" };
                    res.Add(r);
                }


            }

            if (Sq != null)
            {
                //find rage
                Match m = Regex.Match(Sq, "[Kk]i [Pp]ool \\((?<count>[0-9]+) points?,");
                if (m.Success)
                {
                    int count = int.Parse(m.Groups["count"].Value);
                    ActiveResource r = new ActiveResource() { Name = "Ki pool", Max = count, Current = count };
                    res.Add(r);
                }
            }

            return res;
        }


        public override void ApplyDefaultConditions()
        {
            if (HasDefensiveAbility("Incorporeal") && FindCondition("Incorporeal") == null)
            {
                ActiveCondition ac = new ActiveCondition();
                ac.Condition = Condition.FindCondition("Incorporeal");
                AddCondition(ac);
            }
        }


        private static string ChangeStartingNumber(string text, int diff)
        {
            string returnText = text;
            if (text != null)
            {

                Regex regName = new Regex("^-?[0-9]+");

                Match match = regName.Match(returnText);

                if (match.Success)
                {
                    int val = int.Parse(match.Value);

                    val += diff;

                    returnText = regName.Replace(returnText, val.ToString(), 1);
                }
            }

            return returnText;
        }

        private static int GetStartingNumber(string text)
        {
            int val = 0;

            if (text != null)
            {
                Regex regName = new Regex("^-?[0-9]+");

                Match match = regName.Match(text);

                if (match.Success)
                {
                    val = int.Parse(match.Value);
                }
            }

            return val;
        }

        private static string AddSr(string text, int value)
        {
            string returnText = text;

            if (returnText == null || returnText.Length == 0)
            {
                returnText = value.ToString();
            }
            else
            {
                returnText = ChangeStartingNumberMaxValue(text, value);
            }

            return returnText;
        }

        private static string ChangeStartingNumberMaxValue(string text, int value)
        {
            string returnText = text;

            Regex regName = new Regex("^-?[0-9]+");

            returnText = regName.Replace(returnText, delegate (Match m)
                {

                    int val = int.Parse(m.Value);

                    return Math.Max(val, value).ToString();
                }
                , 1);

            return returnText;
        }


        public static string ChangeStartingModOrVal(string text, int diff)
        {
            string returnText = text;

            if (text != null)
            {

                Regex regName = new Regex("^(\\+|-)?[0-9]+");

                Match match = regName.Match(returnText);

                if (match.Success)
                {
                    int val = int.Parse(match.Value);

                    val += diff;

                    returnText = regName.Replace(returnText, CmStringUtilities.PlusFormatNumber(val), 1);
                }
            }

            return returnText;
        }

        private static int GetStartingModOrVal(string text)
        {
            int val = 0;
            if (text != null)
            {
                Regex regName = new Regex("^(\\+|-)?[0-9]+");

                Match match = regName.Match(text);

                if (match.Success)
                {
                    val = int.Parse(match.Value);
                }
            }


            return val;
        }

        private static int GetStartingMod(string text)
        {
            int val = 0;
            if (text != null)
            {
                Regex regName = new Regex("^(\\+|-)[0-9]+");

                Match match = regName.Match(text);

                if (match.Success)
                {
                    val = int.Parse(match.Value);
                }
            }


            return val;
        }
        #region Templates
        public bool MakeAdvanced()
        {
            AdjustNaturalArmor(2);
            AdjustStrength(4);
            AdjustDexterity(4);
            AdjustConstitution(4);
            AdjustIntelligence(4);
            AdjustWisdom(4);
            AdjustCharisma(4);
            AdjustCr(1);

            return true;
        }

        public bool MakeGiant()
        {
            if (SizeMods.GetSize(Size) != MonsterSize.Colossal)
            {

                AdjustSize(1);
                AdjustNaturalArmor(3);
                AdjustStrength(4);
                AdjustConstitution(4);
                AdjustDexterity(-2);

                AdjustCr(1);

                return true;
            }

            return false;
        }

        public bool MakeCelestial()
        {
            return MakeSummoned(HalfOutsiderType.Celestial);
        }

        public bool MakeFiendish()
        {

            return MakeSummoned(HalfOutsiderType.Fiendish);
        }

        public bool MakeEntropic()
        {
            return MakeSummoned(HalfOutsiderType.Entropic);
        }

        public bool MakeResolute()
        {

            return MakeSummoned(HalfOutsiderType.Resolute);
        }

        public static string GetOutsiderOpposedType(HalfOutsiderType type)
        {
            string opType = "";

            switch (type)
            {
                case HalfOutsiderType.Celestial:
                    opType = "evil";
                    break;
                case HalfOutsiderType.Fiendish:
                    opType = "good";
                    break;
                case HalfOutsiderType.Entropic:
                    opType = "law";
                    break;
                case HalfOutsiderType.Resolute:
                    opType = "chaos";
                    break;
            }

            System.Diagnostics.Debug.Assert(opType != "");

            return opType;


        }

        public static string GetOutsiderDrType(HalfOutsiderType type)
        {
            string opType = "";

            switch (type)
            {
                case HalfOutsiderType.Celestial:
                    opType = "evil";
                    break;
                case HalfOutsiderType.Fiendish:
                    opType = "good";
                    break;
                case HalfOutsiderType.Entropic:
                    opType = "lawful";
                    break;
                case HalfOutsiderType.Resolute:
                    opType = "chaotic";
                    break;
            }

            System.Diagnostics.Debug.Assert(opType != "");

            return opType;


        }

        public static string GetOutsiderTypeName(HalfOutsiderType type)
        {
            string opType = "";

            switch (type)
            {
                case HalfOutsiderType.Celestial:
                    opType = "celestial";
                    break;
                case HalfOutsiderType.Fiendish:
                    opType = "fiendish";
                    break;
                case HalfOutsiderType.Entropic:
                    opType = "entropic";
                    break;
                case HalfOutsiderType.Resolute:
                    opType = "resolute";
                    break;
            }

            System.Diagnostics.Debug.Assert(opType != "");

            return opType;
        }

        private bool MakeSummoned(HalfOutsiderType outsiderType)
        {

            //add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);

            //add smite evil as swift action

            AddSmite(outsiderType, false);
            //add DR/evil as needed
            Dr = AddSummonDr(Dr, Hd, GetOutsiderDrType(outsiderType));

            //add CR as needed
            DieRoll roll = FindNextDieRoll(Hd, 0);
            if (roll.Count >= 5)
            {
                AdjustCr(1);
            }

            //add SR = CR+5
            Sr = AddSummonSr(Sr, Cr, 5);


            int resistAmount = 5;
            if (HdRoll.Count > 10)
            {
                resistAmount = 15;
            }
            else if (HdRoll.Count > 4)
            {
                resistAmount = 10;
            }
            if (Equals(Type, "animal") | Equals(Type, "vermin"))
            {
                //implied but not explictly stated
                //Type = "magical beast";
            }
            switch (outsiderType)
            {
                case HalfOutsiderType.Celestial:
                    {

                        //SubType = "(good, extraplanar)";//implied but not explictly stated
                        //add resist acid, cold, electricity
                        Resist = AddResitance(Resist, "acid", resistAmount);
                        Resist = AddResitance(Resist, "cold", resistAmount);
                        Resist = AddResitance(Resist, "electricity", resistAmount);
                        break;
                    }
                case HalfOutsiderType.Fiendish:
                    {

                        //SubType = "(evil, extraplanar)";//implied but not explictly stated
                        //add resist fire, cold
                        Resist = AddResitance(Resist, "fire", resistAmount);
                        Resist = AddResitance(Resist, "cold", resistAmount);
                        break;
                    }
                case HalfOutsiderType.Entropic:
                    {

                        //SubType = "(chaotic, extraplanar)";//implied but not explictly stated
                        //add resist acid, fire
                        Resist = AddResitance(Resist, "acid", resistAmount);
                        Resist = AddResitance(Resist, "fire", resistAmount);
                        break;
                    }
                case HalfOutsiderType.Resolute:
                    {

                        //SubType = "(lawful, extraplanar)";//implied but not explictly stated
                        //add resist acid, cold, fire
                        Resist = AddResitance(Resist, "acid", resistAmount);
                        Resist = AddResitance(Resist, "cold", resistAmount);
                        Resist = AddResitance(Resist, "fire", resistAmount);
                        break;
                    }
            }

            return true;
        }

        public bool MakeYoung()
        {
            if (SizeMods.GetSize(Size) != MonsterSize.Fine)
            {
                AdjustNaturalArmor(-2);

                AdjustStrength(-4);
                AdjustConstitution(-4);
                AdjustDexterity(4);

                AdjustSize(-1);

                AdjustCr(-1);
                return true;
            }
            return false;
        }

        public bool AugmentSummoning()
        {

            AdjustStrength(4);
            AdjustConstitution(4);

            return true;
        }

        public int AddRacialHd(int dice, Stat stat, bool size)
        {
            CreatureTypeInfo typeInfo = CreatureTypeInfo.GetInfo(Type);

            int oldHp = Hp;

            DieRoll oldHd = FindNextDieRoll(Hd, 0);

            //add hd
            AdjustHd(dice);

            DieRoll newHd = FindNextDieRoll(Hd, 0);

            int appliedDice = newHd.Count - oldHd.Count;

            //adjust skills
            int skillCount = Math.Max(typeInfo.Skills + AbilityBonus(Intelligence), 1);
            AdjustSkills(skillCount * newHd.TotalCount - skillCount * oldHd.TotalCount);


            //add stats if needed
            int statCount = appliedDice / 4;
            if (statCount != 0)
            {
                switch (stat)
                {
                    case Stat.Strength:
                        AdjustStrength(statCount);
                        break;
                    case Stat.Dexterity:
                        AdjustDexterity(statCount);
                        break;
                    case Stat.Constitution:
                        AdjustConstitution(statCount);
                        break;
                    case Stat.Intelligence:
                        AdjustIntelligence(statCount);
                        break;
                    case Stat.Wisdom:
                        AdjustWisdom(statCount);
                        break;
                    case Stat.Charisma:
                        AdjustCharisma(statCount);
                        break;
                }
            }

            //change BAB
            int babChange = typeInfo.GetBab(newHd.Count) - typeInfo.GetBab(oldHd.Count);
            AdjustBaseAttack(babChange, false);

            bool fortGood = false;
            bool refGood = false;
            bool willGood = false;


            //adjust saves
            if (typeInfo.SaveVariesCount == 0)
            {
                fortGood = typeInfo.FortGood;
                refGood = typeInfo.RefGood;
                willGood = typeInfo.WillGood;

            }
            else
            {
                //calc saves
                int baseFort = 0;
                if (Fort != null)
                {
                    baseFort = Fort.Value - AbilityBonus(Type == "undead" ? Charisma : Constitution);
                }

                int baseRef = 0;
                if (Ref != null)
                {
                    baseRef = Ref.Value - AbilityBonus(Dexterity);
                }

                int baseWill = 0;
                if (Will != null)
                {
                    baseWill = Will.Value - AbilityBonus(Wisdom);
                }

                List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();

                list.Add(new KeyValuePair<int, int>(baseFort, 0));
                list.Add(new KeyValuePair<int, int>(baseRef, 1));
                list.Add(new KeyValuePair<int, int>(baseWill, 2));


                list.Sort((a, b) => b.Key - a.Key);

                for (int i = 0; i < typeInfo.SaveVariesCount; i++)
                {
                    switch (list[i].Value)
                    {
                        case 0:
                            fortGood = true;
                            break;
                        case 1:
                            refGood = true;
                            break;
                        case 2:
                            willGood = true;
                            break;
                    }
                }


            }


            if (Fort != null)
            {
                Fort += CreatureTypeInfo.GetSave(fortGood, newHd.Count)
                         - CreatureTypeInfo.GetSave(fortGood, oldHd.Count);
            }
            if (Will != null)
            {
                Will += CreatureTypeInfo.GetSave(willGood, newHd.Count)
                         - CreatureTypeInfo.GetSave(willGood, oldHd.Count);
            }
            if (Ref != null)
            {
                Ref += CreatureTypeInfo.GetSave(refGood, newHd.Count)
                         - CreatureTypeInfo.GetSave(refGood, oldHd.Count);
            }




            //add size if needed
            if (size)
            {
                int sizeChanges = 0;

                if (newHd.Count > oldHd.Count)
                {
                    sizeChanges = (int)Math.Log(((double)newHd.Count) / (double)oldHd.Count, 2.0);
                }
                if (oldHd.Count > newHd.Count)
                {
                    sizeChanges = -(int)Math.Log(((double)oldHd.Count) / (double)newHd.Count, 2.0);
                }

                MonsterSize oldSize = SizeMods.GetSize(Size);

                AdjustSize(sizeChanges);

                MonsterSize newSize = SizeMods.GetSize(Size);

                //add chart size change bonuses
                if (sizeChanges > 0)
                {
                    while (((int)oldSize) < (int)newSize)
                    {

                        oldSize = (MonsterSize)1 + (int)oldSize;

                        SizeMods mods = SizeMods.GetMods(oldSize);
                        AdjustChartMods(mods, true);

                    }
                }
                else if (sizeChanges < 0)
                {

                    while (((int)oldSize) > (int)newSize)
                    {
                        SizeMods mods = SizeMods.GetMods(oldSize);
                        AdjustChartMods(mods, false);

                        oldSize = (MonsterSize)((int)oldSize) - 1;
                    }
                }
            }

            //calculate the new CR
            int crLevel = 0;
            if (!Cr.Contains('/'))
            {
                crLevel = int.Parse(Cr);
            }

            if (oldHp < Hp)
            {
                oldHp += GetCrhpChange(crLevel + 1);

                while (oldHp < Hp)
                {
                    crLevel++;
                    AdjustCr(1);

                    oldHp += GetCrhpChange(crLevel + 1);
                }
            }
            else if (oldHp > Hp)
            {
                oldHp -= GetCrhpChange(crLevel);
                while (oldHp > Hp && crLevel > 1)
                {
                    crLevel--;
                    AdjustCr(-1);

                    oldHp -= GetCrhpChange(crLevel);
                }

            }

            return appliedDice;

        }

        public bool MakeHalfDragon(string color)
        {
            //living creatures only
            if (Constitution == null)
            {
                return false;
            }


            //get element type
            DragonColorInfo colorInfo = _dragonColorList[color.ToLower()];
            string element = colorInfo.Element;

            //make type dragon
            Type = "dragon";

            //add natual armor
            AdjustNaturalArmor(4);

            //add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);

            //add low light vision
            Senses = AddSense(Senses, "low-light vision");

            //add imunnity sleep, paralysis, breath weapon type
            Immune = AddImmunity(Immune, element);
            Immune = AddImmunity(Immune, "sleep");
            Immune = AddImmunity(Immune, "paralysis");

            //add fly average 2x speed
            Speed = AddFlyFromMove(Speed, 2, "average");

            //set bite & 2 claw attacks
            AddNaturalAttack("bite", 2, 1);


            AddNaturalAttack("claw", 1, 0);

            //add breath weapon
            SpecialAttacks = AddSpecialAttack(SpecialAttacks,
                "breath weapon (" + colorInfo.Distance + "-foot " + colorInfo.WeaponType + " of " +
                    colorInfo.Element + ", " + HdRoll.Count +
                    "d6 " + colorInfo.Element + " damage, Reflex DC " +
                    (10 + HdRoll.Count / 2 + AbilityBonus(Constitution)) + " half)", 1);


            //add stats
            AdjustStrength(8);
            AdjustConstitution(6);
            AdjustIntelligence(2);
            AdjustCharisma(2);

            //increase CR
            AdjustCr(2);
            if (Cr == "2")
            {
                AdjustCr(1);
            }

            return true;
        }

        public bool MakeHalfCelestial(HashSet<Stat> bonusStats)
        {

            return MakeHalfOutsider(HalfOutsiderType.Celestial, bonusStats);
        }

        public bool MakeHalfFiend(HashSet<Stat> bonusStats)
        {
            return MakeHalfOutsider(HalfOutsiderType.Fiendish, bonusStats);

        }

        public enum HalfOutsiderType
        {
            Celestial,
            Fiendish,
            Entropic,
            Resolute
        }

        public bool MakeHalfOutsider(HalfOutsiderType outsiderType, HashSet<Stat> bonusStats)
        {
            //living creatures only
            if (Constitution == null || bonusStats.Count != 3 ||
                Intelligence == null || Intelligence < 4)
            {
                return false;
            }


            //increase CR
            if (HdRoll.Count < 5)
            {
                AdjustCr(1);
            }
            else if (HdRoll.Count < 11)
            {
                AdjustCr(2);
            }
            else
            {
                AdjustCr(3);
            }

            //make alignment good/evil
            AlignmentType align = ParseAlignment(Alignment);
            align.Moral = (outsiderType == HalfOutsiderType.Celestial) ? MoralAxis.Good : MoralAxis.Evil;
            Alignment = AlignmentText(align);

            //change type
            Type = "outsider";
            SubType = "(native)";

            //adjust armor
            AdjustNaturalArmor(1);

            //add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);

            //add immunity
            Immune = AddImmunity(Immune, (outsiderType == HalfOutsiderType.Celestial) ? "disease" : "poison");

            //add resistance
            Resist = AddResitance(Resist, "acid", 10);
            Resist = AddResitance(Resist, "cold", 10);
            Resist = AddResitance(Resist, "electricity", 10);

            if (outsiderType == HalfOutsiderType.Celestial)
            {
                SaveMods = AddPlusModValue(SaveMods, "vs. poison", 4);
            }

            //add DR
            Dr = AddDr(Dr, "magic", (HdRoll.Count <= 11) ? 5 : 10);

            //add SR
            Sr = AddSr(Sr, int.Parse(Cr) + 11);

            //add fly average 2x speed
            Speed = AddFlyFromMove(Speed, 2, "good");

            if (outsiderType == HalfOutsiderType.Fiendish)
            {
                //set bite & 2 claw attacks
                AddNaturalAttack("bite", 1, 1);

                AddNaturalAttack("claw", 2, 0);
            }



            AddSmite(outsiderType, true);


            //add spell like abilities
            if (Intelligence >= 8 || (Wisdom != null && Wisdom >= 8))
            {
                int hdCount = HdRoll.Count;

                if (hdCount >= 1)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "protection from evil", 3, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "bless", 1, hdCount);
                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "darkness", 3, hdCount);
                    }

                }
                if (hdCount >= 3)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {

                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "aid", 1, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "detect evil", 1, hdCount);
                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "desecrate", 1, hdCount);
                    }
                }
                if (hdCount >= 5)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "cure serious wounds", 1, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "neutralize poison", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "unholy blight", 1, hdCount);
                    }
                }
                if (hdCount >= 7)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "holy smite", 1, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "remove disease", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "poison", 3, hdCount);
                    }
                }
                if (hdCount >= 9)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "dispel evil", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "contagion", 1, hdCount);
                    }
                }
                if (hdCount >= 11)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "holy word", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "blasphemy", 1, hdCount);
                    }
                }
                if (hdCount >= 13)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "holy aura", 3, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "hallow", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "unholy aura", 3, hdCount);
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "unhallow", 1, hdCount);
                    }
                }
                if (hdCount >= 15)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "mass charm monster", 1, hdCount);

                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "horrid writing", 1, hdCount);
                    }
                }
                if (hdCount >= 17)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "summon monster IX(celestials only)", 1, hdCount);
                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "summon monster IX(fiends only)", 1, hdCount);
                    }
                }
                if (hdCount >= 19)
                {
                    if (outsiderType == HalfOutsiderType.Celestial)
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "resurrection", 1, hdCount);
                    }
                    else
                    {
                        SpellLikeAbilities = AddSpellLikeAbility(SpellLikeAbilities, "destruction", 1, hdCount);
                    }
                }

            }

            //add stats
            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            {
                if (bonusStats.Contains(stat))
                {
                    AdjustStat(stat, 4);
                }
                else
                {
                    AdjustStat(stat, 2);
                }
            }


            return true;
        }

        public void AddSmite(HalfOutsiderType outsiderType, bool half)
        {

            string smiteAlignment = GetOutsiderOpposedType(outsiderType);
            string halfType = (half ? "half-" : "") + GetOutsiderTypeName(outsiderType);

            //add smite good/evil
            SpecialAttacks = AddSpecialAttack(SpecialAttacks, "smite " + smiteAlignment, 1);

            SpecialAbility ab = new SpecialAbility();
            ab.Name = "Smite " + smiteAlignment;
            ab.Type = "Su";
            ab.Text = "Once per day, as a swift action, the " + halfType + " can smite " + smiteAlignment.ToLower() + " as the smite evil ability of a paladin of the same level as the " + halfType + "'s Hit Dice" + ((outsiderType == HalfOutsiderType.Celestial) ? "" : (", except affecting a " + smiteAlignment.ToLower() + " target")) + ". The smite persists until target is dead or the " + halfType + " rests.";
            SpecialAbilitiesList.Add(ab);
        }

        public bool MakeSkeleton(bool bloody, bool burning, bool champion)
        {
            if (string.Compare(Type, "undead", true) == 0 ||
                string.Compare(Type, "construct", true) == 0 ||
                Strength == null || Dexterity == null ||
                    (SubType != null && string.Compare(SubType, "swarm", true) == 0))
            {
                return false;
            }
            if (champion && (Intelligence == null || Intelligence.Value < 3))
            {
                return false;
            }

            //add dex
            AdjustDexterity(2);

            //add strength
            if (champion)
            {
                AdjustStrength(2);
            }

            //remove con
            Constitution = null;

            if (!champion)
            {
                //remove int
                int? old = Intelligence;
                _intelligence = null;
                ApplyIntelligenceAdjustments(old);

                //make wisdom 10
                AdjustWisdom(10 - Wisdom.Value);

                //make charisma 10
                AdjustCharisma(10 - Charisma.Value);
            }



            //change type
            Type = "undead";

            //add natural armor
            switch (SizeMods.GetSize(Size))
            {
                case MonsterSize.Small:
                    AdjustNaturalArmor(1);
                    break;
                case MonsterSize.Medium:
                case MonsterSize.Large:
                    AdjustNaturalArmor(2);
                    break;
                case MonsterSize.Huge:
                    AdjustNaturalArmor(3);
                    break;
                case MonsterSize.Gargantuan:
                    AdjustNaturalArmor(6);
                    break;
                case MonsterSize.Colossal:
                    AdjustNaturalArmor(10);
                    break;
            }

            //change cr
            int count = HdRoll.Count;
            if (count == 1)
            {
                Adjuster.Cr = "1/3";
            }
            else if (count <= 11)
            {
                Adjuster.Cr = (count / 2).ToString();
            }
            else
            {
                Adjuster.Cr = ((count / 3) + 5).ToString();
            }

            //change hd
            DieRoll roll = HdRoll;
            roll.Die = 8;
            if (champion)
            {
                //adjust for new charisma
                roll.Count += 2;
                roll.Mod = roll.TotalCount * AbilityBonus(Charisma);

            }
            else
            {
                roll.Mod = 0;
                if (roll.ExtraRolls != null)
                {
                    //roll.extraRolls.Clear();
                    if (_className != null)
                    {
                        var hdfromClass = GetClassLvls(_className);
                        if (roll.TotalCount - hdfromClass == 0)
                        {
                            roll.Text = "1d8";
                            _className = "";
                        }
                        else
                        {
                            roll.Count = (roll.TotalCount - hdfromClass);
                            roll.Text = roll.Count + "d8";
                            _className = "";
                        }
                    }
                }
            }

            Hd = "(" + DieRollText(roll) + ")";
            Hp = roll.AverageRoll();

            //change saves
            Fort = roll.Count / 3 + AbilityBonus(Charisma);
            Ref = roll.Count / 3 + AbilityBonus(Dexterity);
            Will = roll.Count / 2 + AbilityBonus(Wisdom);



            //remove special attacks & special abilities
            if (!champion)
            {
                RemoveAllForUndead();
            }
            else
            {

                //Add Channel Resistance +4
                DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);
            }


            //add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);


            //add DR
            Dr = AddDr(null, "bludgeoning", 5);

            //add immunities
            if (bloody)
            {
                Immune = AddImmunity(Immune, "cold");
            }

            if (burning)
            {
                //switch cold to fire immunity                
                Immune = AddImmunity(Immune, "fire");
            }

            Immune = AddImmunity(Immune, "undead traits");

            //remove fly
            Speed = RemoveFly(Speed);

            //adjust bab
            AdjustBaseAttack(roll.Count * 3 / 4 - BaseAtk, true);

            //add claws
            AddNaturalAttack("claw", 2, 0);

            //remove feats
            //add improved initiative
            AddFeat("Improved Initiative");
            Init = AbilityBonus(Dexterity) + 4;




            if (champion)
            {
                AdjustCr(1);
            }


            DieRoll hdRoll = HdRoll;
            if (bloody)
            {
                AdjustCharisma(4);
            }
            else if (burning)
            {
                AdjustCharisma(2);
            }


            //adjust for bloody Skeleton
            if (bloody)
            {
                //adjust cr
                AdjustCr(1);

                //add fast healing
                if (hdRoll.Count > 1)
                {
                    AddFastHealing(hdRoll.Count / 2);
                }


                //Add Channel Resistance +4
                DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);


                //add deathless
                Sq = AddToStringList(Sq, "deathless");
                SpecialAbility ab = new SpecialAbility();
                ab.Name = "Deathless";
                ab.Text = "A bloody skeleton is destroyed when reduced to 0 hit points, but it returns to unlife 1 hour later at 1 hit point, allowing its fast healing thereafter to resume healing it. A bloody skeleton can be permanently destroyed if it is destroyed by positive energy, if it is reduced to 0 hit points in the area of a bless or hallow spell, or if its remains are sprinkled with a vial of holy water.";
                ab.Type = "Su";
                SpecialAbilitiesList.Add(ab);


            }
            if (burning)
            {
                //adjust cr
                AdjustCr(1);


                //add fire to melee attacks
                Melee = SetPlusOnMeleeAttacks(Melee, "1d6 fire", false);


                //add aura
                Aura = AddToStringList(Aura, "fiery aura");
                SpecialAbility ab = new SpecialAbility();
                ab.Name = "Fiery Aura";
                ab.Text = "Creatures adjacent to a burning skeleton take 1d6 points of fire damage at the start of their turn. Anyone striking a burning skeleton with an unarmed strike or natural attack takes 1d6 points of fire damage.";
                ab.Type = "Ex";
                SpecialAbilitiesList.Add(ab);


                //add fiery death
                int deathDc = 10 + hdRoll.Count / 2 + AbilityBonus(Charisma);
                Sq = AddToStringList(Aura, "fiery death (DC " + deathDc + ")");
                ab = new SpecialAbility();
                ab.Name = "Fiery Death";
                ab.Text = "A burning skeleton explodes into a burst of flame when it dies. Anyone adjacent to the skeleton when it is destroyed takes 1d6 points of fire damage. A Reflex save (DC " + deathDc + ") halves this damage.";
                ab.Type = "Su";
                SpecialAbilitiesList.Add(ab);

            }


            return true;
        }

        public bool MakeLich()
        {
            if (string.Compare(Type, "undead", true) == 0 ||
                Strength == null || Dexterity == null)
            {
                return false;
            }


            //increase CR
            AdjustCr(2);

            //make alignment evil
            AlignmentType align = ParseAlignment(Alignment);
            align.Moral = MoralAxis.Evil;
            Alignment = AlignmentText(align);


            //make type undead
            Type = "undead";

            //Add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);


            //set natural armor bonus to 5 or creature's natural armor, whichever better
            if (NaturalArmor < 5)
            {
                AdjustNaturalArmor(5 - NaturalArmor);
            }

            //int+2, wis+2, cha+2, con -
            AdjustIntelligence(2);
            AdjustWisdom(2);
            AdjustCharisma(2);
            Constitution = null;

            //add channel resistance 4
            DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);

            // add DR 15/bludgeoning and magic
            Dr = AddDr(null, "bludgeoning and magic", 5);

            //add immunity to cold and electricity
            Immune = AddImmunity(Immune, "cold");
            Immune = AddImmunity(Immune, "electricity");

            //add rejuvenation (su)
            SpecialAbility ab = new SpecialAbility();
            ab.Name = "Rejuvenation";
            ab.Text = "When a lich is destroyed, its phylactery (which is generally hidden by the lich in a safe place far from where it chooses to dwell) immediately begins to rebuild the undead spellcaster's body nearby. This process takes 1d10 days—if the body is destroyed before that time passes, the phylactery merely starts the process anew. After this time passes, the lich wakens fully healed (albeit without any gear it left behind on its old body), usually with a burning need for revenge against those who previously destroyed it.";
            ab.Type = "Su";
            SpecialAbilitiesList.Add(ab);

            //add melee touch attack
            AddNaturalAttack("touch", 1, 8);

            //add Fear Aura (su)
            ab = new SpecialAbility();
            ab.Name = "Fear Aura";
            ab.Text = "Creatures of less than 5 HD in a 60-foot radius that look at the lich must succeed on a Will save or become frightened. Creatures with 5 HD or more must succeed at a Will save or be shaken for a number of rounds equal to the lich's Hit Dice. A creature that successfully saves cannot be affected again by the same lich's aura for 24 hours. This is a mind-affecting fear effect.";
            ab.Type = "Su";
            SpecialAbilitiesList.Add(ab);

            //add Paralyzing Touch (su)
            ab = new SpecialAbility();
            ab.Name = "Paralyzing Touch";
            ab.Text = "Any living creature a lich hits with its touch attack must succeed on a Fortitude save or be permanently paralyzed. Remove paralysis or any spell that can remove a curse can free the victim (see the bestow curse spell description, with a DC equal to the lich's save DC). The effect cannot be dispelled. Anyone paralyzed by a lich seems dead, though a DC 20 Perception check or a DC 15 Heal check reveals that the victim is still alive.";
            ab.Type = "Su";
            SpecialAbilitiesList.Add(ab);

            //add +8 racial bonus on Perception, Sense Motive and Stealth.


            //Make HD D8
            DieRoll roll = HdRoll;
            roll.Die = 8;
            //add charisma and toughness
            roll.Mod = AbilityBonus(Charisma) * roll.TotalCount +
                (roll.TotalCount < 3 ? 3 : roll.TotalCount);
            Hd = "(" + DieRollText(roll) + ")";
            Hp = roll.AverageRoll();

            return true;
        }

        public bool MakeVampire()
        {
            if (string.Compare(Type, "undead", true) == 0 ||
                Strength == null || Dexterity == null)
            {
                return false;
            }

            //increase CR
            AdjustCr(2);

            //Make Evil
            AlignmentType align = ParseAlignment(Alignment);
            align.Moral = MoralAxis.Evil;
            Alignment = AlignmentText(align);

            //Make undead
            Type = "undead";

            //Add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);

            //Add 6 to Natural armor
            AdjustNaturalArmor(6);

            //change ability scores
            AdjustStrength(6);
            AdjustDexterity(4);
            AdjustIntelligence(2);
            AdjustWisdom(2);
            AdjustCharisma(4);
            Constitution = null;

            //Add Channel Resistance +4
            DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);

            //Add DR 10/Magic and silver
            Dr = AddDr(Dr, "magic and silver", 10);

            //Add resist cold and electricity 10
            Resist = AddResitance(Resist, "cold", 10);
            Resist = AddResitance(Resist, "electricity", 10);

            //add fast healing 5
            AddFastHealing(5);

            //add vampire weaknesses
            Weaknesses = AddToStringList(Weaknesses, "vampire weaknesses");

            //add slam
            AddNaturalAttack("slam", 1, 0);

            //**add special attacks

            //blood drain
            SpecialAttacks = AddToStringList(SpecialAttacks, "blood drain");
            SpecialAbility ab = new SpecialAbility();
            ab.Name = "Blood Drain";
            ab.Type = "Su";
            ab.Text = "A vampire can suck blood from a grappled opponent; if the vampire establishes or maintains a pin, it drains blood, dealing 1d4 points of Constitution damage. The vampire heals 5 hit points or gains 5 temporary hit points for 1 hour (up to a maximum number of temporary hit points equal to its full normal hit points) each round it drains blood.";
            SpecialAbilitiesList.Add(ab);

            //children of the night
            SpecialAttacks = AddToStringList(SpecialAttacks, "children of the night");
            ab = new SpecialAbility();
            ab.Name = "Children of the Night";
            ab.Type = "Su";
            ab.Text = "Once per day, a vampire can call forth 1d6+1 rat swarms, 1d4+1 bat swarms, or 2d6 wolves as a standard action. (If the base creature is not terrestrial, this power might summon other creatures of similar power.) These creatures arrive in 2d6 rounds and serve the vampire for up to 1 hour.";
            SpecialAbilitiesList.Add(ab);

            //create spawn
            SpecialAttacks = AddToStringList(SpecialAttacks, "create spawn");
            ab = new SpecialAbility();
            ab.Name = "Create Spawn";
            ab.Type = "Su";
            ab.Text = "A vampire can create spawn out of those it slays with blood drain or energy drain, provided that the slain creature is of the same creature type as the vampire's base creature type. The victim rises from death as a vampire in 1d4 days. This vampire is under the command of the vampire that created it, and remains enslaved until its master's destruction. A vampire may have enslaved spawn totaling no more than twice its own Hit Dice; any spawn it creates that would exceed this limit become free-willed undead. A vampire may free an enslaved spawn in order to enslave a new spawn, but once freed, a vampire or vampire spawn cannot be enslaved again.";
            SpecialAbilitiesList.Add(ab);

            //dominate
            SpecialAttacks = AddToStringList(SpecialAttacks, "dominate");
            ab = new SpecialAbility();
            ab.Name = "Dominate";
            ab.Type = "Su";
            ab.Text = "A vampire can crush a humanoid opponent's will as a standard action. Anyone the vampire targets must succeed on a Will save or fall instantly under the vampire's influence, as though by a dominate person spell (caster level 12th). The ability has a range of 30 feet. At the GM's discretion, some vampires might be able to affect different creature types with this power.";
            SpecialAbilitiesList.Add(ab);

            //energy drain
            SpecialAttacks = AddToStringList(SpecialAttacks, "energy drain");
            ab = new SpecialAbility();
            ab.Name = "EnergyDrain";
            ab.Type = "Su";
            ab.Text = "A creature hit by a vampire's slam (or other natural weapon) gains two negative levels. This ability only triggers once per round, regardless of the number of attacks a vampire makes.";
            SpecialAbilitiesList.Add(ab);

            //**add special qualities

            //change shape
            Sq = AddToStringList(Sq, "change shape (dire bat or wolf, beast shape II)");
            ab = new SpecialAbility();
            ab.Name = "Change Shape";
            ab.Text = "A vampire can use change shape to assume the form of a dire bat or wolf, as beast shape II.";
            ab.Type = "Su"; SpecialAbilitiesList.Add(ab);

            //gaseous form
            Sq = AddToStringList(Sq, "gaseous form");
            ab = new SpecialAbility();
            ab.Name = "Gaseous Form";
            ab.Type = "Su";
            ab.Text = "As a standard action, a vampire can assume gaseous form at will (caster level 5th), but it can remain gaseous indefinitely and has a fly speed of 20 feet with perfect maneuverability.";
            SpecialAbilitiesList.Add(ab);

            //shadowless
            Sq = AddToStringList(Sq, "shadowless");
            ab = new SpecialAbility();
            ab.Name = "Shadowless";
            ab.Type = "Ex";
            ab.Text = "A vampire casts no shadows and shows no reflection in a mirror.";
            SpecialAbilitiesList.Add(ab);

            //spider climb
            Sq = AddToStringList(Sq, "spider climb");
            ab = new SpecialAbility();
            ab.Name = "Spider Climb";
            ab.Type = "Ex";
            ab.Text = "A vampire can climb sheer surfaces as though under the effects of a spider climb spell.";
            SpecialAbilitiesList.Add(ab);

            //add racial skill bonuses
            AddRacialSkillBonus("Bluff", 8);
            AddRacialSkillBonus("Perception", 8);
            AddRacialSkillBonus("Sense Motive", 8);
            AddRacialSkillBonus("Stealth", 8);

            //add feats
            AddFeat("Alertness");
            AddFeat("Combat Reflexes");
            AddFeat("Dodge");
            AddFeat("Improved Initiative");
            AddFeat("Lightning Reflexes");
            AddFeat("Toughness");



            //Make HD D8
            DieRoll roll = HdRoll;
            roll.Die = 8;
            //add charisma and toughness
            roll.Mod = AbilityBonus(Charisma) * roll.TotalCount +
                (roll.TotalCount < 3 ? 3 : roll.TotalCount);
            Hd = "(" + DieRollText(roll) + ")";
            Hp = roll.AverageRoll();




            return true;
        }

        public enum GhostTemplateAbilities
        {
            CorruptingGaze = 0,
            DrainingTouch = 1,
            FrightfulMoan = 2,
            Malevolence = 3,
            Telekinesis = 4
        }

        public static string GetGhostTemplateAbilityName(GhostTemplateAbilities ability)
        {
            switch (ability)
            {
                case Monster.GhostTemplateAbilities.CorruptingGaze:
                    return "Corrupting Gaze";
                case Monster.GhostTemplateAbilities.DrainingTouch:
                    return "Draining Touch";
                case Monster.GhostTemplateAbilities.FrightfulMoan:
                    return "Frightful Moan";
                case Monster.GhostTemplateAbilities.Malevolence:
                    return "Malevolence";
                case Monster.GhostTemplateAbilities.Telekinesis:
                default:
                    return "Telekinesis";
            }
        }



        public bool MakeGhost(GhostTemplateAbilities[] ghostTemplateAbilities)
        {
            if (string.Compare(Type, "undead", true) == 0 || string.Compare(Type, "construct", true) == 0 || Charisma < 6
                || ghostTemplateAbilities is not { Length: 5 })
            {
                return false;
            }
            //increase CR
            AdjustCr(2);

            //make alignment evil
            AlignmentType align = ParseAlignment(Alignment);
            align.Moral = MoralAxis.Evil;
            Alignment = AlignmentText(align);

            //Make undead
            Type = "undead";
            SubType = "Incorporeal";
            var aCondition = new ActiveCondition
            {
                Condition = Condition.FindCondition("Incorporeal")
            };
            AddCondition(aCondition);

            //add undead immunites
            Immune = AddToStringList(Immune, "undead immunities");

            //change ability scores
            Strength = null;
            Constitution = null;
            AdjustCharisma(4);

            //add skill bonuses(class skills?)
            AddRacialSkillBonus("Perception", 8);
            AddRacialSkillBonus("Stealth", 8);

            //remove movements, add fly(what if it already could fly better?)

            int? oldFly = Adjuster.FlySpeed;

            int newSpeed = 30;
            if (oldFly != null && oldFly.Value > 30)
            {
                newSpeed = oldFly.Value;
            }

            Speed = "Fly " + newSpeed + " ft. (perfect)";


            //remove attacks and ranged attacks(if ghost touch add back manually)
            Melee = "";
            Ranged = "";

            //Make HD D8
            DieRoll roll = HdRoll;
            roll.Die = 8;
            //add charisma and toughness
            roll.Mod = AbilityBonus(Charisma) * roll.TotalCount +
                       (roll.TotalCount < 3 ? 3 : roll.TotalCount);
            Hd = "(" + DieRollText(roll) + ")";
            Hp = roll.AverageRoll();

            //Reduce Natural armor to 0
            AdjustNaturalArmor(0);

            //Remove Armor and shield bonuses
            Armor = 0;
            Shield = 0;

            // Add Deflection = to Charisma bonus
            Deflection = AbilityBonus(Charisma);

            //add darkvision
            Senses = ChangeDarkvisionAtLeast(Senses, 60);

            //Defensive Abilities: Ghosts gain channel resistance +4, 
            DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);

            //add rejuvenation (su)
            SpecialAbility ab = new SpecialAbility();
            ab.Name = "Rejuvenation";
            ab.Text = "In most cases, it’s difficult to destroy a ghost through simple combat: the “destroyed” spirit restores itself in 2d4 days. Even the most powerful spells are usually only temporary solutions. The only way to permanently destroy a ghost is to determine the reason for its existence and set right whatever prevents it from resting in peace. The exact means varies with each spirit and may require a good deal of research, and should be created specifically for each different ghost by the GM.";
            ab.Type = "Su";
            SpecialAbilitiesList.Add(ab);

            //Corrupting Touch
            SpecialAttacks = AddToStringList(SpecialAttacks, "Corrupting Touch Fort Half DC: " + (10 + (HdRoll.TotalCount / 2) + AbilityBonus(Charisma)).ToString() + " (Touch +" + (BaseAtk + AbilityBonus(Charisma)).ToString() + " (" + Cr + "d6 plus 1d4 Cha))");
            ab = new SpecialAbility();
            ab.Name = "Corrupting Touch";
            ab.Type = "Su";
            ab.Text = "All ghosts gain this incorporeal touch attack. By passing part of its incorporeal body through a foe’s body as a standard action, the ghost inflicts a number of d6s equal to its CR in damage. This damage is not negative energy—it manifests in the form of physical wounds and aches from supernatural aging. Creatures immune to magical aging are immune to this damage, but otherwise the damage bypasses all forms of damage reduction. A Fortitude save halves the damage inflicted.";
            SpecialAbilitiesList.Add(ab);

            //special attacks to choose from
            int bonusAbilites = (this.GetCrIntWholeNumber() - 3) / 3;

            for (int i = 0; i < CmMathUtilities.Max(bonusAbilites, 5); i++)
            {
                GhostTemplateAbilities ability = ghostTemplateAbilities[i];
                switch (ability)
                {
                    case GhostTemplateAbilities.CorruptingGaze:

                        //Corrupting Gaze
                        SpecialAttacks = AddToStringList(SpecialAttacks, "Corrupting Gaze 30' Fort Negates Charisma Damage DC:" + (10 + (HdRoll.TotalCount / 2) + AbilityBonus(Charisma)).ToString() + " (Gaze (2d10 plus 1d4 Cha))");
                        ab = new SpecialAbility();
                        ab.Name = "Corrupting Gaze";
                        ab.Type = "Su";
                        ab.Text = "The ghost is disfigured through age or violence, and has a gaze attack with a range of 30 feet that causes 2d10 damage and 1d4 Charisma damage (Fortitude save negates Charisma damage but not physical damage).";
                        SpecialAbilitiesList.Add(ab);

                        break;
                    case GhostTemplateAbilities.DrainingTouch:
                        //Draining Touch
                        SpecialAttacks = AddToStringList(SpecialAttacks, "Draining Touch (Touch +" + (BaseAtk + AbilityBonus(Charisma)).ToString() + " 1d4 drain from chosen Stat)");
                        ab = new SpecialAbility();
                        ab.Name = "Draining Touch";
                        ab.Type = "Su";
                        ab.Text = "The ghost died while insane or diseased. It gains a touch attack that drains 1d4 points from any one ability score it selects on a hit. On each such successful attack, the ghost heals 5 points of damage to itself. When a ghost makes a draining touch attack, it cannot use its standard ghostly touch attack.";
                        SpecialAbilitiesList.Add(ab);
                        break;

                    case GhostTemplateAbilities.FrightfulMoan:
                        //Frightful Moan
                        SpecialAttacks = AddToStringList(SpecialAttacks, "Frightful Moan 30' Spread Will Negates DC:" + (10 + (HdRoll.TotalCount / 2) + AbilityBonus(Charisma)).ToString());
                        ab = new SpecialAbility();
                        ab.Name = "Frightful Moan";
                        ab.Type = "Su";
                        ab.Text = "The ghost died in the throes of crippling terror. It can emit a frightful moan as a standard action. All living creatures within a 30-foot spread must succeed on a Will save or become panicked for 2d4 rounds. This is a sonic mind-affecting fear effect. A creature that successfully saves against the moan cannot be affected by the same ghost’s moan for 24 hours.";
                        SpecialAbilitiesList.Add(ab);
                        break;

                    case GhostTemplateAbilities.Malevolence:

                        //Malevolence
                        SpecialAttacks = AddToStringList(SpecialAttacks, "Malevolence Will DC:" + (10 + (HdRoll.TotalCount / 2) + AbilityBonus(Charisma)).ToString());
                        ab = new SpecialAbility();
                        ab.Name = "Malevolence";
                        ab.Type = "Su";
                        ab.Text = "The ghost died while insane or diseased. It gains a touch attack that drains 1d4 points from any one ability score it selects on a hit. On each such successful attack, the ghost heals 5 points of damage to itself. When a ghost makes a draining touch attack, it cannot use its standard ghostly touch attack.";
                        SpecialAbilitiesList.Add(ab);
                        break;

                    case GhostTemplateAbilities.Telekinesis:
                        //Telekinesis
                        SpecialAttacks = AddToStringList(SpecialAttacks, "Telekinesis Will DC:" + (10 + (HdRoll.TotalCount / 2) + AbilityBonus(Charisma)).ToString());
                        ab = new SpecialAbility();
                        ab.Name = "Telekinesis";
                        ab.Type = "Su";
                        ab.Text = "The ghost’s death involved great physical trauma. The ghost can use telekinesis as a standard action once every 1d4 rounds (caster level 12th or equal to the ghost’s HD, whichever is higher).";
                        SpecialAbilitiesList.Add(ab);
                        break;
                }
            }

            return true;
        }

        public enum ZombieType
        {
            Normal = 0,
            Juju = 1,
            Fast = 2,
            Plague = 3,
            Void = 4
        }

        public bool MakeZombie(ZombieType zombieType)
        {

            if (string.Compare(Type, "undead", true) == 0 ||
                Strength == null || Dexterity == null)
            {
                return false;
            }



            //change type
            Type = "undead";

            if (zombieType == ZombieType.Juju)
            {

                //Make Evil
                AlignmentType align = ParseAlignment(Alignment);
                align.Moral = MoralAxis.Evil;
                Alignment = AlignmentText(align);


                //adjust natural armor
                AdjustNaturalArmor(3);

                //change hd
                ChangeHdToDie(8);

                //Defensive Abilities: Juju zombies gain channel resistance +4, 
                DefensiveAbilities = ChangeSkillStringMod(DefensiveAbilities, "channel resistance", 4, true);

                //DR 5/magic and slashing (or DR 10/magic and slashing if it has 11 HD or more), 
                Dr = AddDr(Dr, "magic", (HdRoll.Count <= 11) ? 5 : 10);
                Dr = AddDr(Dr, "slashing", (HdRoll.Count <= 11) ? 5 : 10);

                //and fire resistance 10. 
                Resist = AddResitance(Resist, "fire", 10);

                //They are immune to cold, electricity, and magic missile.
                Immune = AddImmunity(Immune, "cold");
                Immune = AddImmunity(Immune, "electricity");
                Immune = AddImmunity(Immune, "magic missile");

                //Speed: A winged juju zombie’s maneuverability drops to clumsy. If the base creature flew magically, its fly speed is unchanged. Retain all other movement types.
                Speed = SetFlyQuality(Speed, "clumsy");

                //Attacks: A juju zombie retains all the natural weapons, manufactured weapon attacks, and weapon proficiencies of the base creature. It also gains a slam attack that deals damage based on the juju zombie’s size, but as if it were one size category larger than its actual size.
                //add slam (+1 diestep)
                AddNaturalAttack("slam", 1, 1);

                //Abilities: Increase from the base creature as follows: Str +4, Dex +2. A juju zombie has no Constitution score; as an undead, it uses its Charisma in place of Constitution when calculating hit points, Fortitude saves, or any special ability that relies on Constitution.
                AdjustStrength(4);
                AdjustDexterity(2);

                //Feats: A juju zombie gains Improved Initiative and Toughness as bonus feats.
                AddFeat("Improved Initiative");
                AddFeat("Toughness");

                //Skills: A juju zombie gains a +8 racial bonus on all Climb checks.
                AddRacialSkillBonus("Climb", 8);

            }
            else
            {

                //Make Neutral Evil
                AlignmentType al = new AlignmentType();
                al.Moral = MoralAxis.Evil;
                al.Order = OrderAxis.Neutral;
                Alignment = AlignmentText(al);

                //adjust strength
                AdjustStrength(2);

                //Adjust dex
                AdjustDexterity(-2);

                //Adjust Charisma
                if (Charisma == null)
                {
                    _charisma = 10;
                }
                else
                {
                    AdjustCharisma(10 - Charisma.Value);
                }

                //Adjust Wisdom
                if (Wisdom == null)
                {
                    _wisdom = 10;
                }
                else
                {
                    AdjustWisdom(10 - Wisdom.Value);
                }

                //change hd
                ChangeHdToDie(8);

                DieRoll roll = HdRoll;

                //change saves
                Fort = roll.Count / 3 + AbilityBonus(Charisma);
                Ref = roll.Count / 3 + AbilityBonus(Dexterity);
                Will = roll.Count / 2 + AbilityBonus(Wisdom);

                //change cr
                int count = HdRoll.Count;
                if (count == 1)
                {
                    Adjuster.Cr = "1/4";
                }
                else if (count == 2)
                {
                    Adjuster.Cr = "1/4";
                }
                else if (count <= 12)
                {
                    Adjuster.Cr = ((count - 1) / 2).ToString();
                }
                else
                {
                    Adjuster.Cr = ((count - 12) / 3 + 6).ToString();
                }



                //remove special abilities
                RemoveAllForUndead();


                //add darkvision
                Senses = ChangeDarkvisionAtLeast(Senses, 60);


                //add natural ac
                MonsterSize ms = SizeMods.GetSize(Size);
                if (ms >= MonsterSize.Small)
                {
                    int bonus = 0;

                    if (ms < MonsterSize.Gargantuan)
                    {
                        bonus = ((int)ms) - (int)MonsterSize.Tiny;
                    }
                    else if (ms == MonsterSize.Gargantuan)
                    {
                        bonus = +7;
                    }
                    else if (ms == MonsterSize.Colossal)
                    {
                        bonus += 11;
                    }

                    AdjustNaturalArmor(bonus - NaturalArmor);
                }



                //add undead imuinites
                Immune = AddToStringList(Immune, "undead immunities");


                //add DR/5 slashing
                Dr = AddDr(Dr, "slashing", 5);

                //make fly clumsy
                Speed = SetFlyQuality(Speed, "clumsy");

                //adjust BAB
                AdjustBaseAttack(roll.Count * 3 / 4 - BaseAtk, true);


                //add slam (+1 diestep)
                AddNaturalAttack("slam", 1, 1);

                //add toughness
                AddFeat("Toughness");

                //add staggered SQ
                Sq = AddToStringList(Sq, "staggered");
                SpecialAbility ab = new SpecialAbility();
                ab.Name = "Staggered";
                ab.Type = "Ex";
                ab.Text = "Zombies have poor reflexes and can only perform a single move action or standard action each round. A zombie can move up to its speed and attack in the same round as a charge action.";
                SpecialAbilitiesList.Add(ab);

                //set values for zombie subtypes
                if (zombieType == ZombieType.Fast || zombieType == ZombieType.Void)
                {
                    //speed + 10
                    Speed = ChangeStartingNumber(Speed, 10);

                    //remove DR
                    Dr = null;

                    //remove staggered
                    Sq = RemoveFromStringList(Sq, "staggered");
                    SpecialAbilitiesList.Clear();

                    //Quick Strikes
                    SpecialAttacks = AddToStringList(SpecialAttacks, "quick strikes");
                    ab = new SpecialAbility();
                    ab.Name = "Quick Strikes";
                    ab.Type = "Ex";
                    ab.Text = "Whenever a fast zombie takes a full-attack action, it can make one additional slam attack at its highest base attack bonus.";
                    SpecialAbilitiesList.Add(ab);
                    //switch to 2 slams for quick strikes
                    AddNaturalAttack("slam", 2, 1);

                    //Add dex to change from -2 to +2
                    AdjustDexterity(4);

                    if (zombieType == ZombieType.Void)
                    {
                        //Tongue attack
                        AddNaturalAttack("tongue", 1, 5, null, true);

                        //Blood Drain
                        SpecialAttacks = AddToStringList(SpecialAttacks, "blood drain");
                        ab = new SpecialAbility();
                        ab.Name = "Blood Drain";
                        ab.Type = "Ex";
                        ab.Text = "If a void zombie hits a living creature with its tongue attack, it drains blood, dealing 2 points of Strength damage before the tongue detaches.";
                        SpecialAbilitiesList.Add(ab);
                    }
                }
                else if (zombieType == ZombieType.Plague)
                {
                    //remove DR
                    Dr = null;

                    //get save dc
                    int dc = 10 + roll.Count / 2 + AbilityBonus(Charisma);

                    //add death burst
                    SpecialAttacks = AddToStringList(SpecialAttacks, "death burst (DC " + dc + ")");
                    ab = new SpecialAbility();
                    ab.Name = "Death Burst";
                    ab.Type = "Ex";
                    ab.Text = "When a plague zombie dies, it explodes in a burst of decay. All creatures adjacent to the plague zombie are exposed to its plague as if struck by a slam attack and must make a Fortitude save or contract zombie rot.";
                    SpecialAbilitiesList.Add(ab);

                    //add disease
                    Melee = SetPlusOnMeleeAttacks(Melee, "disease", true);
                    ab = new SpecialAbility();
                    ab.Name = "Zombie Rot";
                    ab.Type = "Su";
                    ab.Text = "The slam attack — as well as any other natural attacks — of a plague zombie carries the zombie rot disease.\r\n\r\n" +
                                "Zombie rot: slam; save Fort DC " + dc + "; onset 1d4 days; frequency 1/day; effect 1d2 Con, this damage cannot be healed while the creature is infected; cure 2 consecutive saves. Anyone who dies while infected rises as a plague zombie in 2d6 hours.";
                    SpecialAbilitiesList.Add(ab);

                }
            }


            return true;
        }

        public enum SimpleMythicTemplateType
        {
            Agile,
            Arcane,
            Divine,
            Invincible,
            Savage
        }

        public bool MakeSimpleMythic(SimpleMythicTemplateType templateType)
        {


            switch (templateType)
            {
                case SimpleMythicTemplateType.Agile:

                    AddSubtype( "mythic");

                    //mr 1
                    AdjustMr(1);

                    //CR 1
                    AdjustCr(1);

                    //Init + 20 bonus;
                    Init += 20;

                    //AC + 2 dodge bonus;
                    AdjustDodge(2);

                    //hp mythic bonus hit points(see Mythic Bonus Hit Points sidebar);

                    //Defensive Abilities evasion(as the rogue class feature); 
                    DefensiveAbilities = AddToStringList(DefensiveAbilities, "evasion");

                    //Speed +30 feet for all movement types(up to double the creature’s base movement speed);
                    Adjuster.AdjustAllSpeed(30);

                    //Special Attacks dual initiative.
                    SpecialAttacks = AddToStringList(SpecialAttacks, "dual initiative");
                    DualInit = Init - 20;

                    return true;
                case SimpleMythicTemplateType.Arcane:

                    AddSubtype("mythic");

                    //mr 1
                    AdjustMr(HdRoll.TotalCount > 10 ? 2 : 1);

                    //CR 1
                    AdjustCr(1);

                    //AC + 2 deflection bonus; 
                    AdjustDeflection(2);

                    // SR gains SR equal to its new CR + 11; 
                    Sr = (GetCrIntWholeNumber() + 11).ToString();

                    //Special Attacks mythic magic, 
                    SpecialAttacks = AddToStringList(SpecialAttacks, "mythic magic");
                    SpecialAbility ab = new SpecialAbility();
                    ab.Name = "Mythic Magic";
                    ab.Text = "Up to three times per day, when the creature casts a spell, it can cast the mythic version instead(as with all mythic spells, the creature must expend mythic power to cast a mythic spell in this way).";
                    ab.Type = "Su";
                    SpecialAbilitiesList.Add(ab);


                    //simple arcane spellcasting.
                    SpecialAttacks = AddToStringList(SpecialAttacks, "simple arcane spellcasting");
                    ab = new SpecialAbility();
                    ab.Name = "Simple Arcane Spellcasting";
                    ab.Text = "The creature gains the ability to cast spells from the sorcerer/wizard spell list. " +
                        "Select a number of spells with total spell levels equal to twice the creature’s CR. No spell for this ability should have a level higher than 1 + 1 / 2 the creature’s CR.A 0 - level spell counts as 1 / 2 spell level toward this total. " +
                        "The creature can cast each of these spells once per day. " +
                        "Its caster level is equal to its Hit Dice.It uses the higher of its Intelligence or Charisma modifiers to determine its spell DCs.";
                    ab.Type = "Su";
                    SpecialAbilitiesList.Add(ab);

                    return true;

                case SimpleMythicTemplateType.Divine:

                    AddSubtype("mythic");

                    //mr 1
                    AdjustMr(HdRoll.TotalCount > 10 ? 2 : 1);

                    //CR 1
                    AdjustCr(1);


                    //Aura aura of grace
                    Aura = AddToStringList(Aura, "aura of grace");
                    ab = new SpecialAbility();
                    ab.Name = "Aura of Grace";
                    ab.Text = "This creature and all allies within 10 feet receive a + 2 sacred bonus on saving throws—or a profane bonus if the this creature is evil.";
                    ab.Type = "Ex";
                    SpecialAbilitiesList.Add(ab);


                    //AC + 2 deflection bonus;
                    AdjustDeflection(2);
                    //Special Attacks mythic magic, 
                    SpecialAttacks = AddToStringList(SpecialAttacks, "mythic magic");
                    ab = new SpecialAbility();
                    ab.Name = "Mythic Magic";
                    ab.Text = "Up to three times per day, when the creature casts a spell, it can cast the mythic version instead(as with all mythic spells, the creature must expend mythic power to cast a mythic spell in this way).";
                    ab.Type = "Su";
                    SpecialAbilitiesList.Add(ab);

                    //simple arcane spellcasting.
                    SpecialAttacks = AddToStringList(SpecialAttacks, "simple divine spellcasting");

                    ab = new SpecialAbility();
                    ab.Name = "Simple divine Spellcasting";
                    ab.Text = "The creature gains the ability to cast spells from the cleric or druid spell list. " +
                        "Select a number of spells with total spell levels equal to twice the creature’s CR. " +
                        "No spell for this ability should have a level higher than 1 + 1/2 the creature’s CR. " +
                        "A 0-level spell counts as 1/2 spell level toward this total. The creature can cast each of these spells once per day." +
                        "Its caster level is equal to its Hit Dice. It uses its Wisdom or Charisma (whichever is higher) to determine its spell DCs.";
                    ab.Type = "Su";
                    SpecialAbilitiesList.Add(ab);


                    return true;

                case SimpleMythicTemplateType.Invincible:


                    AddSubtype("mythic");

                    //mr 1
                    AdjustMr(HdRoll.TotalCount > 10 ? 2 : 1);

                    //CR 1
                    AdjustCr(1);

                    //AC increase natural armor bonus by 2(or 4 if the creature has 11 or more Hit Dice);
                    AdjustNaturalArmor(HdRoll.TotalCount > 10 ? 4 : 2);

                    //gains DR and resistance to all types of energy as per Table: Invincible Template Defenses, as well as 
                    GainEnergyResistForMythicTemplates();

                    //block attacks and                    
                    DefensiveAbilities = AddToStringList(DefensiveAbilities, "block attacks");
                    ab = new SpecialAbility();
                    ab.Name = "Block Attacks";
                    ab.Text = "Once per round, when the creature is hit by a melee or ranged attack, it can attempt a melee attack using its highest attack bonus. If this result exceeds the result from the attack against it, the creature is unaffected by the attack (as if the attack had missed).";
                    ab.Type = "Ex";
                    SpecialAbilitiesList.Add(ab);

                    //second save.
                    ab = new SpecialAbility();
                    ab.Name = "Second Save";
                    ab.Text = "Whenever the creature fails a saving throw against an effect with a duration greater than 1 round, it can keep trying to shake off the effect. At the start of its turn, if it’s still affected, it can attempt the save one more time as a free action. If this save succeeds, the effect affects the creature as if it had succeeded at its initial saving throw. " +
                        "If the effect already allows another saving throw on a later turn to break the effect(such as for hold monster), this ability is in addition to the extra saving throw from the effect.";
                    ab.Type = "Ex";
                    SpecialAbilitiesList.Add(ab);

                    return true;

                case SimpleMythicTemplateType.Savage:

                    AddSubtype("mythic");

                    //mr 1
                    AdjustMr(HdRoll.TotalCount > 10 ? 2 : 1);

                    //CR 1
                    AdjustCr(1);

                    //AC increase natural armor bonus by 2;
                    AdjustNaturalArmor(2);

                    //Defensive Abilities gains DR and resistance to all types of energy as per Table: Savage Template Defenses 
                    GainEnergyResistForMythicTemplates();

                    //Special Attacks all attacks gain bleed 1(this stacks with itself), 
                    ObservableCollection<AttackSet> sets = new ObservableCollection<AttackSet>(MeleeAttacks);
                    ObservableCollection<ViewModels.Attack> ranged = new ObservableCollection<ViewModels.Attack>(RangedAttacks);
                    var attacks = new CharacterAttacks(sets, ranged);

                    foreach (var item in attacks.NaturalAttacks)
                    {
                        if (item.Plus == null || item.Plus.Length == 0)
                        {
                            item.Plus = "bleed 1";
                        }
                        else
                        {
                            item.Plus = item.Plus + " and bleed 1";
                        }
                            
                    }

                   Melee = MeleeString(attacks);



                    //feral savagery(full attack).
                    SpecialAttacks = AddToStringList(SpecialAttacks, "feral savagery(full attack)");
                    ab = new SpecialAbility();
                    ab.Name = "Feral Savagery";
                    ab.Text = "Under the circumstances listed in the monster’s stat block—such as when it makes a full attack or a rend attack—it can immediately attempt an additional attack against an opponent. " +
                        "This attack is made using the creature’s full base attack bonus, plus any modifiers appropriate to the situation. " +
                        "This additional attack doesn’t stack with similar means of gaining additional attacks, such as the haste spell or a speed weapon. " +
                        "This ability doesn’t grant an extra action, so you can’t use it to cast a second spell or otherwise take an extra action in the round.";
                    ab.Type = "Ex";
                    return true;


            }
            return false;

        }

        void GainEnergyResistForMythicTemplates()
        {
            if (HdRoll.TotalCount > 10)
            {
                Resist = AddResitance(Resist, "energy", 15);
                Dr = AddDr(Dr, "epic", 10);

            }
            else if (HdRoll.TotalCount > 4)
            {
                Resist = AddResitance(Resist, "energy", 10);
                Dr = AddDr(Dr, "epic", 5);
            }
            else
            {
                Resist = AddResitance(Resist, "energy", 5);
            }

        }


        public bool MakeOgrekin(int benefit, int disadvantage)
        {
            AdjustCr(1);

            SubType = "giant";

            //Armor Class: Natural armor improves by +3.
            AdjustNaturalArmor(3);

            //Ability Scores: Str +6, Con +4, Int –2, Cha –2.
            AdjustStrength(6);
            AdjustConstitution(4);
            AdjustIntelligence(-2);
            AdjustCharisma(-2);

            switch (benefit)
            {
                case 1:
                    //1: Oversized Limb: The ogrekin can wield weapons one size category larger than normal without any penalty and 
                    Sq = AddToStringList(Sq, "oversized limb");

                    //gains a +2 bonus to its Strength.
                    AdjustStrength(2);

                    break;


                case 2:
                    //2: Oversized Maw: The ogrekin gains a bite attack (1d4).
                    Sq = AddToStringList(Sq, "oversized maw");
                    AddNaturalAttack("bite", 1, -1);
                    break;

                case 3:
                    //3: Quick Metabolism: The ogrekin gains a +2 racial bonus on Fortitude saves.
                    Sq = AddToStringList(Sq, "quick metabolism");
                    if (Fort != null)
                    {
                        Fort += 2;
                    }
                    break;

                case 4:
                    //4: Thick Skin: Improve natural armor bonus by +2.
                    Sq = AddToStringList(Sq, "thick skin");
                    AdjustNaturalArmor(2);
                    break;

                case 5:
                    //5: Vestigial Limb: Vestigial third arm (can't be used to use items) grants a +4 racial bonus on grapple checks.
                    Sq = AddToStringList(Sq, "vestigial limb");

                    break;

                case 6:
                    //6: Vestigial Twin: A malformed twin's head juts out from the ogrekin, providing the ogrekin with all-around vision.
                    Sq = AddToStringList(Sq, "vestigial twin");
                    Senses = AddSense(Senses, "all-around vision");

                    break;

            }

            switch (disadvantage)
            {
                case 1:
                    //1: Deformed Hand: One hand can't wield weapons; –2 penalty on attack rolls with two-handed weapons.
                    Sq = AddToStringList(Sq, "deformed hand");
                    break;


                case 2:
                    //2: Fragile: The ogrekin is particularly frail and gaunt. It loses its +4 racial bonus to Con.
                    Sq = AddToStringList(Sq, "fragile");
                    AdjustConstitution(4);
                    break;


                case 3:
                    //3: Light Sensitive: The ogrekin gains light sensitivity.
                    Sq = AddToStringList(Sq, "light sensitive");
                    break;


                case 4:
                    //4: Obese: The ogrekin takes a –2 penalty to Dexterity (minimum score of 1).
                    Sq = AddToStringList(Sq, "obese");
                    if (Dexterity > 3)
                    {
                        AdjustDexterity(-2);
                    }
                    else if (Dexterity == 3)
                    {
                        AdjustDexterity(-1);
                    }
                    break;


                case 5:
                    //5: Stunted Legs: The ogrekin's base speed is reduced by 10 feet (minimum base speed of 5 feet).
                    Sq = AddToStringList(Sq, "stunted legs");

                    Adjuster.LandSpeed = Math.Min(Adjuster.LandSpeed - 10, 5);

                    break;


                case 6:
                    //6: Weak Mind: The ogrekin's head is huge and misshapen. It gains a –2 penalty on Will saving throws.
                    Sq = AddToStringList(Sq, "weak mind");
                    if (Will != null)
                    {
                        Will -= 2;
                    }
                    break;

            }


            return true;
        }
#endregion

        public void ChangeHdToDie(int die)
        {
            DieRoll roll = HdRoll;
            roll.Die = 8;

            switch (SizeMods.GetSize(Size))
            {
                case MonsterSize.Small:
                case MonsterSize.Medium:
                    roll.Count++;
                    break;
                case MonsterSize.Large:
                    roll.Count += 2;
                    break;
                case MonsterSize.Huge:
                    roll.Count += 4;
                    break;
                case MonsterSize.Gargantuan:
                    roll.Count += 6;
                    break;
                case MonsterSize.Colossal:
                    roll.Count += 10;
                    break;
            }

            Hd = "(" + DieRollText(roll) + ")";
            if (roll.ExtraRolls != null)
            {
                roll.ExtraRolls.Clear();
            }


            roll.Mod = Math.Max(3, roll.Count);
            Hp = roll.AverageRoll();
        }

        public void AdjustBaseAttack(int diff, bool fix)
        {
            //adjust bab
            BaseAtk += diff;

            SizeMods mods = SizeMods.GetMods(SizeMods.GetSize(Size));

            //adjust cmb
            CmbNumeric += diff;

            //adjust cmd
            CmdNumeric += diff;

            if (fix)
            {
                //fix attacks
                Melee = FixMeleeAttacks(Melee, BaseAtk + AbilityBonus(Strength) + mods.Attack, AbilityBonus(Strength), true);

                Ranged = FixRangedAttacks(Ranged, BaseAtk + AbilityBonus(Dexterity) + mods.Attack, AbilityBonus(Strength), true, this);
            }
            else
            {

                Melee = ChangeAttackMods(Melee, diff);
                Ranged = ChangeAttackMods(Ranged, diff);
            }
        }

        public void FixMeleeAttacks()
        {
            FixAttacks(true, false);
        }

        public void FixRangedAttacks()
        {
            FixAttacks(false, true);
        }

        public void FixAttacks()
        {
            FixAttacks(true, true);
        }

        public void FixAttacks(bool fixMelee, bool fixRanged)
        {

            ObservableCollection<AttackSet> sets = new ObservableCollection<AttackSet>(MeleeAttacks);
            ObservableCollection<ViewModels.Attack> ranged = new ObservableCollection<ViewModels.Attack>(RangedAttacks);
            CharacterAttacks attacks = new CharacterAttacks(sets, ranged);
            if (fixMelee)
            {
                Melee = MeleeString(attacks);
            }
            if (fixRanged)
            {
                Ranged = RangedString(attacks);
            }
        }

        public void AddNaturalAttack(string name, int count, int step)
        {
            AddNaturalAttack(name, count, step, null);
        }


        public void AddNaturalAttack(string name, int count, int step, string plus)
        {

            AddNaturalAttack(name, count, step, plus, false);
        }

        public void AddNaturalAttack(string name, int count, int step, string plus, bool noDamageBonus)
        {

            if (Weapon.Weapons.ContainsKey(name))
            {
                ObservableCollection<AttackSet> sets = new ObservableCollection<AttackSet>(MeleeAttacks);
                ObservableCollection<ViewModels.Attack> ranged = new ObservableCollection<ViewModels.Attack>(RangedAttacks);
                CharacterAttacks attacks = new CharacterAttacks(sets, ranged);

                bool bAdded = false;
                foreach (WeaponItem wi in attacks.NaturalAttacks)
                {
                    if (string.Compare(wi.Name, Name, true) == 0)
                    {
                        if (wi.Count < count)
                        {
                            wi.Count = count;
                        }
                        if (plus != null)
                        {
                            wi.Plus = plus;
                        }
                        bAdded = true;
                        break;
                    }

                }

                if (!bAdded)
                {
                    WeaponItem item = new WeaponItem(Weapon.Weapons[name]);
                    item.Count = count;
                    if (plus != null)
                    {
                        item.Plus = plus;
                    }
                    if (noDamageBonus)
                    {
                        item.NoMods = true;

                        DieRoll damageRoll = new DieRoll(0, 1, 0);
                        damageRoll = DieRoll.StepDie(damageRoll, step);
                        item.Step = damageRoll.Step;
                    }
                    attacks.NaturalAttacks.Add(item);
                }

                Melee = MeleeString(attacks);
            }
            else
            {

                ViewModels.Attack attack = new ViewModels.Attack();
                attack.CritMultiplier = 2;
                attack.CritRange = 20;
                attack.Name = name;
                attack.Count = count;
                MonsterSize monsterSize = SizeMods.GetSize(Size);
                SizeMods mods = SizeMods.GetMods(monsterSize);

                DieRoll damageRoll = new DieRoll(0, 1, noDamageBonus ? 0 : AbilityBonus(Strength));
                attack.Damage = DieRoll.StepDie(damageRoll, (int)monsterSize + step);


                attack.Bonus = new List<int>() { BaseAtk + AbilityBonus(Strength) + mods.Attack };

                if (plus != null)
                {
                    attack.Plus = plus;
                }

                Melee = AddAttack(Melee, attack);
            }

        }

        public void RemoveAllForUndead()
        {
            //remove defensive abilities
            DefensiveAbilities = null;
            Sr = null;
            Weaknesses = null;
            Resist = null;
            Dr = null;
            Aura = null;


            //remove Immunity
            Immune = null;

            //remove skills
            SkillValueDictionary.Clear();
            Languages = null;
            RacialMods = null;

            //remove feats
            FeatsList.Clear();

            //remove special attacks & special abilities
            SpecialAbilities = null;
            SpecialAbilitiesList.Clear();
            SpecialAttacks = null;
            SpellLikeAbilities = null;
            SpellsKnown = null;
            SpellDomains = null;
            SpellsPrepared = null;
            Sq = null;

        }

        public void ApplyFeatChanges(string feat, bool added)
        {

            if (feat == "Alertness")
            {
                AddOrChangeSkill("Perception", added ? 2 : -2);
                AddOrChangeSkill("Sense Motive", added ? 2 : -2);

            }
            else if (feat == "Dodge")
            {

                FullAc += added ? 1 : -1;
                TouchAc += added ? 1 : -1;
            }
            else if (feat == "Improved Initiative")
            {
                Init += added ? 4 : -4;
            }
            else if (feat == "Lightning Reflexes")
            {
                Ref += added ? 2 : -2;
            }
            else if (feat == "Great Fortitude")
            {
                Fort += added ? 2 : -2;
            }
            else if (feat == "Iron Will")
            {
                Will += added ? 2 : -2;
            }
            else if (feat == "Toughness")
            {
                DieRoll roll = HdRoll;

                int bonus = roll.TotalCount < 3 ? 3 : roll.TotalCount;

                roll.Mod += added ? bonus : -bonus;

                HdRoll = roll;

                Hp += added ? bonus : -bonus;

            }
            else if (feat == "Weapon Finesse")
            {
                //Attacks
                FixMeleeAttacks();
            }
        }

        private static int GetCrhpChange(int crLevel)
        {
            int val = 0;

            if (crLevel <= 2)
            {
                val = 5;
            }
            else if (crLevel <= 4)
            {
                val = 10;
            }
            else if (crLevel <= 12)
            {
                val = 15;
            }
            else if (crLevel <= 16)
            {
                val = 20;
            }
            else if (crLevel <= 20)
            {
                val = 30;
            }
            else
            {
                val = 30;
            }

            return val;
        }

        private void AdjustChartMods(SizeMods mods, bool add)
        {
            if (mods.Strength != 0)
            {
                AdjustStrength(mods.Strength * (add ? 1 : -1));
            }
            if (mods.Dexterity != 0)
            {
                AdjustDexterity(mods.Dexterity * (add ? 1 : -1));
            }
            if (mods.Constitution != 0)
            {
                AdjustConstitution(mods.Constitution * (add ? 1 : -1));
            }
            if (mods.NaturalArmor != 0)
            {
                AdjustNaturalArmor(mods.NaturalArmor * (add ? 1 : -1));
            }
        }

        public void AdjustNaturalArmor(int value)
        {
            int mod = Math.Max(value, -NaturalArmor);

            NaturalArmor += mod;
            FullAc += mod;
            FlatFootedAc += mod;
            AcMods = ReplaceModifierNumber(_acMods, "natural", NaturalArmor, false);

        }

        public void AdjustShield(int value)
        {

            Shield += value;
            FullAc += value;
            FlatFootedAc += value;
            AcMods = ReplaceModifierNumber(_acMods, "shield", Shield, false);

        }

        public void AdjustSizeAcModifier(int diff)
        {

            //adjust AC
            FullAc += diff;
            TouchAc += diff;
            FlatFootedAc += diff;

            int mod = FindModifierNumber(_acMods, "size");

            AcMods = ReplaceModifierNumber(_acMods, "size", mod + diff, false);
        }

        public void AdjustArmor(int value)
        {

            Armor += value;
            FullAc += value;
            FlatFootedAc += value;
            AcMods = ReplaceModifierNumber(_acMods, "armor", Armor, false);

        }

        public void AdjustDodge(int value)
        {
            Dodge += value;
            FullAc += value;
            Cmd = ChangeCmd(Cmd, value);
            TouchAc += value;
            AcMods = ReplaceModifierNumber(_acMods, "dodge", Dodge, false);

        }

        public void AdjustDeflection(int value)
        {
            Deflection += value;
            FullAc += value;
            FlatFootedAc += value;
            Cmd = ChangeCmd(Cmd, value);
            TouchAc += value;
            AcMods = ReplaceModifierNumber(_acMods, "deflection", Deflection, false);

        }

        
        public int GetCrIntWholeNumber()
        {
            try
            {
                if (Cr.Contains('/'))
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Cr);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void AdjustCr(int diff)
        {
            if (diff > 0)
            {
                if (Cr.Contains('/'))
                {
                    Cr = (diff).ToString();
                }
                else
                {
                    int crVal = int.Parse(Cr);

                    crVal += diff;

                    Cr = crVal.ToString();
                }
            }
            else
            {
                Regex crSlash = new Regex("([0-9]+)/([0-9]+)");

                Match match = crSlash.Match(Cr);

                int crInt = 1;

                if (match.Success)
                {
                    int val = int.Parse(match.Groups[2].Value);

                    if (_lowCrToIntChart.ContainsKey(val))
                    {
                        crInt = _lowCrToIntChart[val];
                    }
                }
                else
                {
                    crInt = int.Parse(Cr);
                }

                crInt += diff;

                if (crInt >= 1)
                {
                    Cr = crInt.ToString();
                }
                else
                {
                    int crValOut;

                    if (_intToLowCrChart.TryGetValue(crInt, out crValOut))
                    {
                        Cr = "1/" + crValOut.ToString();
                    }
                    else
                    {
                        Cr = "1/8";
                    }
                }

            }

            Xp = GetXpString(Cr);
        }

        [XmlIgnore]
        public int? IntCr => TryGetCrChartInt(Cr);


        public static int? TryGetCrChartInt(string crText)
        {
            if (crText == null)
            {
                return null;
            }

            Regex crSlash = new Regex("([0-9]+)/([0-9]+)");

            Match match = crSlash.Match(crText);

            int? crInt = null;

            if (match.Success)
            {
                int val = 1;

                if (int.TryParse(match.Groups[2].Value, out val))
                {

                    if (_lowCrToIntChart.ContainsKey(val))
                    {
                        crInt = _lowCrToIntChart[val];
                    }
                }
            }
            else
            {
                int cVal;
                if (int.TryParse(crText, out cVal))
                {
                    crInt = cVal;
                }
            }

            return crInt;
        }

        public static int GetCrChartInt(string crText)
        {
            Regex crSlash = new Regex("([0-9]+)/([0-9]+)");

            Match match = crSlash.Match(crText);

            int crInt = 1;

            if (match.Success)
            {
                int val = 1;

                if (int.TryParse(match.Groups[2].Value, out val))
                {

                    if (_lowCrToIntChart.ContainsKey(val))
                    {
                        crInt = _lowCrToIntChart[val];
                    }
                }
            }
            else
            {
                crInt = int.Parse(crText);
            }

            return crInt;

        }

        private static string GetXpString(string crText)
        {
            return GetCrValue(crText).ToString("#,#", CultureInfo.InvariantCulture);

        }

        public static long GetCrValue(string crText)
        {
            try
            {
                long xpVal = 0;

                if (crText.Contains('/'))
                {
                    if (_xpValues.ContainsKey(crText))
                    {
                        xpVal = _xpValues[crText];
                    }
                }
                else
                {
                    int x = int.Parse(crText);

                    xpVal = GetIntCrValue(x);
                }

                return xpVal;
            }
            catch
            {
                throw;
            }
        }

        public static long? TryGetCrValue(string crText)
        {

            long? xpVal = null;

            if (crText.Contains('/'))
            {
                if (_xpValues.ContainsKey(crText))
                {
                    xpVal = _xpValues[crText];
                }
            }
            else
            {
                int x;

                if (int.TryParse(crText, out x))
                {
                    if (x > 0)
                    {
                        xpVal = GetIntCrValue(x);
                    }
                }
            }

            return xpVal;
        }

        private static long GetAllIntCrValue(int crInt)
        {
            if (crInt < -4)
            {
                return 0;
            }
            if (crInt < 1)
            {
                return _xpValues["1/" + _intToLowCrChart[crInt]];
            }
            else
            {
                return GetIntCrValue(crInt);
            }
        }

        private static long GetIntCrValue(int crInt)
        {
            long x = crInt;

            long powVal = ((x + 1) / 2) + 1;
            long xpVal = ((long)Math.Pow(2, powVal)) * 100;
            if ((x - 1) % 2 != 0)
            {
                xpVal += xpVal / 2;
            }

            return xpVal;
        }

        public static string EstimateCr(long xp)
        {
            string cr = null;

            //get CR
            if (xp < GetAllIntCrValue(-4))
            {

            }
            else
            {
                int i = -4;


                while (!(xp >= GetAllIntCrValue(i) && xp < GetAllIntCrValue(i + 1)))
                {
                    i++;
                }

                if (i < 1)
                {
                    int denominator = _intToLowCrChart[i];

                    cr = "1/" + denominator;
                }
                else
                {
                    cr = i.ToString();
                }

            }

            return cr;
        }

        public void AdjustMr(int change)
        {
            int? startMr = _mr;
            if (_mr == null)
            {
                _mr = 0;
            }
            int startNumMr = _mr.Value;
            _mr += change;
            int diff = change;
            if (_mr <= 0)
            {
                diff = -startNumMr;
                _mr = null;
            }
            if (diff != 0)
            {
                int maxDie = HdRoll.HighestDie();
                int hpdiff = diff * maxDie;

                ChangeHdMod(hpdiff);

            }
            if (startMr != _mr)
            {

                NotifyPropertyChanged("MR");
            }

        }

        public override void AdjustDexterity(int value)
        {
            if (DexZero)
            {
                if (PreLossDex != null)
                {
                    PreLossDex += value;
                }
            }
            else
            {
                if (Dexterity != null)
                {
                    int? oldDex = Dexterity;
                    Dexterity += value;

                    ApplyDexterityAdjustments(oldDex);

                }
            }
        }

        private void ApplyDexterityAdjustments(int? oldDex)
        {

            int oldDexBonus = AbilityBonus(oldDex);
            int newDexBonus = AbilityBonus(Dexterity);

            int diff = newDexBonus - oldDexBonus;

            //adjust armor
            FullAc += diff;
            TouchAc += diff;

            if (newDexBonus < 0)
            {
                FlatFootedAc = FullAc;
            }
            else
            {
                FlatFootedAc = FullAc - newDexBonus;
            }

            AcMods = ReplaceModifierNumber(_acMods, "Dex", newDexBonus, false);

            if (AcMods != null)
            {
                if (!Regex.Match(AcMods, "\\(.*\\)").Success)
                {
                    AcMods = "(" + AcMods + ")";
                }

                if (AcMods == "()")
                {
                    AcMods = "";
                }
            }


            //adjust initiative
            Init += diff;

            //adjust save
            Ref += diff;

            //adjust CMD
            Cmd = ChangeCmd(Cmd, diff);

            //adjust attacks
            if (Ranged != null && _ranged.Length > 0)
            {
                Ranged = ChangeAttackMods(Ranged, diff);
            }

            ChangeSkillsForStat(Stat.Dexterity, diff);


        }

        public override void AdjustWisdom(int value)
        {
            if (Wisdom != null)
            {
                int? oldWis = Wisdom;
                Wisdom += value;

                ApplyWisdomAdjustments(oldWis);

            }

        }

        private void ApplyWisdomAdjustments(int? oldWis)
        {

            int oldBonus = AbilityBonus(oldWis);
            int newBonus = AbilityBonus(Wisdom);

            int diff = newBonus - oldBonus;

            //adjust perception
            if (!ChangeSkill("Perception", diff))
            {
                Senses = ChangeSkillStringMod(Senses, "Perception", diff);
            }

            //adjust save
            Will += diff;

            ChangeSkillsForStat(Stat.Wisdom, diff);
        }

        public override void AdjustIntelligence(int value)
        {
            if (Intelligence != null)
            {
                int? oldInt = Intelligence;
                Intelligence += value;

                ApplyIntelligenceAdjustments(oldInt);

            }
        }

        private void ApplyIntelligenceAdjustments(int? oldInt)
        {
            int oldBonus = AbilityBonus(oldInt);
            int newBonus = AbilityBonus(Intelligence);

            int diff = newBonus - oldBonus;


            //adjust skills
            ChangeSkillsForStat(Stat.Intelligence, diff);

        }

        public override void AdjustStrength(int value)
        {
            if (StrZero)
            {
                if (PreLossStr != null)
                {
                    PreLossStr += value;
                }
            }
            else
            {
                if (Strength != null)
                {
                    int? old = Strength;
                    Strength += value;

                    ApplyStrengthAdjustments(old);



                }
            }
        }

        private void ApplyStrengthAdjustments(int? old)
        {
            int oldBonus = AbilityBonus(old);
            int newBonus = AbilityBonus(Strength);

            int diff = newBonus - oldBonus;
            int diffPlusHalf = (newBonus + newBonus / 2) - (oldBonus + oldBonus / 2);
            int halfDiff = newBonus / 2 - oldBonus / 2;

            //adjust attacks
            if (Melee != null && Melee.Length > 0)
            {
                Melee = ChangeAttackMods(Melee, diff);
                Melee = ChangeAttackDamage(Melee, diff, diffPlusHalf, halfDiff);
            }

            if (Ranged != null && Ranged.Length > 0)
            {
                Ranged = ChangeThrownDamage(Ranged, diff, diffPlusHalf);
            }

            //adjust CMB
            Cmb = ChangeStartingModOrVal(Cmb, diff);

            //adjust CMD
            Cmd = ChangeCmd(Cmd, diff);

            //adjust skills
            ChangeSkillsForStat(Stat.Strength, diff);
        }

        public override void AdjustConstitution(int value)
        {
            if (Constitution != null)
            {
                int? old = Constitution;
                Constitution += value;

                ApplyConstitutionAdjustments(old);


            }
        }

        private void ApplyConstitutionAdjustments(int? old)
        {

            int oldBonus = AbilityBonus(old);
            int newBonus = AbilityBonus(Constitution);

            int diff = newBonus - oldBonus;

            //adjust hp/hd
            ChangeHdMod(diff);

            //adjust save
            Fort += diff;

            //adjust skills
            ChangeSkillsForStat(Stat.Constitution, diff);

        }

        public override void AdjustCharisma(int value)
        {
            if (Charisma != null)
            {
                int? old = Charisma;
                Charisma += value;

                ApplyCharismaAdjustments(old);

            }
        }

        private void ApplyCharismaAdjustments(int? old)
        {
            int oldBonus = AbilityBonus(old);
            int newBonus = AbilityBonus(Charisma);

            int diff = newBonus - oldBonus;

            if (string.Compare(Type, "Undead", true) == 0)
            {

                //adjust save
                Fort += diff;

                //adjust hd
                ChangeHdMod(diff);
            }

            //adjust skills
            ChangeSkillsForStat(Stat.Charisma, diff);

        }

        public void AdjustHd(int diff)
        {
            DieRoll roll = FindNextDieRoll(Hd, 0);

            //check for toughness
            bool toughness = FeatsList.Contains("Toughness");

            //get hp mod
            int hpMod = 0;

            if (string.Compare(Type, "undead", true) == 0)
            {
                hpMod = AbilityBonus(Charisma);
            }
            else if (string.Compare(Type, "construct", true) != 0)
            {
                hpMod = AbilityBonus(Constitution);
            }

            int applyCount = diff;

            if (roll.Count + diff < 1)
            {
                applyCount = 1 - roll.Count;
            }


            int oldCount = roll.Count;


            roll.Count += applyCount;
            roll.Mod += hpMod * applyCount;
            int toughnessExtra = 0;
            if (toughness)
            {
                if (applyCount > 0)
                {
                    int diffCount = oldCount;

                    if (diffCount < 3)
                    {
                        diffCount = 3;
                    }

                    toughnessExtra = roll.Count - diffCount;

                    if (toughnessExtra > 0)
                    {
                        roll.Mod += toughnessExtra;
                    }
                    else
                    {
                        toughnessExtra = 0;
                    }
                }
                else if (applyCount < 0)
                {
                    int newCount = roll.Count;

                    if (newCount < 3)
                    {
                        newCount = 3;
                    }

                    toughnessExtra = newCount - oldCount;

                    if (toughnessExtra > 0)
                    {
                        roll.Mod += toughnessExtra;
                    }
                    else
                    {
                        toughnessExtra = 0;
                    }

                }
            }

            Hd = "(" + DieRollText(roll) + ")";

            Hp += hpMod * applyCount + toughnessExtra;

            Hp += (applyCount * roll.Die) / 2 + applyCount / 2;

            SpellLikeAbilities = ChangeSpellLikeCl(SpellLikeAbilities, applyCount);

        }

        private static string ChangeSpellLikeCl(string text, int diff)
        {
            if (text == null)
            {
                return null;
            }

            string returnText = text;

            Regex regEx = new Regex("(CL )([0-9]+)((th)|(rd)|(nd)|(st))");

            returnText = regEx.Replace(returnText, delegate (Match m)
            {
                int cl = int.Parse(m.Groups[2].Value) + diff;


                string end = "th";

                switch (cl % 10)
                {
                    case 1:
                        end = "st";
                        break;
                    case 2:
                        end = "nd";
                        break;
                    case 3:
                        end = "rd";
                        break;
                }

                return "CL " + cl.ToString() + end;

            });


            return returnText;
        }

        private static string ChangeDarkvisionAtLeast(string text, int dist)
        {

            Regex regDark = new Regex("(darkvision )([0-9]+)( ft\\.)", RegexOptions.IgnoreCase);

            string returnText = text;
            bool bFound = false;

            returnText = regDark.Replace(text, delegate (Match m)
               {
                   bFound = true;
                   return m.Groups[1] +
                       (Math.Max(int.Parse(m.Groups[2].Value), dist)).ToString() +
                       m.Groups[3];

               }, 1);

            if (!bFound)
            {
                Match match = new Regex(";").Match(text);

                string newText = string.Format("darkvision {0} ft.", dist);

                newText += match.Success ? ", " : "; ";

                returnText = newText + returnText;
            }


            return returnText;
        }

        private static string AddSense(string text, string sense)
        {

            string returnText = text;
            if (returnText == null)
            {
                returnText = "";
            }


            Regex regSense = new Regex(Regex.Escape(sense), RegexOptions.IgnoreCase);

            if (!regSense.Match(returnText).Success)
            {
                bool bAdded = false;

                Regex regColon = new Regex(";");

                returnText = regColon.Replace(returnText, delegate (Match m)
                    {
                        bAdded = true;
                        return ", " + sense + ";";
                    }, 1);

                if (!bAdded)
                {
                    returnText = sense + "; " + returnText;
                }

            }

            return returnText;
        }

        private static string AddImmunity(string text, string type)
        {
            return AddToStringList(text, type);

        }

        private const string FlyString = "(fly )([0-9]+)( ft\\. \\()([\\p{L}]+)(\\))";

        private static string AddFlyFromMove(string text, int speedMult, string quality)
        {

            string returnText = text;

            //get speed
            Regex regName = new Regex("^[0-9]+");
            Match match = regName.Match(returnText);
            int move = 0;
            if (match.Success)
            {
                move = int.Parse(match.Value);
            }
            int flySpeed = move * speedMult;

            Regex regFly = new Regex(FlyString, RegexOptions.IgnoreCase);

            bool bAdded = false;
            returnText = regFly.Replace(returnText, delegate (Match m)
                {
                    flySpeed = Math.Max(int.Parse(m.Groups[2].Value), flySpeed);

                    bAdded = true;
                    return m.Groups[1].Value + flySpeed +
                        m.Groups[3].Value + GetMaxFlyQuality(m.Groups[4].Value, quality) + m.Groups[5].Value;
                }, 1);

            if (!bAdded)
            {
                returnText = returnText + ", fly " + flySpeed + " ft. (" + quality + ")";
            }

            return returnText;
        }

        private static string SetFlyQuality(string text, string quality)
        {
            string returnText = text;

            if (returnText != null)
            {

                Regex regFly = new Regex(FlyString, RegexOptions.IgnoreCase);

                returnText = regFly.Replace(returnText, delegate (Match m)
                {

                    return m.Groups[1].Value + m.Groups[2].Value +
                        m.Groups[3].Value + quality + m.Groups[5].Value;
                }, 1);
            }


            return returnText;
        }

        private static string RemoveFly(string text)
        {
            Regex regFly = new Regex("(, )?(fly )([0-9]+)( ft\\. \\()([\\p{L}]+)(\\))", RegexOptions.IgnoreCase);


            return regFly.Replace(text, "");
        }

        private static int GetFlyQuality(string quality)
        {
            int value;

            if (!_flyQualityList.TryGetValue(quality, out value))
            {
                value = -1;
            }

            return value;
        }

        public static string GetFlyQualityString(int val)
        {
            return _flyQualityList.First(a => a.Value == val).Key;
        }

        private static string GetMaxFlyQuality(string quality1, string quality2)
        {
            return (GetFlyQuality(quality1) > GetFlyQuality(quality2)) ? quality1 : quality2;
        }

        private bool AddSubtype(string subtype)
        {
            string work = "";
            if (SubType != null)
            {
                work = SubType.Trim(new char[] { '(', ')' });
            }


            work = AddToStringList(work, subtype, out bool added);

            if (work.Length > 0)
            {
                SubType = "(" + work + ")";
            }
            else
            {
                SubType = null;
            }

            return added;

        }

        private bool RemoveSubtype(string subtype)
        {
            bool removed = false;

            if (_subType != null && _subType.Length > 0)
            {

                string work = SubType.Trim(new char[] { '(', ')' });

                work = RemoveFromStringList(work, subtype, out removed);

                if (work.Length > 0)
                {
                    SubType = "(" + work + ")";
                }
                else
                {
                    SubType = null;
                }
            }

            return removed;
        }

        private bool HasSubtype(string subtype)
        {
            if (SubType == null)
            {
                return false;
            }

            string work = SubType.Trim(new char[] { '(', ')' });
            return StringListHasItem(work, subtype);
        }

       

        private static string AddDr(string text, string type, int val)
        {

            Regex regDr = new Regex("([0-9]+)(/ " + type + ")");

            string returnText = text;

            if (returnText == null)
            {
                returnText = "";
            }

            bool bFound = false;

            returnText = regDr.Replace(returnText, delegate (Match m)
                {
                    bFound = true;
                    return
                        (Math.Max(int.Parse(m.Groups[1].Value), val)).ToString() +
                        m.Groups[2];

                }, 1);

            if (!bFound)
            {
                if (returnText.Length > 0)
                {
                    returnText += ", ";
                }
                returnText += val.ToString() + "/" + type;

            }

            return returnText;

        }

        private static string AddAttack(string text, ViewModels.Attack attack)
        {
            ViewModels.Attack addAttack = attack;


            string returnText = text;
            if (returnText == null)
            {
                returnText = "";
            }

            Regex regAttack = new Regex(ViewModels.Attack.RegexString(attack.Name), RegexOptions.IgnoreCase);
            bool bAdded = false;

            returnText = regAttack.Replace(returnText, delegate (Match m)
                {
                    bAdded = true;
                    ViewModels.Attack foundAttack = ViewModels.Attack.ParseAttack(m);



                    addAttack.Damage.Step =
                        (AverageDamage(foundAttack.Damage.Step) > AverageDamage(addAttack.Damage.Step)) ?
                        foundAttack.Damage.Step : addAttack.Damage.Step;


                    return addAttack.Text;


                });

            if (!bAdded)
            {
                returnText += ", " + addAttack.Text;
            }

            return returnText;
        }

        private string FixMeleeAttacks(string text, int bonus, int damageMod, bool removePlus)
        {
            if (text == null)
            {
                return null;
            }

            string returnText = text;

            CharacterAttacks attacks = new CharacterAttacks(this);

            if (attacks.MeleeWeaponSets.Count > 0 || attacks.NaturalAttacks.Count > 0)
            {
                returnText = MeleeString(attacks);
            }


            return returnText;


        }

        private string FixRangedAttacks(string text, int bonus, int damageMod, bool removePlus, Monster monster)
        {

            if (text == null)
            {
                return null;
            }

            string returnText = text;

            CharacterAttacks attacks = new CharacterAttacks(this);

            if (attacks.RangedAttacks.Count > 0)
            {
                returnText = RangedString(attacks);
            }


            return returnText;
        }

        private static string SetPlusOnMeleeAttacks(string text, string plus, bool natural)
        {
            if (text == null)
            {
                return null;
            }

            string returnText = text;

            Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);

            returnText = regAttack.Replace(returnText, delegate (Match m)
            {

                ViewModels.Attack info = ViewModels.Attack.ParseAttack(m);


                if (!natural || !_weaponNameList.ContainsKey(info.Name.ToLower()))
                {
                    info.Plus = plus;
                }


                return info.Text;

            });

            return returnText;
        }

        private static double AverageDamage(DieStep step)
        {
            return AverageDamage(step.Count, step.Die);
        }

        private static double AverageDamage(int count, int die)
        {
            double val = ((double)die) / 2.0 + 0.5;

            return val * count;
        }

        private static string AddSpecialAttack(string text, string attack, int count)
        {
            Regex regSp = new Regex("(" + attack + " \\(?)([0-9]+)(/ ?day\\)?)", RegexOptions.IgnoreCase);

            string returnText = text;

            if (returnText == null)
            {
                returnText = "";
            }

            bool bFound = false;

            returnText = regSp.Replace(returnText, delegate (Match m)
                {
                    bFound = true;
                    return m.Groups[1] +
                        (int.Parse(m.Groups[2].Value) + count).ToString() +
                        m.Groups[3];

                }, 1);

            if (!bFound)
            {
                if (returnText.Length > 0)
                {
                    returnText += ", ";
                }
                returnText += attack + " (" + count + "/day)";

            }

            return returnText;
        }

        private static string AddSpellLikeAbility(string text, string ability, int count, int cl)
        {
            string returnText = text;

            bool addCl = false;
            if (returnText == null || returnText.Length == 0)
            {
                addCl = true;
            }


            returnText = AddToStringList(returnText, count + "/day-" + ability);

            if (addCl)
            {
                returnText = "(CL " + cl + ") " + returnText;
            }

            return returnText;

        }

        private static string AddSummonDr(string text, string hd, string type)
        {
            string returnText = text;

            DieRoll roll = FindNextDieRoll(hd, 0);

            if (roll.Count > 4 && roll.Count <= 10)
            {
                returnText = AddDr(returnText, type, 5);
            }
            else if (roll.Count >= 10)
            {
                returnText = AddDr(returnText, type, 10);
            }

            return returnText;
        }

        private static string AddSummonSr(string text, string cr, int extra)
        {
            string returnText = text;

            if (returnText == null)
            {
                returnText = "0";
            }


            int intCr = 0;

            if (!cr.Contains('/'))
            {
                intCr = int.Parse(cr);
            }


            int newSr = intCr + extra;

            Regex regNum = new Regex("[0-9]+");

            returnText = regNum.Replace(returnText, delegate (Match m)
                {
                    return Math.Max(int.Parse(m.Value), newSr).ToString();
                }, 1);



            return returnText;
        }

        public void AddFastHealing(int amount)
        {
            HpMods = AddTypeValToStringList(HpMods, "fast healing", amount);
        }

        private static string AddResitance(string text, string type, int val)
        {
            return AddTypeValToStringList(text, type, val);
        }

        private static string AddTypeValToStringList(string text, string type, int val)
        {

            Regex regRes = new Regex("(" + type + " )([0-9]+)", RegexOptions.IgnoreCase);

            string returnText = text;

            if (returnText == null)
            {
                returnText = "";
            }

            bool bFound = false;

            returnText = regRes.Replace(returnText, delegate (Match m)
            {
                bFound = true;
                return
                    m.Groups[1] +
                    (Math.Max(int.Parse(m.Groups[2].Value), val)).ToString();


            }, 1);

            if (!bFound)
            {
                if (returnText.Length > 0)
                {
                    returnText += ", ";
                }
                returnText += type + " " + val;

            }

            return returnText;

        }

        private static string AddPlusModtoList(string text, string type, int val)
        {
            Regex regRes = new Regex("(" + type + " )((\\+|-)[0-9]+)");

            string returnText = text;

            if (returnText == null)
            {
                returnText = "";
            }

            bool bFound = false;

            returnText = regRes.Replace(returnText, delegate (Match m)
            {
                bFound = true;
                return
                    m.Groups[1] +
                    (Math.Max(int.Parse(m.Groups[2].Value), val)).ToString();


            }, 1);

            if (!bFound)
            {
                if (returnText.Length > 0)
                {
                    returnText += ", ";
                }
                returnText += type + " " + CmStringUtilities.PlusFormatNumber(val);

            }

            return returnText;

        }

        private void ChangeHdMod(int diff)
        {

            //adjust hp
            DieRoll hdRoll = FindNextDieRoll(Hd, 0);
            if (hdRoll != null)
            {
                hdRoll.Mod += (diff * hdRoll.TotalCount) / hdRoll.Fraction;
                Hd = ReplaceDieRoll(Hd, hdRoll, 0);

                Hp += diff * hdRoll.TotalCount;
            }

        }

        public bool HasDefensiveAbility(string quality)
        {
            if (DefensiveAbilities == null)
            {
                return false;
            }

            return new Regex(Regex.Escape(quality), RegexOptions.IgnoreCase).Match(DefensiveAbilities).Success;
        }

        private void AdjustSkills(int diff)
        {
            if (Intelligence != null)
            {
                CreatureTypeInfo info = CreatureTypeInfo.GetInfo(Type);

                List<KeyValuePair<SkillValue, int>> list = GetSkillRanks();


                if (list.Count > 0 && diff != 0)
                {
                    int count = Math.Abs(diff);

                    int listNum = 0;

                    for (int i = 0; i < count; i++)
                    {
                        int extraTries = list.Count;
                        bool added = false;
                        while (!added && extraTries > 0)
                        {
                            SkillValue skillValue = list[listNum].Key;

                            if (diff > 0)
                            {
                                if (skillValue.Mod < SkillMax(skillValue.Name))
                                {
                                    skillValue.Mod++;
                                    added = true;
                                }
                            }
                            else if (diff < 0)
                            {
                                if (skillValue.Mod > SkillMin(skillValue.Name))
                                {
                                    skillValue.Mod--;
                                    added = true;

                                    if (skillValue.Mod == SkillMin(skillValue.Name) && GetRacialSkillMod(skillValue.FullName) == 0)
                                    {
                                        SkillValueDictionary.Remove(skillValue.FullName);
                                    }
                                }
                            }

                            if (!added)
                            {
                                extraTries--;
                            }

                            listNum = (listNum + 1) % list.Count;
                        }

                        if (extraTries == 0)
                        {
                            if (diff > 0)
                            {
                                foreach (string skill in info.ClassSkills)
                                {
                                    if (skill != "Knowledge")
                                    {
                                        if (!_skillValueDictionary.ContainsKey(skill))
                                        {

                                            SkillValue val = new SkillValue(skill);
                                            try
                                            {
                                                val.Mod = AbilityBonus(GetStat(SkillsList[val.Name])) +
                                                    4;
                                                _skillValueDictionary[skill] = val;
                                                added = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Diagnostics.Debug.WriteLine(val.Name);
                                                System.Diagnostics.Debug.WriteLine(ex.ToString());
                                                throw;
                                            }
                                        }
                                    }
                                }

                            }

                            if (!added)
                            {
                                break;
                            }
                        }

                    }
                }
            }

        }

        private int SkillMax(string skill)
        {
            CreatureTypeInfo info = CreatureTypeInfo.GetInfo(Type);

            return HdRoll.TotalCount + AbilityBonus(GetStat(SkillsList[skill])) + (info.IsClassSkill(skill) ? 3 : 0) + GetRacialSkillMod(skill);
        }

        private int SkillMin(string skill)
        {
            CreatureTypeInfo info = CreatureTypeInfo.GetInfo(Type);

            return AbilityBonus(GetStat(SkillsList[skill])) + (info.IsClassSkill(skill) ? 3 : 0) + GetRacialSkillMod(skill);
        }

        private int GetRacialSkillMod(string skill)
        {
            int mod = 0;

            if (RacialMods != null)
            {
                Regex regVal = new Regex("((\\+|-)[0-9]+) " + skill);


                Match m = regVal.Match(RacialMods);

                if (m.Success)
                {
                    mod = int.Parse(m.Groups[1].Value);

                }
            }


            return mod;
        }

        private List<KeyValuePair<SkillValue, int>> GetSkillRanks()
        {

            List<KeyValuePair<SkillValue, int>> list = new List<KeyValuePair<SkillValue, int>>();


            foreach (SkillValue skillValue in SkillValueDictionary.Values)
            {
                try
                {
                    int val = skillValue.Mod - AbilityBonus(GetStat(SkillsList[skillValue.Name]));

                    list.Add(new KeyValuePair<SkillValue, int>(skillValue, val));
                }

                catch (KeyNotFoundException)
                {
                    System.Diagnostics.Debug.WriteLine(Name + " Skill not found: " + skillValue.Name);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    throw;
                }

            }



            list.Sort((a, b) => b.Value - a.Value);

            return list;
        }

        public static AlignmentType ParseAlignment(string alignment)
        {
            AlignmentType type = new AlignmentType();

            if (alignment.Contains("G"))
            {
                type.Moral = MoralAxis.Good;
            }
            else if (alignment.Contains("E"))
            {
                type.Moral = MoralAxis.Evil;
            }
            else
            {
                type.Moral = MoralAxis.Neutral;
            }


            if (alignment.Contains("L"))
            {
                type.Order = OrderAxis.Lawful;
            }
            else if (alignment.Contains("C"))
            {
                type.Order = OrderAxis.Chaotic;
            }
            else
            {
                type.Order = OrderAxis.Neutral;
            }


            return type;
        }

        public static string AlignmentText(AlignmentType alignment)
        {
            if (alignment.Moral == MoralAxis.Neutral && alignment.Order == OrderAxis.Neutral)
            {
                return "N";
            }

            string text = "";

            switch (alignment.Order)
            {
                case OrderAxis.Lawful:
                    text += "L";
                    break;
                case OrderAxis.Neutral:
                    text += "N";
                    break;
                case OrderAxis.Chaotic:
                    text += "C";
                    break;
            }

            switch (alignment.Moral)
            {
                case MoralAxis.Good:
                    text += "G";
                    break;
                case MoralAxis.Neutral:
                    text += "N";
                    break;
                case MoralAxis.Evil:
                    text += "E";
                    break;
            }

            return text;

        }

        public static string CreatureTypeText(CreatureType type)
        {
            return _creatureTypeNames[type];
        }

        public static CreatureType ParseCreatureType(string name)
        {
            if (!_creatureTypes.ContainsKey(name.ToLower()))
            {
                System.Diagnostics.Debug.WriteLine("Unknow creature type.  Type: " + name);
                return _creatureTypes[_creatureTypeNames[CreatureType.Humanoid]];
            }
            return _creatureTypes[name.ToLower()];

        }

        public void BaseClone(Monster s)
        {
            foreach (ActiveCondition c in ActiveConditions)
            {
                ((BaseMonster)s).ActiveConditions.Add(new ActiveCondition(c));
            }

            foreach (Condition c in UsableConditions)
            {
                s.UsableConditions.Add(new Condition(c));
            }

            s.DexZero = DexZero;
        }


        public override void ApplyBonus(ConditionBonus bonus, bool remove)
        {
            if (bonus.Str != null && Strength != null)
            {
                AdjustStrength(remove ? -bonus.Str.Value : bonus.Str.Value);
            }
            if (bonus.Dex != null && Dexterity != null)
            {
                AdjustDexterity(remove ? -bonus.Dex.Value : bonus.Dex.Value);
            }
            if (bonus.Con != null && Constitution != null)
            {
                AdjustConstitution(remove ? -bonus.Con.Value : bonus.Con.Value);
            }
            if (bonus.Int != null && Intelligence != null)
            {
                AdjustIntelligence(remove ? -bonus.Int.Value : bonus.Int.Value);
            }
            if (bonus.Wis != null && Wisdom != null)
            {
                AdjustWisdom(remove ? -bonus.Wis.Value : bonus.Wis.Value);
            }
            if (bonus.Cha != null && Charisma != null)
            {
                AdjustCharisma(remove ? -bonus.Cha.Value : bonus.Cha.Value);
            }
            if (bonus.StrSkill != null)
            {
                ChangeSkillsForStat(Stat.Strength, remove ? -bonus.StrSkill.Value : bonus.StrSkill.Value);
            }
            if (bonus.DexSkill != null)
            {
                ChangeSkillsForStat(Stat.Dexterity, remove ? -bonus.DexSkill.Value : bonus.DexSkill.Value);
            }
            if (bonus.ConSkill != null)
            {
                ChangeSkillsForStat(Stat.Constitution, remove ? -bonus.ConSkill.Value : bonus.ConSkill.Value);
            }
            if (bonus.IntSkill != null)
            {
                ChangeSkillsForStat(Stat.Intelligence, remove ? -bonus.IntSkill.Value : bonus.IntSkill.Value);
            }
            if (bonus.WisSkill != null)
            {
                ChangeSkillsForStat(Stat.Wisdom, remove ? -bonus.WisSkill.Value : bonus.WisSkill.Value);
            }
            if (bonus.ChaSkill != null)
            {
                ChangeSkillsForStat(Stat.Charisma, remove ? -bonus.ChaSkill.Value : bonus.ChaSkill.Value);
            }
            if (bonus.LoseDex)
            {
                LoseDexBonus = !remove;
            }
            if (bonus.Armor != null)
            {
                AdjustArmor(remove ? -bonus.Armor.Value : bonus.Armor.Value);
            }
            if (bonus.Shield != null)
            {
                AdjustShield(remove ? -bonus.Shield.Value : bonus.Shield.Value);
            }
            if (bonus.Dodge != null)
            {
                AdjustDodge(remove ? -bonus.Dodge.Value : bonus.Dodge.Value);
            }
            if (bonus.NaturalArmor != null)
            {
                AdjustNaturalArmor(remove ? -bonus.NaturalArmor.Value : bonus.NaturalArmor.Value);
            }
            if (bonus.Deflection != null)
            {
                AdjustDeflection(remove ? -bonus.Deflection.Value : bonus.Deflection.Value);
            }
            if (bonus.Ac != null)
            {
                int val = (remove ? -bonus.Ac.Value : bonus.Ac.Value);
                FullAc += val;
                TouchAc += val;
                FlatFootedAc += val;
                CmdNumeric += val;
            }
            if (bonus.Initiative != null)
            {
                Init += remove ? -bonus.Initiative.Value : bonus.Initiative.Value;
            }
            if (bonus.AllAttack != null)
            {
                Melee = ChangeAttackMods(Melee, remove ? -bonus.AllAttack.Value : bonus.AllAttack.Value);
                Ranged = ChangeAttackMods(Ranged, remove ? -bonus.AllAttack.Value : bonus.AllAttack.Value);
            }
            if (bonus.MeleeAttack != null)
            {
                Melee = ChangeAttackMods(Melee, remove ? -bonus.MeleeAttack.Value : bonus.MeleeAttack.Value);
            }
            if (bonus.RangedAttack != null)
            {
                Ranged = ChangeAttackMods(Ranged, remove ? -bonus.RangedAttack.Value : bonus.RangedAttack.Value);
            }
            if (bonus.AttackDamage != null)
            {
                int val = remove ? -bonus.AttackDamage.Value : bonus.AttackDamage.Value;
                Melee = ChangeAttackDamage(Melee, val, val, val);
                Ranged = ChangeAttackDamage(Ranged, val, val, val);
            }
            if (bonus.MeleeDamage != null)
            {
                int val = remove ? -bonus.MeleeDamage.Value : bonus.MeleeDamage.Value;
                Melee = ChangeAttackDamage(Melee, val, val, val);
            }
            if (bonus.RangedDamage != null)
            {
                int val = remove ? -bonus.RangedDamage.Value : bonus.RangedDamage.Value;
                Ranged = ChangeAttackDamage(Ranged, val, val, val);
            }
            if (bonus.Fort != null)
            {
                Fort += remove ? -bonus.Fort.Value : bonus.Fort.Value;
            }
            if (bonus.Ref != null)
            {
                Ref += remove ? -bonus.Ref.Value : bonus.Ref.Value;
            }
            if (bonus.Will != null)
            {
                Will += remove ? -bonus.Will.Value : bonus.Will.Value;
            }
            if (bonus.Perception != null)
            {
                AddOrChangeSkill("Perception", remove ? -bonus.Perception.Value : bonus.Perception.Value);
            }
            if (bonus.AllSaves != null)
            {
                Fort += remove ? -bonus.AllSaves.Value : bonus.AllSaves.Value;
                Ref += remove ? -bonus.AllSaves.Value : bonus.AllSaves.Value;
                Will += remove ? -bonus.AllSaves.Value : bonus.AllSaves.Value;
            }
            if (bonus.AllSkills != null)
            {
                foreach (Stat stat in Enum.GetValues(typeof(Stat)))
                {
                    ChangeSkillsForStat(stat, remove ? -bonus.AllSkills.Value : bonus.AllSkills.Value);
                }
            }
            if (bonus.Size != null)
            {
                AdjustSize(remove ? -bonus.Size.Value : bonus.Size.Value);
            }
            if (bonus.Cmb != null)
            {
                CmbNumeric += remove ? -bonus.Cmb.Value : bonus.Cmb.Value;
            }
            if (bonus.Cmd != null)
            {
                CmdNumeric += remove ? -bonus.Cmd.Value : bonus.Cmd.Value;
            }
            if (bonus.DexZero)
            {
                DexZero = !remove;
            }
            if (bonus.StrZero)
            {
                StrZero = !remove;
            }
            if (bonus.Hp != null)
            {
                Hp += bonus.Hp.Value.NegateIf(remove);
            }
        }

        public static Stat StatFromName(string name)
        {
            Stat stat = Stat.Strength;

            if (string.Compare("Strength", name, true) == 0)
            {
                stat = Stat.Strength;
            }
            else if (string.Compare("Dexterity", name, true) == 0)
            {
                stat = Stat.Dexterity;
            }
            else if (string.Compare("Constitution", name, true) == 0)
            {
                stat = Stat.Constitution;
            }
            else if (string.Compare("Intelligence", name, true) == 0)
            {
                stat = Stat.Intelligence;
            }
            else if (string.Compare("Wisdom", name, true) == 0)
            {
                stat = Stat.Wisdom;
            }
            else if (string.Compare("Charisma", name, true) == 0)
            {
                stat = Stat.Charisma;
            }

            return stat;
        }

        public static string StatText(Stat stat)
        {
            string text = null;

            switch (stat)
            {
                case Stat.Strength:
                    text = "Strength";
                    break;
                case Stat.Dexterity:
                    text = "Dexterity";
                    break;
                case Stat.Constitution:
                    text = "Constitution";
                    break;
                case Stat.Intelligence:
                    text = "Intelligence";
                    break;
                case Stat.Wisdom:
                    text = "Wisdom";
                    break;
                case Stat.Charisma:
                    text = "Charisma";
                    break;
            }

            return text;
        }

        public static string ShortStatText(Stat stat)
        {
            string text = null;

            switch (stat)
            {
                case Stat.Strength:
                    text = "Str";
                    break;
                case Stat.Dexterity:
                    text = "Dex";
                    break;
                case Stat.Constitution:
                    text = "Con";
                    break;
                case Stat.Intelligence:
                    text = "Int";
                    break;
                case Stat.Wisdom:
                    text = "Wis";
                    break;
                case Stat.Charisma:
                    text = "Cha";
                    break;
            }

            return text;
        }




        public static Dictionary<string, Stat> SkillsList => _skillsList;

        public static Dictionary<string, SkillInfo> SkillsDetails => _skillsDetails;

        public static List<string> FlyQualityList => new(from a in _flyQualityList orderby a.Value select a.Key);

        public static List<string> CombatManeuvers { get; set; }

        private const string DieRollRegexString = "([0-9]+)(/[0-9]+)?d([0-9]+)(?<extra>(\\+([0-9]+)d([0-9]+))*)((\\+|-)[0-9]+)?";

        public static DieRoll FindNextDieRoll(string text)
        {
            return FindNextDieRoll(text, 0);
        }

        public static DieRoll FindNextDieRoll(string text, int start)
        {
            return DieRoll.FromString(text, start);
        }

        public static string ReplaceDieRoll(string text, DieRoll roll, int start)
        {
            int end;

            return ReplaceDieRoll(text, roll, start, out end);
        }

        public static string ReplaceDieRoll(string text, DieRoll roll, int start, out int end)
        {
            string returnText = text;

            end = -1;

            Regex regRoll = new Regex(DieRollRegexString);

            Match match = regRoll.Match(text, start);

            if (match.Success)
            {
                string dieText = DieRollText(roll);

                returnText = regRoll.Replace(returnText, dieText, 1, start);

                end = match.Index + dieText.Length;
            }

            return returnText;
        }

        public static string DieRollText(DieRoll roll)
        {
            if (roll == null)
            {
                return "0d0";
            }
            return roll.Text;
        }

        public bool AddFeat(string feat)
        {

            bool added = false;
            if (!FeatsList.Contains(feat))
            {
                FeatsList.Add(feat);
                added = true;
            }

            //add feats

            if (added)
            {
                ApplyFeatChanges(feat, true);

                FeatsList.Sort();

            }

            return added;
        }

        public void AdjustSize(int diff)
        {
            MonsterSize sizeOld = SizeMods.GetSize(Size);
            MonsterSize sizeNew = SizeMods.ChangeSize(sizeOld, diff);

            SizeMods oldMods = SizeMods.GetMods(sizeOld);
            SizeMods newMods = SizeMods.GetMods(sizeNew);

            //change size text
            Size = newMods.Name;

            //change skills
            ChangeSkill("Fly", newMods.Fly - oldMods.Fly);
            ChangeSkill("Stealth", newMods.Stealth - oldMods.Stealth);


            //adjust CMB
            CmbNumeric += newMods.Combat - oldMods.Combat;

            //adjust CMD
            CmdNumeric += newMods.Combat - oldMods.Combat;

            //adjust attacks
            int attackDiff = newMods.Attack - oldMods.Attack;
            if (Melee != null && Melee.Length > 0)
            {
                Melee = ChangeAttackMods(Melee, attackDiff);
                Melee = ChangeAttackDieStep(Melee, diff);
            }

            if (Ranged != null && Ranged.Length > 0)
            {
                Ranged = ChangeAttackMods(Ranged, attackDiff);
                Ranged = ChangeAttackDieStep(Ranged, diff);
            }

            //adjust size AC modfiier
            AdjustSizeAcModifier(attackDiff);



            //adjust reach
            if (Reach != null && Reach.Length > 0)
            {
                Reach = ChangeReachForSize(Reach, sizeOld, sizeNew, diff);
            }


            //adjust space
            Regex regSwarm = new Regex("swarm");
            if (SubType == null || !regSwarm.Match(SubType).Success)
            {
                Space = newMods.Space;
            }
        }

        protected string ChangeAttackMods(string text, int diff)
        {
            if (text == null)
            {
                return null;
            }
            string returnText = text;

            if (returnText.IndexOf(" or ") > 0)
            {
                List<string> orAttacks = new List<string>();
                foreach (string subAttack in returnText.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries))
                    orAttacks.Add(ChangeAttackMods(subAttack, diff));

                return string.Join(" or ", orAttacks.ToArray());
            }

            //if (returnText.IndexOf(" and ") > 0)
            //{
            //    List<string> andAttacks = new List<string>();
            //    foreach (string subAttack in returnText.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries))
            //        andAttacks.Add(ChangeAttackMods(subAttack, diff));

            //    return string.Join(" and ", andAttacks.ToArray());
            //}			

            //find mods 
            return ChangeSingleAttackMods(diff, returnText);
        }

        private static string ChangeSingleAttackMods(int diff, string returnText)
        {
            Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);

            returnText = regAttack.Replace(returnText, delegate (Match m)
            {
                ViewModels.Attack info = ViewModels.Attack.ParseAttack(m);
                //get mod
                for (int i = 0; i < info.Bonus.Count; i++)
                {
                    info.Bonus[i] += diff;
                }

                return info.Text;

            });
            return returnText;
        }

        private static string ChangeAttackDieStep(string text, int diff)
        {
            if (text == null)
            {
                return null;
            }
            string returnText = text;

            if (returnText.IndexOf(" or ") > 0)
            {
                List<string> orAttacks = new List<string>();
                foreach (string subAttack in returnText.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries))
                    orAttacks.Add(ChangeAttackDieStep(subAttack, diff));

                return string.Join(" or ", orAttacks.ToArray());
            }

            //if (returnText.IndexOf(" and ") > 0)
            //{
            //    List<string> andAttacks = new List<string>();
            //    foreach (string subAttack in returnText.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries))
            //        andAttacks.Add(ChangeAttackDieStep(subAttack, diff));

            //    return string.Join(" and ", andAttacks.ToArray());
            //}

            //find mods 
            return ChangeSingleAttackDieStep(returnText, diff);
        }

        private static string ChangeSingleAttackDieStep(string text, int diff)
        {
            if (text == null)
            {
                return null;
            }

            string returnText = text;

            Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);

            returnText = regAttack.Replace(returnText, delegate (Match m)
            {
                ViewModels.Attack info = ViewModels.Attack.ParseAttack(m);


                info.Damage = DieRoll.StepDie(info.Damage, diff);

                return info.Text;

            });

            return returnText;

        }

        public string ChangeAttackDamage(string text, int diff, int diffPlusHalf, int halfDiff)
        {

            if (text == null)
            {
                return null;
            }

            string returnText = text;

            if (text.IndexOf(" or ") > 0)
            {
                List<string> orAttack = new List<string>();
                foreach (string subAttack in text.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries))
                    orAttack.Add(ChangeAttackDamage(subAttack, diff, diffPlusHalf, halfDiff));

                return string.Join(" or ", orAttack.ToArray());
            }

            //if (text.IndexOf(" and ") > 0)
            //{
            //    List<string> andAttack = new List<string>();
            //    foreach (string subAttack in text.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries))
            //        andAttack.Add(ChangeAttackDamage(subAttack, diff, diffPlusHalf, halfDiff));

            //    return string.Join(" and ", andAttack.ToArray());
            //}

            return ChangeAttack(diff, diffPlusHalf, halfDiff, returnText);
        }

        private string ChangeAttack(int diff, int diffPlusHalf, int halfDiff, string returnText)
        {
            Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);

            returnText = regAttack.Replace(returnText, delegate (Match m)
            {
                ViewModels.Attack info = ViewModels.Attack.ParseAttack(m);

                if (!info.AltDamage)
                {
                    int applyDiff = diff;

                    if (info.Weapon != null)
                    {
                        if (info.Weapon.Class == "Natual" && info.Weapon.Light)
                        {
                            applyDiff = halfDiff;
                        }

                        else if (info.Weapon.Hands == "Two-Handed")
                        {
                            applyDiff = diffPlusHalf;
                        }
                    }


                    info.Damage.Mod += applyDiff;

                    if (info.OffHandDamage != null)
                    {
                        if (HasFeat("Double Slice") || HasSpecialAbility("Superior Two-Weapon Fighting"))
                        {
                            info.OffHandDamage.Mod += applyDiff;
                        }
                        else
                        {
                            info.OffHandDamage.Mod += halfDiff;
                        }

                    }

                }

                return info.Text;

            });
            return returnText;
        }

        private static string ChangeReachForSize(string reach, MonsterSize sizeOld, MonsterSize sizeNew, int diff)
        {

            int units = 0;


            if (sizeNew == MonsterSize.Tiny ||
               sizeNew == MonsterSize.Diminutive ||
               sizeNew == MonsterSize.Fine)
            {
                units = 0;
            }
            else if ((int)sizeOld < (int)MonsterSize.Small)
            {

                if (sizeNew == MonsterSize.Small)
                {
                    units = 1;
                }
                else
                {
                    units = ((int)sizeNew) - (int)MonsterSize.Small;
                }
            }
            else
            {


                Regex numReg = new Regex("[0-9]+");

                Match match = numReg.Match(reach);

                units = int.Parse(match.Value) / 5 + diff;


                if (sizeNew == MonsterSize.Small)
                {
                    units++;
                }
                else if (sizeOld == MonsterSize.Small)
                {
                    units--;
                }
            }


            int reachInt = units * 5;
            return string.Format("{0} ft.", reachInt);

        }

        public bool RemoveFeat(string feat)
        {
            bool removed = false;
            if (FeatsList.Contains(feat))
            {
                FeatsList.Remove(feat);
                removed = true;
            }

            //add feats

            if (removed)
            {
                ApplyFeatChanges(feat, false);

                FeatsList.Sort();

            }

            return removed;
        }

        public string MeleeString(CharacterAttacks attacks)
        {
            string text = "";


            //find combat feats
            CombatFeats cf = GetCombatFeats();

            int offHandAttacks = 1 + (cf.ImprovedTwoWeaponFighting ? 1 : 0) + (cf.GreaterTwoWeaponFighting ? 1 : 0);


            bool firstSet = true;

            List<List<WeaponItem>> setList = new List<List<WeaponItem>>();

            foreach (List<WeaponItem> list in attacks.MeleeWeaponSets)
            {
                if (list.Count > 0)
                {
                    setList.Add(list);
                }
            }




            if (attacks.NaturalAttacks.Count > 0)
            {
                setList.Add(new List<WeaponItem>());
            }



            foreach (List<WeaponItem> set in setList)
            {
                List<ViewModels.Attack> attackSet = new List<ViewModels.Attack>();

                //determine if we are have multiple attacks
                bool hasOff = false;
                bool hasHeavyOff = false;
                foreach (WeaponItem item in set)
                {
                    if (item.Weapon.Hands == "Double" || !item.MainHand)
                    {
                        hasOff = true;
                    }
                    if (!item.MainHand && !item.Weapon.Light)
                    {
                        //ignore special cases
                        if (!(string.Compare(item.Name, "Whip") == 0 && cf.WhipMastery))
                        {
                            hasHeavyOff = true;
                            break;
                        }
                    }
                }

                int handsUsed = 0;
                foreach (WeaponItem item in set)
                {

                    //find feats for atttack
                    AttackFeats af = GetAttackFeats(item);

                    //create attack
                    ViewModels.Attack attack = StartAttackFromItem(item, af, cf);

                    //get hand related bonus
                    int handMod = 0;
                    int offHandMod = 0;
                    GetHandMods(hasOff, hasHeavyOff, item, cf, out handMod, out offHandMod);

                    //add bonuses
                    attack.Bonus = new List<int>();
                    int baseBonus = BaseAtk;
                    bool firstBonus = true;
                    int count = 0;
                    while ((baseBonus > 0 || firstBonus) && (item.MainHand || count < offHandAttacks || cf.SuperiorTwoWeaponFighting))
                    {
                        attack.Bonus.Add(AttackBonus(baseBonus, handMod, item, cf, af));

                        if (item.Weapon.Double)
                        {
                            if (cf.SuperiorTwoWeaponFighting || count < offHandAttacks)
                            {
                                attack.Bonus.Add(AttackBonus(baseBonus, offHandMod, item, cf, af));
                            }

                        }

                        if (firstBonus && item.HasSpecialAbility("speed"))
                        {
                            attack.Bonus.Add(AttackBonus(baseBonus, 0, item, cf, af));
                        }

                        baseBonus -= 5;
                        firstBonus = false;
                        count++;

                    }

                    //set damage

                    if (!item.NoMods)
                    {
                        SetAttackDamageMod(attack, item, af, cf, false, false);
                    }

                    //add to set
                    attackSet.Add(attack);

                    //add hands
                    handsUsed += item.Weapon.HandsUsed;
                }

                bool hasWeaponAttack = (handsUsed > 0);

                int handsToGive = (attacks.Hands - handsUsed);

                foreach (WeaponItem item in attacks.NaturalAttacks)
                {
                    bool useAttack = true;
                    int useCount = item.Count;

                    //skip attack if hand full
                    if (item.Weapon.IsHand && handsUsed > 0)
                    {

                        useAttack = false;
                        handsUsed -= item.Count;

                        if (handsToGive > 0)
                        {
                            handsToGive -= item.Count;
                            useAttack = true;

                            if (handsToGive < 0)
                            {
                                useCount = item.Count + handsToGive;
                            }
                        }

                    }

                    if (useAttack)
                    {
                        //find feats for atttack
                        AttackFeats af = GetAttackFeats(item);

                        //create attack
                        ViewModels.Attack attack = StartAttackFromItem(item, af, cf);

                        //set count
                        attack.Count = useCount;

                        //get hand related bonus
                        int handMod = 0;
                        if (hasWeaponAttack)
                        {
                            if (cf.MultiAttack)
                            {
                                handMod = -2;
                            }
                            else
                            {
                                handMod = -5;
                            }
                        }
                        else
                        {
                            if (item.Weapon.Light && (attacks.NaturalAttacks.Count > 1))
                            {
                                if (cf.MultiAttack)
                                {
                                    handMod = -2;
                                }
                                else
                                {
                                    handMod = -5;
                                }
                            }
                        }


                        attack.Bonus = new List<int>();
                        int baseBonus = BaseAtk;

                        attack.Bonus.Add(AttackBonus(baseBonus, handMod, item, cf, af));

                        //set damage
                        if (!item.NoMods)
                        {
                            SetAttackDamageMod(attack, item, af, cf, !((attacks.NaturalAttacks.Count > 1) || (item.Count > 1)) && set.Count == 0, set.Count > 0);
                        }
                        //add to set
                        attackSet.Add(attack);
                    }
                }

                if (attackSet.Count > 0)
                {
                    //add text to string
                    if (!firstSet)
                    {
                        text += " or ";
                    }

                    bool firstAttack = true;
                    foreach (ViewModels.Attack atk in attackSet)
                    {
                        if (firstAttack)
                        {
                            firstAttack = false;
                        }
                        else
                        {
                            text += ", ";
                        }

                        text += atk.Text;
                    }

                    firstSet = false;
                }
            }

            return text;
        }

        public string RangedString(CharacterAttacks attacks)
        {
            string text = null;


            if (attacks.RangedWeapons != null && attacks.RangedWeapons.Count > 0)
            {

                //find combat feats
                CombatFeats cf = GetCombatFeats();

                List<ViewModels.Attack> list = new List<ViewModels.Attack>();



                foreach (WeaponItem item in attacks.RangedWeapons)
                {
                    AttackFeats af = GetAttackFeats(item);


                    ViewModels.Attack attack = StartAttackFromItem(item, af, cf);


                    attack.Bonus = new List<int>();
                    int baseBonus = BaseAtk;
                    bool firstBonus = true;
                    int count = 0;
                    while ((!item.Weapon.Throw && baseBonus > 0) || firstBonus)
                    {
                        attack.Bonus.Add(AttackBonus(baseBonus, 0, item, cf, af));

                        if (firstBonus && item.HasSpecialAbility("speed"))
                        {
                            attack.Bonus.Add(AttackBonus(baseBonus, 0, item, cf, af));
                        }

                        baseBonus -= 5;
                        firstBonus = false;
                        count++;

                    }

                    SetRangedAttackDamageMod(attack, item, af, cf);


                    list.Add(attack);
                }


                text = "";
                bool firstAttack = true;
                foreach (ViewModels.Attack atk in list)
                {
                    if (firstAttack)
                    {
                        firstAttack = false;
                    }
                    else
                    {
                        text += " or ";
                    }

                    text += atk.Text;
                }


            }

            return text;
        }

        private struct AttackFeats
        {
            public bool WeaponFocus;
            public bool WeaponSpecialization;
            public bool GreaterWeaponFocus;
            public bool GreaterWeaponSpecialization;
            public bool ImprovedCritical;
            public bool ImprovedNaturalAttack;
            public int WeaponTraining;
        }

        private AttackFeats GetAttackFeats(WeaponItem item)
        {
            AttackFeats af = new AttackFeats();

            string plural = item.Name + "s";

            if (item.Weapon.Plural != null && item.Weapon.Plural.Length > 0)
            {
                plural = item.Weapon.Plural;
            }

            af.WeaponFocus = HasFeat("Weapon Focus", item.Weapon.Name) || HasFeat("Weapon Focus", plural);
            af.WeaponSpecialization = HasFeat("Weapon Specialization", item.Weapon.Name) || HasFeat("Weapon Specialization", plural);
            af.GreaterWeaponFocus = HasFeat("Greater Weapon Focus", item.Weapon.Name) || HasFeat("Greater Weapon Focus", plural);
            af.GreaterWeaponSpecialization = HasFeat("Greater Weapon Specialization", item.Weapon.Name) || HasFeat("Greater Weapon Specialization", plural);
            af.ImprovedCritical = HasFeat("Improved Critical", item.Weapon.Name) || HasFeat("Improved Critical", plural);
            af.ImprovedNaturalAttack = HasFeat("Improved Natural Attack", item.Weapon.Name) || HasFeat("Improved Natural Attack", plural);
            if (item.Weapon.Natural)
            {
                af.WeaponTraining = HasWeaponTraining("natural");
            }
            else
            {
                if (item.Weapon.Groups != null)
                {
                    foreach (string group in item.Weapon.Groups)
                    {
                        af.WeaponTraining = Math.Max(af.WeaponTraining, HasWeaponTraining(group));
                    }
                }
            }

            return af;
        }

        private struct CombatFeats
        {
            public bool MultiweaponFighting;
            public bool TwoWeaponFighting;
            public bool ImprovedTwoWeaponFighting;
            public bool GreaterTwoWeaponFighting;
            public bool SuperiorTwoWeaponFighting;
            public bool DoubleSlice;
            public bool MultiAttack;
            public bool WeaponFinesse;
            public bool WhipMastery;
            public bool SavageBite;
            public bool PowerfulBite;
            public bool RockThrowing;
            public bool IsDragon;
        }

        private CombatFeats GetCombatFeats()
        {
            CombatFeats cf = new CombatFeats();

            cf.MultiweaponFighting = HasFeat("Multiweapon Fighting");
            cf.TwoWeaponFighting = HasFeat("Two-Weapon Fighting");
            cf.ImprovedTwoWeaponFighting = HasFeat("Improved Two-Weapon Fighting");
            cf.GreaterTwoWeaponFighting = HasFeat("Greater Two-Weapon Fighting");
            cf.SuperiorTwoWeaponFighting = HasSpecialAbility("Superior Two-Weapon Fighting");
            cf.DoubleSlice = HasFeat("Double Slice");
            cf.MultiAttack = HasFeat("Multiattack");
            cf.WeaponFinesse = HasFeat("Weapon Finesse");
            cf.WhipMastery = HasSq("Whip Mastery");
            cf.SavageBite = HasSpecialAbility("Savage Bite");
            cf.PowerfulBite = HasSq("powerful bite");
            cf.RockThrowing = HasSpecialAttack("Rock Throwing");
            if (Name != null)
            {
                cf.IsDragon = new Regex(DragonRegexString, RegexOptions.IgnoreCase).Match(Name).Success;
            }
            return cf;
        }

        private static string DragonRegexString => "((Blue)|(Black)|(Green)|(Red)|(White)|(Brass)|(Bronze)|(Copper)|(Gold)|(Silver)) Dragon";

        private ViewModels.Attack StartAttackFromItem(WeaponItem item, AttackFeats af, CombatFeats cf)
        {
            ViewModels.Attack attack = new ViewModels.Attack();
            attack.Weapon = item.Weapon;

            attack.CritMultiplier = item.Broken ? 2 : item.Weapon.CritMultiplier;
            attack.CritRange = item.Broken ? 20 : item.Weapon.CritRange;


            if (string.Compare(attack.Name, "Bite", true) == 0 && cf.SavageBite)
            {
                attack.CritRange -= 1;
            }

            if (af.ImprovedCritical || item.SpecialAbilitySet.ContainsKey("keen"))
            {
                attack.CritRange = 20 - (20 - attack.CritRange) * 2 - 1;
            }

            attack.Name = item.Name.ToLower();

            attack.MagicBonus = item.MagicBonus;
            attack.Masterwork = item.Masterwork;
            attack.Broken = item.Broken;
            attack.SpecialAbilities = item.SpecialAbilities;
            attack.Plus = item.PlusText;

            attack.RangedTouch = item.Weapon.RangedTouch;
            attack.AltDamage = item.Weapon.AltDamage;
            attack.AltDamageStat = item.Weapon.AltDamageStat;
            attack.AltDamageDrain = item.Weapon.AltDamageDrain;
            attack.TwoHanded = item.TwoHanded;

            SetAttackDamageDie(item, attack, af, cf);

            return attack;
        }

        private void SetAttackDamageDie(WeaponItem item, ViewModels.Attack attack, AttackFeats af, CombatFeats cf)
        {
            MonsterSize size = SizeMods.GetSize(Size);

            attack.Damage = FindNextDieRoll(attack.Weapon.DmgM, 0);
            attack.Count = item.Count;

            if (item.Step == null)
            {

                if (attack.Damage != null && !item.NoMods)
                {
                    attack.Damage = DieRoll.StepDie(attack.Damage, ((int)size) - (int)MonsterSize.Medium);
                }
                else
                {
                    attack.Damage = new DieRoll(0, 0, 0);
                }

                if (af.ImprovedNaturalAttack)
                {
                    attack.Damage = DieRoll.StepDie(attack.Damage, 1);
                }

            }
            else
            {
                attack.Damage.Step = item.Step;
            }
        }

        private int WeaponStrengthDamageBonus(ViewModels.Attack attack, WeaponItem item, AttackFeats af, CombatFeats cf, bool onlyNatural, bool makeSecondary)
        {

            int strDamageBonus = AbilityBonus(Strength);

            if (cf.PowerfulBite && string.Compare(attack.Name, "Bite", true) == 0 && !makeSecondary)
            {
                strDamageBonus = AbilityBonus(Strength) * 2;
            }
            else if (((cf.SavageBite || cf.IsDragon) && string.Compare(attack.Name, "Bite", true) == 0) ||
                (cf.IsDragon && ((string.Compare(attack.Name, "Tail", true) == 0) || (string.Compare(attack.Name, "Tail Slap", true) == 0))) ||
                attack.TwoHanded || item.TwoHanded || (onlyNatural && AbilityBonus(Strength) > 0) && !makeSecondary)
            {

                strDamageBonus += AbilityBonus(Strength) / 2;
            }
            else if (attack.Weapon.Light && !onlyNatural && !item.MainHand || makeSecondary)
            {
                strDamageBonus = AbilityBonus(Strength) / 2;
            }

            return strDamageBonus;
        }

        private int MeleeAbilityAttackBonus(WeaponItem item, CombatFeats cf)
        {
            int abilityAttackBonus = AbilityBonus(Strength);

            if (cf.WeaponFinesse && item.Weapon.WeaponFinesse)
            {
                abilityAttackBonus = Math.Max(abilityAttackBonus, AbilityBonus(Dexterity));
            }

            return abilityAttackBonus;

        }

        private int RangedAbilityAttackBonus(WeaponItem item, CombatFeats cf)
        {
            int abilityAttackBonus = AbilityBonus(Dexterity);

            return abilityAttackBonus;

        }

        private void SetAttackDamageMod(ViewModels.Attack attack, WeaponItem item, AttackFeats af, CombatFeats cf, bool onlyNatural, bool makeSecondary)
        {

            int strDamageBonus;



            strDamageBonus = WeaponStrengthDamageBonus(attack, item, af, cf, onlyNatural, makeSecondary);


            attack.Damage.Mod = strDamageBonus + WeaponSpecialBonus(attack, af);

            if (item.Weapon.Double)
            {
                attack.OffHandDamage = (DieRoll)attack.Damage.Clone();

                if (!cf.DoubleSlice && !cf.SuperiorTwoWeaponFighting)
                {
                    attack.OffHandDamage.Mod = WeaponStrengthDamageBonus(attack, item, af, cf, false, true);
                }
            }
        }

        private void SetRangedAttackDamageMod(ViewModels.Attack attack, WeaponItem item, AttackFeats af, CombatFeats cf)
        {
            int strDamageBonus = 0;

            if ((string.Compare(attack.Name, "Rock", true) == 0 && cf.RockThrowing))
            {
                strDamageBonus = AbilityBonus(Strength) + AbilityBonus(Strength) / 2;
            }
            else if (attack.Weapon.Throw || new Regex("composite", RegexOptions.IgnoreCase).Match(attack.Name).Success)
            {
                strDamageBonus = WeaponStrengthDamageBonus(attack, item, af, cf, false, false);
            }


            attack.Damage.Mod = strDamageBonus + WeaponSpecialBonus(attack, af);
        }

        private int WeaponSpecialBonus(ViewModels.Attack attack, AttackFeats af)
        {
            return attack.MagicBonus + (attack.Broken ? -2 : 0) +
                (af.WeaponSpecialization ? 2 : 0) + (af.GreaterWeaponSpecialization ? 2 : 0) + af.WeaponTraining;
        }

        private int AttackBonus(int baseBonus, int handMod, WeaponItem item, CombatFeats cf, AttackFeats af)
        {

            MonsterSize size = SizeMods.GetSize(Size);
            SizeMods mods = SizeMods.GetMods(size);

            int abilityAttackBonus = 0;

            if (item.Weapon.Ranged)
            {
                abilityAttackBonus = RangedAbilityAttackBonus(item, cf);
            }
            else
            {
                abilityAttackBonus = MeleeAbilityAttackBonus(item, cf);
            }

            return baseBonus + abilityAttackBonus + handMod + mods.Attack +
                            (item.Masterwork ? 1 : 0) + item.MagicBonus + (item.Broken ? -2 : 0) +
                            (af.WeaponFocus ? 1 : 0) + (af.GreaterWeaponFocus ? 1 : 0) + af.WeaponTraining;

        }

        void GetHandMods(bool hasOff, bool hasHeavyOff, WeaponItem item, CombatFeats cf, out int handMod, out int offHandMod)
        {
            handMod = 0;
            offHandMod = 0;

            if (hasOff && !cf.SuperiorTwoWeaponFighting)
            {

                offHandMod = -8;
                if (hasHeavyOff)
                {
                    offHandMod -= 2;
                }
                if (cf.TwoWeaponFighting || cf.MultiweaponFighting)
                {
                    offHandMod += 6;
                }

                if (item.MainHand)
                {
                    handMod = -4;
                    if (hasHeavyOff)
                    {
                        handMod -= 2;
                    }
                    if (cf.TwoWeaponFighting || cf.MultiweaponFighting)
                    {
                        handMod += 2;
                    }
                }
                else
                {
                    handMod = offHandMod;
                }
            }
        }

        public bool HasFeat(string feat)
        {
            return HasFeat(feat, null);
        }

        public bool HasFeat(string feat, string subtype)
        {
            string text = feat;
            if (subtype != null)
            {
                text = text + " (" + subtype + ")";
            }

            return FeatsList.Contains(text, new InsensitiveEqualityCompararer());
        }

        public bool HasSq(string quality)
        {
            if (Sq == null)
            {
                return false;
            }

            return new Regex(Regex.Escape(quality), RegexOptions.IgnoreCase).Match(Sq).Success;
        }

        public void AddWeaponTraining(string group, int val)
        {
            if (Sq == null || Sq.Length == 0)
            {
                Sq = "weapon training (" + group.ToLower() + " +" + val + ")";
            }
            else
            {
                Regex regWt = new Regex("(?<start>, )?Weapon Training \\((?<values>([ \\p{L}]+ \\+[0-9]+,?)+)\\)");

                bool foundWeaponTraining = false;

                Sq = regWt.Replace(Sq, delegate (Match m)
                {
                    foundWeaponTraining = true;

                    string retString = "";

                    if (m.Groups["start"].Success)
                    {
                        retString += m.Groups["start"].Value;
                    }

                    Regex regValues = new Regex("(?<name>[ \\p{L}]+ \\+(?<val>[0-9]+))");

                    bool weaponFound = false;
                    retString += "weapon training (" + regValues.Replace(m.Groups["values"].Value, delegate (Match ma)
                    {
                        string valueString = null;

                        if (ma.Groups["name"].Value.Trim().ToLower() == group.ToLower())
                        {
                            weaponFound = true;

                            valueString = ma.Groups["name"] + " +" + Math.Max(int.Parse(ma.Groups["val"].Value), val);
                        }
                        else
                        {
                            valueString = ma.Value;
                        }

                        return valueString;

                    });

                    if (!weaponFound)
                    {
                        retString += ", " + group.ToLower();
                    }

                    retString += ")";



                    return retString;
                }, 1);



                if (!foundWeaponTraining)
                {
                    Sq += ", weapon training (" + group.ToLower() + " +" + val + ")";
                }


            }


        }

        public int HasWeaponTraining(string group)
        {
            int val = 0;

            if (Sq != null && Sq.Length > 0)
            {
                Regex regWt = new Regex("(?<start>, )?weapon training \\((?<values>([ \\p{L}]+ \\+[0-9]+,?)+)\\)", RegexOptions.IgnoreCase);

                Match m = regWt.Match(Sq);

                if (m.Success)
                {
                    Regex regValues = new Regex(Regex.Escape(group) + " \\+(?<val>[0-9]+)", RegexOptions.IgnoreCase);


                    m = regValues.Match(m.Groups["values"].Value);

                    if (m.Success)
                    {
                        val = int.Parse(m.Groups["val"].Value);
                    }

                }
            }

            return val;
        }

        public bool HasSpecialAttack(string name)
        {
            if (SpecialAttacks == null)
            {
                return false;
            }

            return new Regex(Regex.Escape(name), RegexOptions.IgnoreCase).Match(SpecialAttacks).Success;
        }

        public bool HasSpecialAbility(string name)
        {
            if (SpecialAbilitiesList == null)
            {
                return false;
            }

            foreach (SpecialAbility a in SpecialAbilitiesList)
            {
                if (string.Compare(a.Name, name, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        private void TestTwoHandedFromText(ViewModels.Attack attack)
        {
            // Determine if being used two-handed.
            if (attack.Weapon.Hands.Equals("One-Handed", StringComparison.InvariantCultureIgnoreCase))
            {
                int strMod = AbilityBonus(Strength);
                if (strMod > 0)
                {
                    strMod += strMod / 2;

                    if ((attack.Damage.Mod - attack.MagicBonus).Equals(strMod))
                        attack.TwoHanded = true;
                }
            }
        }

        public void ChangeSkillsForStat(Stat stat, int diff)
        {

            foreach (SkillValue skill in SkillValueDictionary.Values)
            {
                Stat skillStat;
                if (SkillsList.TryGetValue(skill.Name, out skillStat))
                {
                    if (skillStat == stat)
                    {
                        skill.Mod += diff;

                        UpdateSkillFields(skill);

                    }
                }
            }

        }

        public void ParseSpellLikeAbilities()
        {
            if (_spellLikeAbilitiesBlock == null && _spellLikeAbilities != null && _spellLikeAbilities.Length > 0)
            {
                _spellLikeAbilitiesBlock = SpellBlockInfo.ParseInfo(_spellLikeAbilities);
            }

        }

        public void ParseSpellsPrepared()
        {

            if (_spellsPreparedBlock == null && _spellsPrepared != null && _spellsPrepared.Length > 0)
            {
                _spellsPreparedBlock = SpellBlockInfo.ParseInfo(_spellsPrepared);
            }
        }

        public void ParseSpellsKnown()
        {

            if (_spellsKnownBlock == null && _spellsKnown != null && _spellsKnown.Length > 0)
            {
                _spellsKnownBlock = SpellBlockInfo.ParseInfo(_spellsKnown);
            }
        }

        private void ParseStats()
        {
            _strength = ParseStat("Str", _abilitiyScores);
            _dexterity = ParseStat("Dex", _abilitiyScores);
            _constitution = ParseStat("Con", _abilitiyScores);
            _intelligence = ParseStat("Int", _abilitiyScores);
            _wisdom = ParseStat("Wis", _abilitiyScores);
            _charisma = ParseStat("Cha", _abilitiyScores);
            _statsParsed = true;
        }

        private int? ParseStat(string stat, string text)
        {
            int? res = null;

            if (text != null)
            {

                Regex regEnd = new Regex(",|$");
                Regex regStat = new Regex(stat);

                Match start = regStat.Match(text);
                if (start.Success)
                {
                    int matchEnd = start.Index + start.Length;
                    Match end = regEnd.Match(text, matchEnd);

                    if (end.Success)
                    {
                        int val = 0;
                        if (int.TryParse(text.Substring(matchEnd, end.Index - matchEnd).Trim(), out val))
                        {
                            res = val;
                        }

                    }
                }
            }

            return res;

        }

        public void UpdateSkillValueList()
        {

            _skillValueList.Clear();
            _skillValueList.AddRange(_skillValueDictionary.Values);

            foreach (SkillValue v in _skillValueList)
            {
                v.PropertyChanged += SkillValuePropertyChanged;
            }
        }

        public void CreateSkillString()
        {
            if (SkillValueDictionary.Count > 0)
            {
                string skillList = "";

                int count = 0;

                foreach (SkillValue val in SkillValueDictionary.Values)
                {
                    if (count > 0)
                    {
                        skillList += ", ";
                    }

                    skillList += val.Text;
                    count++;
                }
                Skills = skillList;
            }
        }

        void SkillValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SkillValue v = (SkillValue)sender;
            UpdateSkillFields(v);
        }



        [XmlIgnore]
        public int Perception
        {
            get
            { 

                int perception;
                if (SkillValueDictionary.ContainsKey("Perception"))
                {
                    perception = SkillValueDictionary["Perception"].Mod;
                }
                else
                {
                    perception = AbilityBonus(Wisdom);
                }

                return perception;
            }
            set
            {
                int diff = value - Perception;

                if (!ChangeSkill("Perception", diff))
                {
                    Senses = ChangeSkillStringMod(Senses, "Perception", diff);
                }
            }
        }


         
        public ObservableCollection<Condition> UsableConditions
        {
            get
            {
                if (_usableConditions == null)
                {
                    _usableConditions = new ObservableCollection<Condition>();
                }

                if (!_usableConditionsParsed && SpecialAbilitiesList != null)
                {
                    ParseUsableConditions();
                }

                return _usableConditions;

            }
            set
            {
                if (_usableConditions != value)
                {
                    _usableConditions = value;
                    NotifyPropertyChanged("UsableConditions");


                }
            }
        }

        [DataMember]
        public bool LoseDexBonus
        {
            get => _loseDexBonus;
            set
            {
                if (_loseDexBonus != value)
                {

                    _loseDexBonus = value;

                    NotifyPropertyChanged("LoseDexBonus");
                }
            }
        }

        [DataMember]
        public bool DexZero
        {
            get => _dexZero;
            set
            {
                if (_dexZero != value)
                {

                    if (value)
                    {
                        _preLossDex = Dexterity;
                        if (Dexterity != null)
                        {
                            AdjustDexterity(-Dexterity.Value);
                        }
                        _dexZero = true;
                    }
                    else
                    {
                        _dexZero = false;
                        if (_preLossDex != null)
                        {
                            AdjustDexterity(_preLossDex.Value);
                        }
                    }

                    NotifyPropertyChanged("DexZero");
                }
            }
        }

        [DataMember]
        public bool StrZero
        {
            get => _strZero;
            set
            {
                if (_strZero != value)
                {

                    if (value)
                    {
                        _preLossStr = Strength;
                        if (Strength != null)
                        {
                            AdjustStrength(-Strength.Value);
                        }
                        _strZero = true;
                    }
                    else
                    {
                        _strZero = false;
                        if (_preLossStr != null)
                        {
                            AdjustStrength(_preLossStr.Value);
                        }
                    }

                    NotifyPropertyChanged("StrZero");
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
        public int? PreLossDex
        {
            get => _preLossDex;
            set
            {
                if (_preLossDex != value)
                {
                    _preLossDex = value;
                }
            }
        }

        [DataMember]
        public int? PreLossStr
        {
            get => _preLossStr;
            set
            {
                if (_preLossStr != value)
                {
                    _preLossStr = value;
                }
            }
        }

        [XmlIgnore]
        protected DieRoll HdRoll
        {
            get => FindNextDieRoll(Hd, 0);
            set => Hd = ReplaceDieRoll(Hd, value, 0);
        }

        [XmlIgnore]
        public CreatureType CreatureType
        {
            get => Monster.ParseCreatureType(Type);
            set => Type = Monster.CreatureTypeText(value);
        }

        [XmlIgnore]
        public List<AttackSet> MeleeAttacks
        {
            get
            {
                List<AttackSet> sets = new List<AttackSet>();

                if (Melee != null)
                {
                    Regex regOr = new Regex("\\) or ");

                    Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);
                    int lastLoc = 0;

                    foreach (Match m in regOr.Matches(Melee))
                    {
                        AttackSet set = new AttackSet();
                        string text = Melee.Substring(lastLoc, m.Index - lastLoc + 1);

                        lastLoc = m.Index + m.Length;

                        foreach (Match a in regAttack.Matches(text))
                        {
                            ViewModels.Attack attack = ViewModels.Attack.ParseAttack(a);

                            if (attack.Weapon != null && attack.Weapon.Class != "Natural")
                            {
                                TestTwoHandedFromText(attack);
                                set.WeaponAttacks.Add(attack);
                            }
                            else
                            {
                                if (attack.Weapon == null)
                                {
                                    attack.Weapon = new Weapon(attack, false, SizeMods.GetSize(Size));

                                    if (attack.Weapon.Natural)
                                    {
                                        set.NaturalAttacks.Add(attack);
                                    }
                                    else
                                    {
                                        set.WeaponAttacks.Add(attack);
                                    }
                                }
                                else
                                {

                                    set.NaturalAttacks.Add(attack);
                                }
                            }

                        }

                        sets.Add(set);

                    }

                    string lastText = Melee.Substring(lastLoc);


                    AttackSet newSet = new AttackSet();

                    foreach (Match a in regAttack.Matches(lastText))
                    {
                        ViewModels.Attack attack = ViewModels.Attack.ParseAttack(a);

                        if (attack.Weapon != null && attack.Weapon.Class != "Natural")
                        {
                            TestTwoHandedFromText(attack);
                            newSet.WeaponAttacks.Add(attack);
                        }
                        else
                        {
                            if (attack.Weapon == null)
                            {
                                attack.Weapon = new Weapon(attack, false, SizeMods.GetSize(Size));
                            }

                            newSet.NaturalAttacks.Add(attack);
                        }
                    }

                    sets.Add(newSet);


                }

                return sets;
            }
        }

        [XmlIgnore]
        public List<ViewModels.Attack> RangedAttacks
        {
            get
            {
                List<ViewModels.Attack> attacks = new List<ViewModels.Attack>();

                Regex regAttack = new Regex(ViewModels.Attack.RegexString(null), RegexOptions.IgnoreCase);

                if (Ranged != null)
                {
                    foreach (Match m in regAttack.Matches(Ranged))
                    {
                        ViewModels.Attack attack = ViewModels.Attack.ParseAttack(m);

                        if (attack.Weapon == null)
                        {
                            attack.Weapon = new Weapon(attack, true, SizeMods.GetSize(Size));
                        }

                        attacks.Add(attack);

                    }
                }

                return attacks;
            }
        }



        [DataMember]
        public string Xp
        {
            get => _xp;
            set
            {
                _xp = value;
                Notify("XP");
            }
        }

        [DataMember]
        public string Race
        {
            get => _race;
            set
            {
                _race = value;
                Notify();
            }
        }

        [DataMember]
        public string Class
        {
            get => _className;
            set
            {
                _className = value;
                Notify();
            }
        }

        [DataMember]
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                Notify();
            }
        }

        [DataMember]
        public string SubType
        {
            get => _subType;
            set
            {
                bool oldMythic = IsMythic;
                _subType = value;

                Notify();
                if (oldMythic != IsMythic)
                {
                    Mythic = (!oldMythic)?"1":"0";
                    Notify("IsMythic");
                }

            }
        }


        [Reactive][DataMember]
        public int? DualInit { get; set; }

        [DataMember]
        public string Senses
        {
            get => _senses;
            set
            {
                _senses = value;
                Notify();
            }
        }

        [DataMember]
        public string Ac
        {
            get => _ac;
            set
            {
                _ac = value;
                Notify("AC");
            }
        }

        [DataMember]
        public string AcMods
        {
            get => _acMods;
            set
            {
                _acMods = value;
                Notify("AC_Mods");
            }
        }

        [DataMember]
        public string Hd
        {
            get => _hd;
            set
            {
                _hd = value;
                Notify("HD");
            }
        }

        [DataMember]
        public string Saves
        {
            get => _saves;
            set
            {
                _saves = value;
                Notify();
            }
        }



        [DataMember]
        public string SaveMods
        {
            get => _saveMods;
            set
            {
                _saveMods = value; 
                Notify("Save_Mods");
            }
        }

   

        [DataMember]
        public string Dr
        {
            get => _dr;
            set
            {
                _dr = value;
                Notify("DR");
            }
        }

        [DataMember]
        public string Sr
        {
            get => _sr;
            set
            {
                _sr = value;
                Notify("SR");
            }
        }



        [DataMember]
        public string Melee
        {
            get => _melee;
            set
            {
                _melee = value;
                Notify();
            }
        }

        [DataMember]
        public string Ranged
        {
            get => _ranged;
            set
            {
                _ranged = value;
                Notify();
            }
        }

        [DataMember]
        public string Space
        {
            get => _space;
            set
            {
                _space = value;
                Notify();
            }
        }

        [DataMember]
        public string Reach
        {
            get => _reach;
            set
            {
                _reach = value;
                Notify();
            }
        }

        [DataMember]
        public string SpecialAttacks
        {
            get
            {

                
                return _specialAttacks;
            }
            set
            {
                _specialAttacks = value;
                Notify();
            }
        }

        [DataMember]
        public string SpellLikeAbilities
        {
            get
            {

                
                return _spellLikeAbilities;
            }
            set
            {
                if (_spellLikeAbilities != value)
                {
                    _spellLikeAbilities = value;
                    _spellLikeAbilitiesBlock = null;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<SpellBlockInfo> SpellLikeAbilitiesBlock
        {
            get
            {
                ParseSpellLikeAbilities();
                return _spellLikeAbilitiesBlock;
            }
        }

        [DataMember]
        public string AbilitiyScores
        {
            get => _abilitiyScores;
            set
            {
                _abilitiyScores = value;
                Notify();
            }
        }

        [DataMember]
        public int BaseAtk
        {
            get => _baseAtk;
            set
            {
                _baseAtk = value;
                Notify();
            }
        }

        [DataMember]
        public string Cmb
        {
            get => _cmb;
            set
            {
                _cmb = value;
                Notify("CMB");
                Notify("CMB_Numeric");
                
            }
        }

        [DataMember]
        public string Cmd
        {
            get => _cmd;
            set
            {
                _cmd = value;
                Notify("CMD");
                Notify("CMD_Numeric");

            }
        }

        [DataMember]
        public string Feats
        {
            get
            {
                
                return _feats;
            }
            set
            {
                _feats = value;
                Notify();
            }
        }

        [DataMember]
        public string Skills
        {
            get
            {
                
                return _skills;
            }
            set
            {
                _skills = value;
                Notify();
            }
        }

        [DataMember]
        public string RacialMods
        {
            get => _racialMods;
            set
            {
                _racialMods = value;
                Notify();
            }
        }


        [DataMember]
        public string Sq
        {
            get => _sq;
            set
            {
                _sq = value;
                Notify("SQ");
            }
        }

        [DataMember]
        public string Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                Notify();
            }
        }

        [DataMember]
        public string Organization
        {
            get => _organization;
            set
            {
                _organization = value;
                Notify();
            }
        }

        [DataMember]
        public string Treasure
        {
            get
            {
     
                return _treasure;
            }
            set
            {
                _treasure = value;
                Notify();
            }
        }


        [DataMember]
        public string DescriptionVisual
        {
            get
            {

                return _descriptionVisual;
            }
            set
            {
                _descriptionVisual = value;
                Notify("Description_Visual");
            }
        }

        [DataMember]
        public string Group
        {
            get => _group;
            set
            {
                _group = value;
                Notify();
            }
        }



        [DataMember]
        public string IsTemplate
        {
            get => _isTemplate;
            set
            {
                _isTemplate = value;
                Notify();
            }
        }

        [DataMember]
        public string SpecialAbilities
        {
            get
            {
                
                return _specialAbilities;
            }
            set
            {
                _specialAbilities = value;
                Notify();
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                
                return _description;
            }
            set
            {
                _description = value;
                Notify();
            }
        }

        [DataMember]
        public string FullText
        {
            get => _fullText;
            set
            {
                _fullText = value;
                Notify();
            }
        }

        [DataMember]
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                Notify();
            }
        }

        [DataMember]
        public string Bloodline
        {
            get => _bloodline;
            set
            {
                _bloodline = value;
                Notify();
            }
        }

        [DataMember]
        public string ProhibitedSchools
        {
            get => _prohibitedSchools;
            set
            {
                _prohibitedSchools = value;
                Notify();
            }
        }

        [DataMember]
        public string BeforeCombat
        {
            get
            {

                
 
                return _beforeCombat;
            }
            set
            {
                _beforeCombat = value;
                Notify();
            }
        }

        [DataMember]
        public string DuringCombat
        {
            get
            {

 
                return _duringCombat;
            }
            set
            {
                _duringCombat = value;
                Notify();
            }
        }

        [DataMember]
        public string Morale
        {
            get
            {
                
                if (_morale == "NULL")
                {
                    _morale = null;
                }
                return _morale;
            }
            set
            {
                _morale = value;
                Notify();
            }
        }

        [DataMember]
        public string Gear
        {
            get
            {
                
                if (_gear == "NULL")
                {
                    _gear = null;
                }
                return _gear;
            }
            set
            {
                _gear = value;
                Notify();
            }
        }

        [DataMember]
        public string OtherGear
        {
            get
            {
                
                return _otherGear;
            }
            set
            {
                _otherGear = value;
                Notify();
            }
        }

        [DataMember]
        public string Vulnerability
        {
            get => _vulnerability;
            set
            {
                _vulnerability = value;
                if (_vulnerability == "NULL")
                {
                    _vulnerability = null;
                }
                Notify();
            }
        }

        [DataMember]
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                Notify();
            }
        }

        [DataMember]
        public string CharacterFlag
        {
            get => _characterFlag;
            set
            {
                _characterFlag = value;
                Notify();
            }
        }

        [DataMember]
        public string CompanionFlag
        {
            get => _companionFlag;
            set
            {
                _companionFlag = value;
                Notify();
            }
        }

        [DataMember]
        public string Fly
        {
            get => _fly;
            set
            {
                _fly = value;
                Notify();
            }
        }

        [DataMember]
        public string Climb
        {
            get => _climb;
            set
            {
                _climb = value;
                Notify();
            }
        }

        [DataMember]
        public string Burrow
        {
            get => _burrow;
            set
            {
                _burrow = value;
                Notify();
            }
        }

        [DataMember]
        public string Swim
        {
            get => _swim;
            set
            {
                _swim = value;
                Notify();
            }
        }

        [DataMember]
        public string Land
        {
            get => _land;
            set
            {
                _land = value;
                Notify();
            }
        }

        [DataMember]
        public string TemplatesApplied
        {
            get => _templatesApplied;
            set
            {
                _templatesApplied = value;
                Notify();
            }
        }

        [DataMember]
        public string OffenseNote
        {
            get => _offenseNote;
            set
            {
                _offenseNote = value;
                Notify();
            }
        }

        [DataMember]
        public string BaseStatistics
        {
            get => _baseStatistics;
            set
            {
                _baseStatistics = value;
                Notify();
            }
        }

        [DataMember]
        public string SpellsPrepared
        {
            get
            {
                
                return _spellsPrepared;
            }
            set
            {
                if (_spellsPrepared != value)
                {
                    _spellsPrepared = value;
                    _spellsPreparedBlock = null;
                    Notify();
                    
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<SpellBlockInfo> SpellsPreparedBlock
        {
            get
            {
                ParseSpellsPrepared();
                return _spellsPreparedBlock;
            }
        }

        [DataMember]
        public string SpellDomains
        {
            get => _spellDomains;
            set
            {
                _spellDomains = value;
                Notify();
            }
        }

        [DataMember]
        public string Aura
        {
            get => _aura;
            set
            {
                _aura = value;
                Notify();
            }
        }

        [DataMember]
        public string DefensiveAbilities
        {
            get => _defensiveAbilities;
            set
            {
                _defensiveAbilities = value;
                Notify();
            }
        }


        [DataMember]
        public string SpellsKnown
        {
            get
            {
                
                return _spellsKnown;
            }
            set
            {
                if (_spellsKnown != value)
                {
                    _spellsKnown = value;
                    _spellsKnownBlock = null;
                    Notify();
                    
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<SpellBlockInfo> SpellsKnownBlock
        {
            get
            {
                ParseSpellsKnown();
                return _spellsKnownBlock;
            }
        }



        [DataMember]
        public string SpeedMod
        {
            get => _speedMod;
            set
            {
                _speedMod = value;
                Notify("Speed_Mod");
            }
        }

        [DataMember]
        public string MonsterSource
        {
            get => _monsterSource;
            set
            {
                _monsterSource = value;
                Notify();
            }
        }

        [DataMember]
        public string ExtractsPrepared
        {
            get => _extractsPrepared;
            set
            {
                if (_extractsPrepared != value)
                {
                    _extractsPrepared = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public string AgeCategory
        {
            get => _ageCategory;
            set
            {
                if (_ageCategory != value)
                {
                    _ageCategory = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public bool DontUseRacialHd
        {
            get => _dontUseRacialHd;
            set
            {
                if (_dontUseRacialHd != value)
                {
                    _dontUseRacialHd = value;
                    Notify("DontUseRacialHD");
                }
            }
        }

        [DataMember]
        public string VariantParent
        {
            get => _variantParent;
            set
            {
                if (_variantParent != value)
                {
                    _variantParent = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public string DescHtml
        {
            get => _descHtml;
            set
            {
                if (_descHtml != value)
                {
                    _descHtml = value;
                    Notify("DescHTML");
                }
            }
        }

        [DataMember]
        public int? Mr
        {
            get => _mr;
            set
            {
                if (_mr != value)
                {
                    _mr = value;
                    Notify("MR");
                }
            }
        }

        [DataMember]
        public string Mythic
        {
            get => _mythic;
            set
            {
                if (_mythic != value)
                {
                    _mythic = value;
                    Notify();
                }
            }
        }

        [XmlIgnore]
        public bool IsMythic
        {
            get => HasSubtype("mythic");
            set
            {
                if (value)
                {
                    AddSubtype("mythic");
                }
                else
                {
                    RemoveSubtype("mythic");
                }
                string newStr = value ? "1" : "0";
                if (Mythic != newStr)
                {
                    Mythic = newStr;
                }
                Notify();
            }
        }

        [DataMember]
        public bool StatsParsed
        {
            get => _statsParsed;
            set => _statsParsed = value;
        }

        [DataMember]
        public override int? Strength
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _strength;
            }
            set
            {
                if (_strength != value)
                {

                    _strength = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public override int? Dexterity
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _dexterity;
            }
            set
            {
                if (_dexterity != value)
                {

                    _dexterity = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public override int? Constitution
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _constitution;
            }
            set
            {
                if (_constitution != value)
                {

                    _constitution = value;

                    Notify();
                }
            }
        }

        [DataMember]
        public override int? Intelligence
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _intelligence;
            }
            set
            {
                if (_intelligence != value)
                {
                    _intelligence = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public override int? Wisdom
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _wisdom;
            }
            set
            {
                if (_wisdom != value)
                {
                    _wisdom = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public override int? Charisma
        {
            get
            {
                if (!_statsParsed)
                {
                    ParseStats();
                }
                return _charisma;
            }
            set
            {
                if (_charisma != value)
                {
                    _charisma = value;
                    Notify();
                }

            }
        }

        [DataMember]
        public bool SpecialAblitiesParsed
        {
            get => _specialAblitiesParsed;
            set
            {
                _specialAblitiesParsed = value;
                Notify();
            }

        }

        [DataMember]
        public ObservableCollection<SpecialAbility> SpecialAbilitiesList
        {
            get
            {
                if (!_specialAblitiesParsed)
                {
                    ParseSpecialAbilities();
                }
                return _specialAbilitiesList;
            }
            set
            {
                _specialAbilitiesList = value;
                Notify();
            }
        }

        [DataMember]
        public bool SkillsParsed
        {
            get => _skillsParsed;
            set => _skillsParsed = value;
        }

        [XmlIgnore]
        public SortedDictionary<string, SkillValue> SkillValueDictionary
        {
            get
            {
                if (!_skillsParsed)
                {
                    ParseSkills();
                }
                else if (_skillValuesMayNeedUpdate)
                {
                    _skillValuesMayNeedUpdate = false;

                    foreach (SkillValue skillValue in _skillValueList)
                    {
                        _skillValueDictionary[skillValue.FullName] = skillValue;
                    }

                }
                return _skillValueDictionary;
            }
            set
            {
                _skillValueDictionary = value;
                Notify();
            }
        }

        [DataMember]
        public List<SkillValue> SkillValueList
        {
            get
            {
                if (!_skillValuesMayNeedUpdate)
                {
                    UpdateSkillValueList();
                }

                _skillValuesMayNeedUpdate = true;
                return _skillValueList;
            }
            set
            {
                _skillValueDictionary = new SortedDictionary<string, SkillValue>(StringComparer.OrdinalIgnoreCase);

                foreach (SkillValue val in value)
                {
                    _skillValueDictionary[val.FullName] = val;
                }

                Notify("SkillValueDictionary");
            }
        }

        [DataMember]
        public bool FeatsParsed
        {
            get => _featsParsed;
            set => _featsParsed = value;
        }

        [DataMember]
        public List<string> FeatsList
        {
            get
            {
                if (_featsList == null)
                {
                    _featsList = new List<string>();
                }
                if (!_featsParsed)
                {
                    ParseFeats();
                }
                return _featsList;
            }
            set
            {
                _featsList = value;
                Notify();
            }
        }

        [DataMember]
        public bool AcParsed
        {
            get => _acParsed;
            set
            {
                if (_acParsed != value)
                {
                    _acParsed = value;
                    Notify("acParsed");
                }
            }
        }

        [DataMember]
        public int FullAc
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _fullAc;
            }
            set
            {
                if (_fullAc != value)
                {
                    _fullAc = value;
                    Notify("FullAC");
                }
            }
        }

        [DataMember]
        public int TouchAc
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _touchAc;
            }
            set
            {
                if (_touchAc != value)
                {
                    _touchAc = value;
                    Notify("TouchAC");
                }
            }
        }

        [DataMember]
        public int FlatFootedAc
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _flatFootedAc;
            }
            set
            {
                if (_flatFootedAc != value)
                {
                    _flatFootedAc = value;
                    Notify("FlatFootedAC");
                }
            }
        }

        [DataMember]
        public int NaturalArmor
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _naturalArmor;
            }
            set
            {
                if (_naturalArmor != value)
                {
                    _naturalArmor = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int Deflection
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _deflection;
            }
            set
            {
                if (_deflection != value)
                {
                    _deflection = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int Shield
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _shield;
            }
            set
            {
                if (_shield != value)
                {
                    _shield = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int Armor
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _armor;
            }
            set
            {
                if (_armor != value)
                {
                    _armor = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int Dodge
        {
            get
            {
                if (!_acParsed)
                {
                    ParseAc();
                }

                return _dodge;
            }
            set
            {
                if (_dodge != value)
                {
                    _dodge = value;
                    Notify();
                }
            }
        }

        [DataMember]
        public int CmbNumeric
        {
            get => GetStartingModOrVal(Cmb);
            set
            {
                int num = CmbNumeric;
                if (num != value)
                {
                    Cmb = ChangeStartingModOrVal(Cmb, value - num);
                    Notify("CMB_Numeric");
                }
            }
        }

        [DataMember]
        public int CmdNumeric
        {
            get => GetStartingNumber(Cmd);
            set
            {
                int num = CmdNumeric;
                if (CmdNumeric != value)
                {
                    Cmd = ChangeCmd(Cmd, value - num);

                    Notify("CMD_Numeric");
                }
            }
        }

        [XmlIgnore]
        public long? XpValue
        {
            get
            {
                if (_xp != null)
                {

                    string xpstr = Regex.Replace(_xp, ",", "");
                    long val = 0;
                    if (long.TryParse(xpstr, out val))
                    {
                        return val;
                    }
                }
                return null;
            }
        }

        [XmlIgnore]
        public bool Npc
        {
            get => _npc;
            set
            {
                _npc = value;
                Notify("NPC");
            }
        }


            





        [XmlIgnore]
        public MonsterAdjuster Adjuster
        {
            get
            {
                if (_adjuster == null)
                {
                    _adjuster = new MonsterAdjuster(this);
                }

                return _adjuster;
            }
        }

        public int GetClassLvls(string mClass)
        {
            var regClasses = "(";
            int lvls = 0;

            foreach (CharacterClassEnum cclass in Enum.GetValues(typeof(CharacterClassEnum)))
            {
                if (cclass == CharacterClassEnum.None) continue;
                regClasses = regClasses+"|"+ CharacterClass.GetName(cclass);

            }

            regClasses = regClasses + ")";
            var regex = new Regex(@"." + regClasses + @"(?<lvl>\d+)");
            var x = regex.Matches(mClass);
            foreach (var match in x)
            {
                lvls += int.Parse(match.ToString());
            }
            return lvls;
        }
        public int? GetManoeuver(string maneuverType)
        {
            return ParseManoeuver(maneuverType);
        }

        private int? ParseManoeuver(string maneuverName)
        {

            var regex = new Regex(@"((?<bonus>(\+|-)\d+) |(?<=\()(?<bonus>(\+|-)\d+).+?)" + maneuverName.ToLower());
            var x = regex.Match(Cmb);

            return x.Success ? int.Parse(x.Groups["bonus"].Value) : CmbNumeric;
        }


        public static ManeuverType GetManeuverType(string maneuver)
        {
            return (ManeuverType)Enum.Parse(typeof(ManeuverType), maneuver);
        }


        public static string ManeuverName(ManeuverType maneuvreType)
        {
            switch (maneuvreType)
            {
                case ManeuverType.BullRush:
                    return "Bull Rush";
                case ManeuverType.DirtyTrick:
                    return "Dirty trick";
                case ManeuverType.Disarm:
                    return "Disarm";
                case ManeuverType.Drag:
                    return "Drag";
                case ManeuverType.Grapple:
                    return "Grapple";
                case ManeuverType.Overrun:
                    return "Overrun";
                case ManeuverType.Reposition:
                    return "Reposition";
                case ManeuverType.Steal:
                    return "Steal";
                case ManeuverType.Sunder:
                    return "Sunder";
                case ManeuverType.Trip:
                    return "Trip";
                default:
                    return "Generic";
            }
        }

        public int? GetSave(SaveType type)
        {
            if (type == SaveType.Fort)
            {
                return Fort;
            }
            else if (type == SaveType.Ref)
            {
                return Ref;
            }
            else if (type == SaveType.Will)
            {
                return Will;
            }

            return 0;
        }


        public static List<string> DragonColors => new(_dragonColorList.Keys);

        public static FlyQuality FlyQualityFromString(string strQuality)
        {
            FlyQuality qual = FlyQuality.Average;

            if (string.Compare(strQuality, "clumsy", true) == 0)
            {
                qual = FlyQuality.Clumsy;
            }
            if (string.Compare(strQuality, "poor", true) == 0)
            {
                qual = FlyQuality.Poor;
            }

            if (string.Compare(strQuality, "average", true) == 0)
            {
                qual = FlyQuality.Average;
            }

            if (string.Compare(strQuality, "good", true) == 0)
            {
                qual = FlyQuality.Good;
            }

            if (string.Compare(strQuality, "perfect", true) == 0)
            {
                qual = FlyQuality.Perfect;
            }


            return qual;
        }

        public static string StringFromFlyQuality(FlyQuality qual)
        {
            string text = "Average";

            switch (qual)
            {
                case FlyQuality.Clumsy:
                    text = "clumsy";
                    break;
                case FlyQuality.Poor:
                    text = "poor";
                    break;
                case FlyQuality.Average:
                    text = "average";
                    break;
                case FlyQuality.Good:
                    text = "good";
                    break;
                case FlyQuality.Perfect:
                    text = "perfect";
                    break;
            }

            return text;
        }

        public static string SaveName(Monster.SaveType type)
        {
            switch (type)
            {
                case Monster.SaveType.Fort:
                    return "Fort";
                case Monster.SaveType.Ref:
                    return "Ref";
                default:
                    return "Will";
            }
        }

        public class MonsterAdjuster : SimpleNotifyClass
        {

            private Monster _monster;

            private string _flyQuality;

            public void NotifyPropertyChanged(string property)
            {
                Notify(property); 
            }


            public MonsterAdjuster(Monster m)
            {
                _monster = m;

                _monster.PropertyChanged += Monster_PropertyChanged;
            }

            void Monster_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);

                if (e.PropertyName == "Size")
                {
                    NotifyPropertyChanged("MonsterSize");
                }
                if (e.PropertyName == "SubType")
                {
                    NotifyPropertyChanged("Subtype");
                }

            }

            public int? Strength
            {
                get => _monster._strength;
                set
                {
                    if (_monster._strength != value)
                    {
                        SetStat(Stat.Strength, value);
                        NotifyPropertyChanged("Strength");
                    }
                }
            }

            public int? Dexterity
            {
                get => _monster._dexterity;
                set
                {
                    if (_monster._dexterity != value)
                    {
                        SetStat(Stat.Dexterity, value);
                        NotifyPropertyChanged("Dexterity");
                    }
                }
            }

            public int? Constitution
            {
                get => _monster._constitution;
                set
                {
                    if (_monster._constitution != value)
                    {
                        SetStat(Stat.Constitution, value);
                        NotifyPropertyChanged("Constitution");
                    }
                }
            }

            public int? Intelligence
            {
                get => _monster._intelligence;
                set
                {
                    if (_monster._intelligence != value)
                    {
                        SetStat(Stat.Intelligence, value);
                        NotifyPropertyChanged("Intelligence");
                    }
                }
            }

            public int? Wisdom
            {
                get => _monster._wisdom;
                set
                {
                    if (_monster._wisdom != value)
                    {
                        SetStat(Stat.Wisdom, value);
                        NotifyPropertyChanged("Wisdom");
                    }
                }
            }

            public int? Charisma
            {
                get => _monster._charisma;
                set
                {
                    if (_monster._charisma != value)
                    {
                        SetStat(Stat.Charisma, value);
                        NotifyPropertyChanged("Charisma");
                    }
                }
            }

            private void SetStat(Stat stat, int? newValue)
            {
                int? statVal = _monster.GetStat(stat);

                if (statVal == null)
                {
                    //adjust from null strength
                    _monster.SetStatDirect(stat, 10);
                    _monster.AdjustStat(stat, newValue.Value - 10);
                }
                else if (newValue == null)
                {
                    //adjust to null strength
                    _monster.AdjustStat(stat, 10 - statVal.Value);
                    _monster.SetStatDirect(stat, null);
                }
                else
                {
                    _monster.AdjustStat(stat, newValue.Value - statVal.Value);
                }
            }

            private int? GetSpeed(string speedType)
            {
                Regex speedReg = new Regex(speedType + " +(?<speed>[0-9]+) +ft\\.", RegexOptions.IgnoreCase);

                int? foundSpeed = null;

                if (_monster.Speed != null)
                {
                    Match m = speedReg.Match(_monster.Speed);

                    if (m.Success)
                    {
                        foundSpeed = int.Parse(m.Groups["speed"].Value);
                    }
                }

                return foundSpeed;
            }

            private void SetSpeed(string speedType, int? value)
            {
                if (value == null)
                {
                    Regex speedReg = new Regex(speedType + " +(?<speed>[0-9]+) +ft\\.(, *)?", RegexOptions.IgnoreCase);
                    _monster.Speed = speedReg.Replace(_monster.Speed, "");

                }
                else
                {

                    bool bFound = false;
                    Regex speedReg = new Regex(speedType + " +(?<speed>[0-9]+) +ft\\.", RegexOptions.IgnoreCase);
                    _monster.Speed = speedReg.Replace(_monster.Speed,
                               delegate (Match m)
                               {
                                   bFound = true;
                                   return value + " ft.";
                               }
                               );

                    if (!bFound)
                    {
                        _monster.Speed += ", " + speedType + " " + value + " ft.";
                    }

                }
            }


            public int LandSpeed
            {
                get
                {
                    int speed = 0;

                    if (_monster.Speed != null)
                    {
                        Regex speedReg = new Regex("^ *(?<speed>[0-9]+) +ft\\.", RegexOptions.IgnoreCase);

                        Match m = speedReg.Match(_monster.Speed);

                        if (m.Success)
                        {
                            speed = int.Parse(m.Groups["speed"].Value);
                        }
                    }

                    return speed;
                }
                set
                {

                    Regex speedReg = new Regex("^ *(?<speed>[0-9]+) +ft\\.", RegexOptions.IgnoreCase);
                    _monster.Speed = speedReg.Replace(_monster.Speed,
                                    delegate (Match m)
                                    {
                                        return value + " ft.";
                                    }
                                    );

                    NotifyPropertyChanged("LandSpeed");
                }
            }

            private bool ParseFly(out int speed, out string quality)
            {
                speed = 0;
                quality = null;

                Regex speedReg = new Regex("fly +(?<speed>[0-9]+) +ft\\. +\\((?<quality>[\\p{L}]+)\\)", RegexOptions.IgnoreCase);
                if (_monster.Speed != null)
                {
                    Match m = speedReg.Match(_monster.Speed);

                    if (m.Success)
                    {
                        speed = int.Parse(m.Groups["speed"].Value);
                        quality = StringCapitalizer.Capitalize(m.Groups["quality"].Value);
                    }
                    return m.Success;
                }
                return false;

            }

            private void SetFly(int speed, string quality)
            {
                Regex speedReg = new Regex("fly +(?<speed>[0-9]+) +ft\\. +\\((?<quality>[\\p{L}]+)\\)", RegexOptions.IgnoreCase);

                string flyString = "fly " + speed + " ft. (" + quality.ToLower() + ")";

                bool found = false;
                _monster.Speed = speedReg.Replace(_monster.Speed, delegate (Match m)
                    {
                        found = true;
                        return flyString;
                    });


                if (!found)
                {
                    _monster.Speed += ", " + flyString;
                }

            }

            private void RemoveFly()
            {
                Regex speedReg = new Regex("(, +)?fly +(?<speed>[0-9]+) +ft\\. +\\((?<quality>[\\p{L}]+)\\)", RegexOptions.IgnoreCase);

                speedReg.Replace(_monster.Speed, "");

            }

            public int? FlySpeed
            {
                get => GetSpeed("fly");
                set
                {
                    if (value == null)
                    {
                        RemoveFly();
                    }
                    else
                    {
                        SetFly(value.Value, StringFromFlyQuality((FlyQuality)FlyQuality));
                    }
                }
            }


            public int FlyQuality
            {
                get
                {
                    int speed;
                    string quality;
                    if (ParseFly(out speed, out quality))
                    {
                        _flyQuality = quality;
                    }
                    else if (_flyQuality == null)
                    {
                        _flyQuality = "Average";
                    }

                    return (int)FlyQualityFromString(_flyQuality);
                }
                set
                {
                    _flyQuality = StringFromFlyQuality((FlyQuality)value);

                    int speed;
                    string quality;
                    if (ParseFly(out speed, out quality))
                    {
                        SetFly(speed, _flyQuality);
                    }

                }
            }


            public int? ClimbSpeed
            {
                get => GetSpeed("climb");
                set
                {
                    SetSpeed("climb", value);
                    NotifyPropertyChanged("ClimbSpeed");
                }
            }
            public int? BurrowSpeed
            {
                get => GetSpeed("burrow");
                set
                {
                    SetSpeed("burrow", value);
                    NotifyPropertyChanged("BurrowSpeed");
                }
            }
            public int? SwimSpeed
            {
                get => GetSpeed("swim");
                set
                {
                    SetSpeed("swim", value);
                    NotifyPropertyChanged("SwimSpeed");
                }
            }

            public void AdjustAllSpeed(int speed)
            {
                LandSpeed = Math.Max(LandSpeed + speed, 0);
                if (FlySpeed != null)
                {
                    FlySpeed = Math.Max(FlySpeed.Value + speed, 0);
                }
                if (SwimSpeed != null)
                {
                    SwimSpeed = Math.Max(SwimSpeed.Value + speed, 0);
                }
                if (ClimbSpeed != null)
                {
                    ClimbSpeed = Math.Max(ClimbSpeed.Value + speed, 0);
                }
                if (BurrowSpeed != null)
                {
                    BurrowSpeed = Math.Max(BurrowSpeed.Value + speed, 0);
                }
            } 

            public int MonsterSize
            {
                get => (int)SizeMods.GetSize(_monster.Size);
                set
                {
                    MonsterSize old = SizeMods.GetSize(_monster.Size);

                    int diff = ((int)value) - (int)old;

                    _monster.AdjustSize(diff);
                    NotifyPropertyChanged("MonsterSize");
                }
            }

            public int Armor
            {
                get => _monster.Armor;
                set
                {
                    if (value != _monster.Armor)
                    {
                        _monster.AdjustArmor(value - _monster.Armor);
                        NotifyPropertyChanged("Armor");
                    }
                }
            }

            public int Deflection
            {
                get => _monster.Deflection;
                set
                {
                    if (value != _monster.Deflection)
                    {
                        _monster.AdjustDeflection(value - _monster.Deflection);
                        NotifyPropertyChanged("Deflection");
                    }
                }
            }

            public int Dodge
            {
                get => _monster.Dodge;
                set
                {
                    if (value != _monster.Dodge)
                    {
                        _monster.AdjustDodge(value - _monster.Dodge);
                        NotifyPropertyChanged("Dodge");
                    }
                }
            }

            public int NaturalArmor
            {
                get => _monster.NaturalArmor;
                set
                {
                    if (value != _monster.NaturalArmor)
                    {
                        _monster.AdjustNaturalArmor(value - _monster.NaturalArmor);
                        NotifyPropertyChanged("NaturalArmor");
                    }
                }
            }

            public int Shield
            {
                get => _monster.Shield;
                set
                {
                    if (value != _monster.Shield)
                    {
                        _monster.AdjustShield(value - _monster.Shield);
                        NotifyPropertyChanged("Shield");
                    }
                }
            }

            public int BaseAtk
            {
                get => _monster.BaseAtk;
                set
                {
                    if (value != _monster.BaseAtk)
                    {
                        _monster.AdjustBaseAttack(value - _monster.BaseAtk, true);
                        NotifyPropertyChanged("BaseAtk");
                    }
                }
            }

            public string SpellLikeAbilities
            {
                get
                {
                    string sla = _monster._spellLikeAbilities;

                    if (sla != null)
                    {
                        Regex regSla = new Regex(@"(?<SLA>([\p{L}( )]+)?\s?Spell-Like Abilities .*)", RegexOptions.IgnoreCase);
                        
                        Match m = regSla.Match(sla);

                        if (m.Success)
                        {
                            sla = m.Groups["SLA"].Value;
                        }
                    }

                    return sla;
                }
                set
                {
                    if (value == null || value.Trim().Length == 0)
                    {
                        _monster.SpellLikeAbilities = null;
                    }

                    _monster.SpellLikeAbilities = "Spell-Like Abilities " + value.Trim();
                    NotifyPropertyChanged("SpellLikeAbilities");
                }

            }

            public string SpellsPrepared
            {
                get
                {
                    string spells = _monster.SpellsPrepared;

                    if (spells != null)
                    {
                        Regex regSpells = new Regex("^ *Spells Prepared +(?<spells>.*)", RegexOptions.IgnoreCase);
                        Match m = regSpells.Match(spells);

                        if (m.Success)
                        {
                            spells = m.Groups["spells"].Value;
                        }
                    }

                    return spells;
                }
                set
                {
                    if (value == null || value.Trim().Length == 0)
                    {
                        _monster.SpellsPrepared = null;
                    }
                    _monster.SpellsPrepared = "Spells Prepared " + value;
                    NotifyPropertyChanged("SpellsPrepared");
                }

            }

            public string SpellsKnown
            {
                get
                {
                    string spells = _monster.SpellsKnown;

                    if (spells != null)
                    {

                        Regex regSpells = new Regex("^ *Spells Known +(?<spells>.*)", RegexOptions.IgnoreCase);
                        Match m = regSpells.Match(spells);

                        if (m.Success)
                        {
                            spells = m.Groups["spells"].Value;
                        }
                    }

                    return spells;
                }
                set
                {
                    if (value == null || value.Trim().Length == 0)
                    {
                        _monster.SpellsKnown = null;
                    }
                    _monster.SpellsKnown = "Spells Known " + value;
                    NotifyPropertyChanged("SpellsKnown");
                }

            }


            public string Cr
            {
                get => _monster.Cr;
                set
                {

                    _monster.Cr = value;
                    _monster.Xp = GetXpString(value);

                    Notify("CR");

                }
            }

            public int? Mr
            {
                get => _monster.Mr;
                set
                {
                    _monster.Mr = value;

                    if (value == null)
                    {
                        //_Monster.
                        //remove mythic
                    }
                    else
                    {
                        //add mythic
                    }

                    Notify("MR");
                }
            }

            public string Subtype
            {
                get
                {
                    if (_monster.SubType == null)
                    {
                        return null;
                    }
                    return _monster.SubType.Trim(new char[] { '(', ')' });
                }
                set
                {
                    if (value == null)
                    {
                        _monster.SubType = null;
                    }
                    else
                    {
                        string val = value.Trim();
                        if (val.Length == 0)
                        {
                            _monster.SubType = null;
                        }
                        else
                        {
                            if (!Regex.Match(val, "\\(.+\\)").Success)
                            {
                                val = "(" + val + ")";
                            }
                            _monster.SubType = val;
                        }
                    }

                    Notify();

                }
            }
            public DieRoll Hd
            {
                get => DieRoll.FromString(_monster.Hd);
                set
                {
                    _monster.Hd = "(" + value.ToString() + ")";
                    _monster.Hp = value.AverageRoll();
                    Notify("HD");
                }
            }

            public int Space
            {
                get
                {
                    int? space = FootConverter.Convert(_monster.Space);
                    return (space == null) ? 0 : (space.Value);
                }
                set => _monster.Space = FootConverter.ConvertBack(value);
            }

            public int Reach
            {
                get
                {
                    int? reach = FootConverter.Convert(_monster.Reach);
                    return (reach == null) ? 0 : (reach.Value);
                }
                set => _monster.Reach = FootConverter.ConvertBack(value);
            }
        }


    }

}
