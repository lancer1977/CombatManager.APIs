/*
 *  Spell.cs
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


using CombatManager.Database.Models;
using CombatManager.Interfaces;
using CombatManager.Utilities;

namespace CombatManager;


public class Spell : BaseDbClass
{
    private string _name;
    private string _school;
    private string _subschool;
    private string _descriptor;
    private string _spellLevel;
    private string _castingTime;
    private string _components;
    private string _costlyComponents;
    private string _range;
    private string _targets;
    private string _effect;
    private string _dismissible;
    private string _area;
    private string _duration;
    private string _shapeable;
    private string _savingThrow;
    private string _spellResistence;
    private string _description;
    private string _descriptionFormated;
    private string _source;
    private string _fullText;
    private string _verbal;
    private string _somatic;
    private string _material;
    private string _focus;
    private string _divineFocus;
    private string _sor;
    private string _wiz;
    private string _cleric;
    private string _druid;
    private string _ranger;
    private string _bard;
    private string _paladin;
    private string _alchemist;
    private string _summoner;
    private string _witch;
    private string _inquisitor;
    private string _oracle;
    private string _antipaladin;
    private string _assassin;
    private string _adept;
    private string _redMantisAssassin;
    private string _magus;
    private string _url;
    private string _slaLevel;
    private string _preparationTime;
    private bool _duplicated;
    private string _acid;
    private string _air;
    private string _chaotic;
    private string _cold;
    private string _curse;
    private string _darkness;
    private string _death;
    private string _disease;
    private string _earth;
    private string _electricity;
    private string _emotion;
    private string _evil;
    private string _fear;
    private string _fire;
    private string _force;
    private string _good;
    private string _language;
    private string _lawful;
    private string _light;
    private string _mindAffecting;
    private string _pain;
    private string _poison;
    private string _shadow;
    private string _sonic;
    private string _water;

    //annotation fields

    //bonus annotation
    private ConditionBonus _bonus;
    //treasure table annotations
    private string _potionWeight;
    private string _divineScrollWeight;
    private string _arcaneScrollWeight;
    private string _wandWeight;
    private string _potionLevel;
    private string _potionCost;
    private string _arcaneScrollLevel;
    private string _arcaneScrollCost;
    private string _divineScrollLevel;
    private string _divineScrollCost;
    private string _wandLevel;
    private string _wandCost;




    private SpellAdjuster _adjuster;

    private static ObservableCollection<Spell> _spells;

    private static Dictionary<int, Spell> _spellsByDetailsId;

    private static SortedDictionary<string, string> _schools;
    private static SortedSet<string> _subschools;
    private static SortedSet<string> _descriptors;
    private static Dictionary<string, ObservableCollection<Spell>> _spellDictionary;

    private static SortedSet<string> _castingTimeOptions;
    private static SortedSet<string> _rangeOptions;
    private static SortedSet<string> _areaOptions;
    private static SortedSet<string> _targetsOptions;
    private static SortedSet<string> _durationOptions;
    private static SortedSet<string> _savingThrowOptions;
    private static SortedSet<string> _spellResistanceOptions;

    private static ISQLService<Spell> _spellsDb;

    public delegate void SpellsDbUpdatedDelegate(ICollection<Spell> added, ICollection<Spell> updated, ICollection<Spell> removed);

    public Spell()
    {

    }

    public Spell(Spell s)
    {
        CopyFrom(s);
    }

    public void CopyFrom(Spell s)
    {
        _name = s._name;
        _school = s._school;
        _subschool = s._subschool;
        _descriptor = s._descriptor;
        _spellLevel = s._spellLevel;
        _castingTime = s._castingTime;
        _components = s._components;
        _costlyComponents = s._costlyComponents;
        _range = s._range;
        _targets = s._targets;
        _effect = s._effect;
        _dismissible = s._dismissible;
        _area = s._area;
        _duration = s._duration;
        _shapeable = s._shapeable;
        _savingThrow = s._savingThrow;
        _spellResistence = s._spellResistence;
        _description = s._description;
        _descriptionFormated = s._descriptionFormated;
        _source = s._source;
        _fullText = s._fullText;
        _verbal = s._verbal;
        _somatic = s._somatic;
        _material = s._material;
        _focus = s._focus;
        _divineFocus = s._divineFocus;
        _sor = s._sor;
        _wiz = s._wiz;
        _cleric = s._cleric;
        _druid = s._druid;
        _ranger = s._ranger;
        _bard = s._bard;
        _paladin = s._paladin;
        _alchemist = s._alchemist;
        _summoner = s._summoner;
        _witch = s._witch;
        _inquisitor = s._inquisitor;
        _oracle = s._oracle;
        _antipaladin = s._antipaladin;
        _assassin = s._assassin;
        _adept = s._adept;
        _redMantisAssassin = s._redMantisAssassin;
        _magus = s._magus;
        _url = s._url;
        _slaLevel = s._slaLevel;
        _preparationTime = s._preparationTime;
        _duplicated = s._duplicated;
        _acid = s._acid;
        _air = s._air;
        _chaotic = s._chaotic;
        _cold = s._cold;
        _curse = s._curse;
        _darkness = s._darkness;
        _death = s._death;
        _disease = s._disease;
        _earth = s._earth;
        _electricity = s._electricity;
        _emotion = s._emotion;
        _evil = s._evil;
        _fear = s._fear;
        _fire = s._fire;
        _force = s._force;
        _good = s._good;
        _language = s._language;
        _lawful = s._lawful;
        _light = s._light;
        _mindAffecting = s._mindAffecting;
        _pain = s._pain;
        _poison = s._poison;
        _shadow = s._shadow;
        _sonic = s._sonic;
        _water = s._water;
        _potionWeight = s._potionWeight;
        _divineScrollWeight = s.DivineScrollWeight;
        _arcaneScrollWeight = s.ArcaneScrollWeight;
        _wandWeight = s._wandWeight;
        _potionLevel = s._potionLevel;
        _potionCost = s._potionCost;
        _arcaneScrollLevel = s._arcaneScrollLevel;
        _arcaneScrollCost = s._arcaneScrollCost;
        _divineScrollLevel = s._divineScrollLevel;
        _divineScrollCost = s._divineScrollCost;
        _wandLevel = s._wandLevel;
        _wandCost = s._wandCost;

        if (s._bonus != null)
        {
            _bonus = (ConditionBonus)s._bonus.Clone();
        }

        DbLoaderId = s.DbLoaderId;

        _adjuster = null;
    }

    public object Clone()
    {
        return new Spell(this);
    }

    protected override void SelfPropertyChanged(string name)
    {
        if (name == "DetailsID")
        {
            Notify("detailsid");
        }
    }

    public int? LevelForClass(CharacterClassEnum cl)
    {
        string levelStr = null;
        switch (cl)
        {
            case CharacterClassEnum.Alchemist:
                levelStr = Alchemist;
                break;

            case CharacterClassEnum.Antipaladin:
                if (Antipaladin != null && Antipaladin.Trim().Length > 0)
                {
                    levelStr = Antipaladin;
                }
                else
                {
                    levelStr = Paladin;
                }
                break;
            case CharacterClassEnum.Bard:
                levelStr = Bard; ;
                break;
            case CharacterClassEnum.Cleric:
                {

                    levelStr = Cleric;
                    break;
                }
            case CharacterClassEnum.Druid:
                {
                    levelStr = Druid;
                    break;
                }
            case CharacterClassEnum.Inquisitor:
                {
                    levelStr = Inquisitor;
                    break;
                }
            case CharacterClassEnum.Magus:
                {
                    levelStr = Magus;
                    break;
                }
            case CharacterClassEnum.Oracle:
                {
                    if (Oracle != null && Oracle.Trim().Length > 0)
                    {
                        levelStr = Oracle;
                    }
                    else
                    {
                        levelStr = Cleric;
                    };
                    break;
                }
            case CharacterClassEnum.Paladin:
                {
                    levelStr = Paladin;
                    break;
                }
            case CharacterClassEnum.Ranger:
                {
                    levelStr = Ranger;
                    break;
                }
            case CharacterClassEnum.Sorcerer:
                {

                    levelStr = Sor;
                    break;
                }
            case CharacterClassEnum.Summoner:
                {
                    levelStr = Summoner;
                    break;
                }
            case CharacterClassEnum.Witch:
                {
                    levelStr = Witch;
                    break;
                }
            case CharacterClassEnum.Wizard:
                {
                    levelStr = Wiz;
                    break;
                }
        }

        if (levelStr != null)
        {
            int val;
            if (int.TryParse(levelStr, out val))
            {
                return val;
            }

        }

        return null;
    }

    public IEnumerable<int> AllLevels
    {
        get
        {
            SortedDictionary<int, int> levList = new SortedDictionary<int, int>();

            foreach (CharacterClassEnum cl in Enum.GetValues(typeof(CharacterClassEnum)))
            {
                int? val = LevelForClass(cl);

                if (val != null)
                {
                    levList[val.Value] = val.Value;
                }
            }

            return levList.Values;

        }
    }

    public bool IsLevel(int lev)
    {
        foreach (CharacterClassEnum cl in Enum.GetValues(typeof(CharacterClassEnum)))
        {
            int? val = LevelForClass(cl);

            if (val == lev)
            {
                return true;
            }
        }
        return false;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                Oldname = _name;
                _name = value;
                Notify("name");
                Notify();
            }
        }
    }

    [XmlIgnore]
    public string Oldname { get; set; }



    public string School
    {
        get => _school;
        set
        {
            if (_school != value)
            {
                _school = value;
                Notify("school");
            }
        }
    }
    public string Subschool
    {
        get => _subschool;
        set
        {
            if (_subschool != value)
            {
                _subschool = value;
                Notify("subschool");
            }
        }
    }
    public string Descriptor
    {
        get => _descriptor;
        set
        {
            if (_descriptor != value)
            {
                _descriptor = value;
                Notify("descriptor");
            }
        }
    }
    public string SpellLevel
    {
        get => _spellLevel;
        set
        {
            if (_spellLevel != value)
            {
                _spellLevel = value;
                Notify("spell_level");
            }
        }
    }
    public string CastingTime
    {
        get
        {
            UpdateFromDetailsDb();
            return _castingTime;
        }
        set
        {
            if (_castingTime != value)
            {
                _castingTime = value;
                Notify("casting_time");
            }
        }
    }
    public string Components
    {
        get
        {
            UpdateFromDetailsDb();
            return _components;
        }
        set
        {
            if (_components != value)
            {
                _components = value;
                if (_adjuster != null)
                {
                    _adjuster.UpdateComponents();
                }
                Notify("components");
            }
        }
    }
    public string CostlyComponents
    {
        get
        {
            UpdateFromDetailsDb();
            return _costlyComponents;
        }
        set
        {
            if (_costlyComponents != value)
            {
                _costlyComponents = value;
                Notify("costly_components");
            }
        }
    }
    public string Range
    {
        get
        {
            UpdateFromDetailsDb();
            return _range;
        }
        set
        {
            if (_range != value)
            {
                _range = value;
                Notify("range");
            }
        }
    }
    public string Targets
    {
        get
        {
            UpdateFromDetailsDb();
            return _targets;
        }
        set
        {
            if (_targets != value)
            {
                _targets = value;
                Notify("targets");
            }
        }
    }
    public string Duration
    {
        get =>
            //UpdateFromDetailsDB();
            _duration;
        set
        {
            if (_duration != value)
            {
                _duration = value;
                Notify("duration");
            }
        }
    }
    public string Dismissible
    {
        get =>
            //UpdateFromDetailsDB();
            _dismissible;
        set
        {
            if (_dismissible != value)
            {
                _dismissible = value;
                Notify("dismissible");
            }
        }
    }
    public string Shapeable
    {
        get =>
            //UpdateFromDetailsDB();
            _shapeable;
        set
        {
            if (_shapeable != value)
            {
                _shapeable = value;
                Notify("shapeable");
            }
        }
    }
    public string SavingThrow
    {
        get
        {
            UpdateFromDetailsDb();
            return _savingThrow;
        }
        set
        {
            if (_savingThrow != value)
            {
                _savingThrow = value;
                Notify("saving_throw");
            }
        }
    }
    public string SpellResistence
    {
        get
        {
            UpdateFromDetailsDb();
            return _spellResistence;
        }
        set
        {
            if (_spellResistence != value)
            {
                _spellResistence = value;
                Notify("spell_resistence");
            }
        }
    }
    public string Description
    {
        get
        {
            UpdateFromDetailsDb();
            return _description;
        }
        set
        {
            if (_description != value)
            {
                _description = value;
                Notify("description");
            }
        }
    }
    public string DescriptionFormated
    {
        get
        {
            UpdateFromDetailsDb();
            return _descriptionFormated;
        }
        set
        {
            if (_descriptionFormated != value)
            {
                _descriptionFormated = value;
                Notify("description_formated");
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
                Notify("source");
            }
        }
    }
    public string FullText
    {
        get => _fullText;
        set
        {
            if (_fullText != value)
            {
                _fullText = value;
                Notify("full_text");
            }
        }
    }
    public string Verbal
    {
        get => _verbal;
        set
        {
            if (_verbal != value)
            {
                _verbal = value;
                Notify("verbal");
            }
        }
    }
    public string Somatic
    {
        get => _somatic;
        set
        {
            if (_somatic != value)
            {
                _somatic = value;
                Notify("somatic");
            }
        }
    }
    public string Material
    {
        get => _material;
        set
        {
            if (_material != value)
            {
                _material = value;
                Notify("material");
            }
        }
    }
    public string Focus
    {
        get => _focus;
        set
        {
            if (_focus != value)
            {
                _focus = value;
                Notify("focus");
            }
        }
    }
    public string DivineFocus
    {
        get => _divineFocus;
        set
        {
            if (_divineFocus != value)
            {
                _divineFocus = value;
                Notify("divine_focus");
            }
        }
    }
    public string Sor
    {
        get => _sor;
        set
        {
            if (_sor != value)
            {
                _sor = value;
                if (_sor == "NULL")
                {
                    _sor = null;
                }
                Notify("sor");
            }
        }
    }
    public string Wiz
    {
        get => _wiz;
        set
        {
            if (_wiz != value)
            {
                _wiz = value;
                if (_wiz == "NULL")
                {
                    _wiz = null;
                }
                Notify("wiz");
            }
        }
    }
    public string Cleric
    {
        get => _cleric;
        set
        {
            if (_cleric != value)
            {
                _cleric = value;
                if (_cleric == "NULL")
                {
                    _cleric = null;
                }
                Notify("cleric");
            }
        }
    }
    public string Druid
    {
        get => _druid;
        set
        {
            if (_druid != value)
            {
                _druid = value;
                if (_druid == "NULL")
                {
                    _druid = null;
                }
                Notify("druid");
            }
        }
    }
    public string Ranger
    {
        get => _ranger;
        set
        {
            if (_ranger != value)
            {
                _ranger = value;
                if (_ranger == "NULL")
                {
                    _ranger = null;
                }
                Notify("ranger");
            }
        }
    }
    public string Bard
    {
        get => _bard;
        set
        {
            if (_bard != value)
            {
                _bard = value;
                if (_bard == "NULL")
                {
                    _bard = null;
                }
                Notify("bard");
            }
        }
    }
    public string Paladin
    {
        get => _paladin;
        set
        {
            if (_paladin != value)
            {
                _paladin = value;
                if (_paladin == "NULL")
                {
                    _paladin = null;
                }
                Notify("paladin");
            }
        }
    }
    public string Effect
    {
        get => _effect;
        set
        {
            if (_effect != value)
            {
                _effect = value;
                Notify("effect");
            }
        }
    }

    public string Area
    {
        get =>
            //UpdateFromDetailsDB();
            _area;
        set
        {
            if (_area != value)
            {
                _area = value;
                Notify("area");
            }
        }
    }


    public string Alchemist
    {
        get => _alchemist;
        set
        {
            if (_alchemist != value)
            {
                _alchemist = value;
                if (_alchemist == "NULL")
                {
                    _alchemist = null;
                }
                Notify("alchemist");
            }
        }
    }
    public string Summoner
    {
        get => _summoner;
        set
        {
            if (_summoner != value)
            {
                _summoner = value;
                if (_summoner == "NULL")
                {
                    _summoner = null;
                }
                Notify("summoner");
            }
        }
    }
    public string Witch
    {
        get => _witch;
        set
        {
            if (_witch != value)
            {
                _witch = value;
                if (_witch == "NULL")
                {
                    _witch = null;
                }
                Notify("witch");
            }
        }
    }
    public string Inquisitor
    {
        get => _inquisitor;
        set
        {
            if (_inquisitor != value)
            {
                _inquisitor = value;
                if (_inquisitor == "NULL")
                {
                    _inquisitor = null;
                }
                Notify("inquisitor");
            }
        }
    }
    public string Oracle
    {
        get => _oracle;
        set
        {
            if (_oracle != value)
            {
                _oracle = value;
                if (_oracle == "NULL")
                {
                    _oracle = null;
                }
                Notify("oracle");
            }
        }
    }
    public string Antipaladin
    {
        get => _antipaladin;
        set
        {
            if (_antipaladin != value)
            {
                _antipaladin = value;
                if (_antipaladin == "NULL")
                {
                    _antipaladin = null;
                }
                Notify("antipaladin");
            }
        }
    }

    public string Assassin
    {
        get => _assassin;
        set
        {
            if (_assassin != value)
            {
                _assassin = value;
                Notify("assassin");
            }
        }
    }

    public string RedMantisAssassin
    {
        get => _redMantisAssassin;
        set
        {
            if (_redMantisAssassin != value)
            {
                _redMantisAssassin = value;
                Notify("red_mantis_assassin");
            }
        }
    }

    public string Adept
    {
        get => _adept;
        set
        {
            if (_adept != value)
            {
                _adept = value;
                Notify("adept");
            }
        }
    }
    public string Url
    {
        get => _url;
        set
        {
            if (_url != value)
            {
                _url = value;
                Notify("URL");
            }
        }
    }

    public string Magus
    {
        get => _magus;
        set
        {
            if (_magus != value)
            {
                _magus = value;
                Notify("magus");
            }
        }
    }
    public string SlaLevel
    {
        get => _slaLevel;
        set
        {
            if (_slaLevel != value)
            {
                _slaLevel = value;
                Notify("SLA_Level");
            }
        }
    }
    public string PreparationTime
    {
        get => _preparationTime;
        set
        {
            if (_preparationTime != value)
            {
                _preparationTime = value;
                Notify("preparation_time");
            }
        }
    }
    public bool Duplicated
    {
        get => _duplicated;
        set
        {
            if (_duplicated != value)
            {
                _duplicated = value;
                Notify("duplicated");
            }
        }
    }
    public string Acid
    {
        get => _acid;
        set
        {
            if (_acid != value)
            {
                _acid = value;
                Notify("acid");
            }
        }
    }
    public string Air
    {
        get
        {
            UpdateFromDetailsDb();
            return _air;
        }
        set
        {
            if (_air != value)
            {
                _air = value;
                Notify("air");
            }
        }
    }
    public string Chaotic
    {
        get
        {
            UpdateFromDetailsDb();
            return _chaotic;
        }
        set
        {
            if (_chaotic != value)
            {
                _chaotic = value;
                Notify("chaotic");
            }
        }
    }
    public string Cold
    {
        get
        {
            UpdateFromDetailsDb();
            return _cold;
        }
        set
        {
            if (_cold != value)
            {
                _cold = value;
                Notify("cold");
            }
        }
    }
    public string Curse
    {
        get
        {
            UpdateFromDetailsDb();
            return _curse;
        }
        set
        {
            if (_curse != value)
            {
                _curse = value;
                Notify("curse");
            }
        }
    }
    public string Darkness
    {
        get
        {
            UpdateFromDetailsDb();
            return _darkness;
        }
        set
        {
            if (_darkness != value)
            {
                _darkness = value;
                Notify("darkness");
            }
        }
    }
    public string Death
    {
        get
        {
            UpdateFromDetailsDb();
            return _death;
        }
        set
        {
            if (_death != value)
            {
                _death = value;
                Notify("death");
            }
        }
    }
    public string Disease
    {
        get
        {
            UpdateFromDetailsDb();
            return _disease;
        }
        set
        {
            if (_disease != value)
            {
                _disease = value;
                Notify("disease");
            }
        }
    }
    public string Earth
    {
        get
        {
            UpdateFromDetailsDb();
            return _earth;
        }
        set
        {
            if (_earth != value)
            {
                _earth = value;
                Notify("earth");
            }
        }
    }
    public string Electricity
    {
        get
        {
            UpdateFromDetailsDb();
            return _electricity;
        }
        set
        {
            if (_electricity != value)
            {
                _electricity = value;
                Notify("electricity");
            }
        }
    }
    public string Emotion
    {
        get
        {
            UpdateFromDetailsDb();
            return _emotion;
        }
        set
        {
            if (_emotion != value)
            {
                _emotion = value;
                Notify("emotion");
            }
        }
    }
    public string Evil
    {
        get
        {
            UpdateFromDetailsDb();
            return _evil;
        }
        set
        {
            if (_evil != value)
            {
                _evil = value;
                Notify("evil");
            }
        }
    }
    public string Fear
    {
        get
        {
            UpdateFromDetailsDb();
            return _fear;
        }
        set
        {
            if (_fear != value)
            {
                _fear = value;
                Notify("fear");
            }
        }
    }
    public string Fire
    {
        get
        {
            UpdateFromDetailsDb();
            return _fire;
        }
        set
        {
            if (_fire != value)
            {
                _fire = value;
                Notify("fire");
            }
        }
    }
    public string Force
    {
        get
        {
            UpdateFromDetailsDb();
            return _force;
        }
        set
        {
            if (_force != value)
            {
                _force = value;
                Notify("force");
            }
        }
    }
    public string Good
    {
        get
        {
            UpdateFromDetailsDb();
            return _good;
        }
        set
        {
            if (_good != value)
            {
                _good = value;
                Notify("good");
            }
        }
    }
    public string Language
    {
        get
        {
            UpdateFromDetailsDb();
            return _language;
        }
        set
        {
            if (_language != value)
            {
                _language = value;
                Notify("language");
            }
        }
    }
    public string Lawful
    {
        get
        {
            UpdateFromDetailsDb();
            return _lawful;
        }
        set
        {
            if (_lawful != value)
            {
                _lawful = value;
                Notify("lawful");
            }
        }
    }
    public string Light
    {
        get
        {
            UpdateFromDetailsDb();
            return _light;
        }
        set
        {
            if (_light != value)
            {
                _light = value;
                Notify("light");
            }
        }
    }
    public string MindAffecting
    {
        get
        {
            UpdateFromDetailsDb();
            return _mindAffecting;
        }
        set
        {
            if (_mindAffecting != value)
            {
                _mindAffecting = value;
                Notify("mind_affecting");
            }
        }
    }
    public string Pain
    {
        get
        {
            UpdateFromDetailsDb();
            return _pain;
        }
        set
        {
            if (_pain != value)
            {
                _pain = value;
                Notify("pain");
            }
        }
    }
    public string Poison
    {
        get
        {
            UpdateFromDetailsDb();
            return _poison;
        }
        set
        {
            if (_poison != value)
            {
                _poison = value;
                Notify("poison");
            }
        }
    }
    public string Shadow
    {
        get
        {
            UpdateFromDetailsDb();
            return _shadow;
        }
        set
        {
            if (_shadow != value)
            {
                _shadow = value;
                Notify("shadow");
            }
        }
    }
    public string Sonic
    {
        get
        {
            UpdateFromDetailsDb();
            return _sonic;
        }
        set
        {
            if (_sonic != value)
            {
                _sonic = value;
                Notify("sonic");
            }
        }
    }
    public string Water
    {
        get
        {
            UpdateFromDetailsDb();
            return _water;
        }
        set
        {
            if (_water != value)
            {
                _water = value;
                Notify("water");
            }
        }
    }

    [XmlElement("id")]
    public int Detailsid
    {
        get => DetailsId;
        set
        {
            if (DetailsId != value)
            {
                _detailsId = value;
            }
        }
    }



    public string PotionWeight
    {
        get => _potionWeight;
        set
        {
            if (_potionWeight != value)
            {
                _potionWeight = value;
                Notify();
            }
        }
    }
    public string DivineScrollWeight
    {
        get => _divineScrollWeight;
        set
        {
            if (_divineScrollWeight != value)
            {
                _divineScrollWeight = value;
                Notify();
            }
        }
    }
    public string ArcaneScrollWeight
    {
        get => _arcaneScrollWeight;
        set
        {
            if (_arcaneScrollWeight != value)
            {
                _arcaneScrollWeight = value;
                Notify();
            }
        }
    }
    public string WandWeight
    {
        get => _wandWeight;
        set
        {
            if (_wandWeight != value)
            {
                _wandWeight = value;
                Notify();
            }
        }
    }


    public string PotionLevel
    {
        get => _potionLevel;
        set
        {
            if (_potionLevel != value)
            {
                _potionLevel = value;
                Notify();
            }
        }
    }
    public string PotionCost
    {
        get => _potionCost;
        set
        {
            if (_potionCost != value)
            {
                _potionCost = value;
                Notify();
            }
        }
    }
    public string ArcaneScrollLevel
    {
        get => _arcaneScrollLevel;
        set
        {
            if (_arcaneScrollLevel != value)
            {
                _arcaneScrollLevel = value;
                Notify();
            }
        }
    }
    public string ArcaneScrollCost
    {
        get => _arcaneScrollCost;
        set
        {
            if (_arcaneScrollCost != value)
            {
                _arcaneScrollCost = value;
                Notify();
            }
        }
    }
    public string DivineScrollLevel
    {
        get => _divineScrollLevel;
        set
        {
            if (_divineScrollLevel != value)
            {
                _divineScrollLevel = value;
                Notify();
            }
        }
    }
    public string DivineScrollCost
    {
        get => _divineScrollCost;
        set
        {
            if (_divineScrollCost != value)
            {
                _divineScrollCost = value;
                Notify();
            }
        }
    }
    public string WandLevel
    {
        get => _wandLevel;
        set
        {
            if (_wandLevel != value)
            {
                _wandLevel = value;
                Notify();
            }
        }
    }
    public string WandCost
    {
        get => _wandCost;
        set
        {
            if (_wandCost != value)
            {
                _wandCost = value;
                Notify();
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
                Notify();
            }
        }
    }


    void UpdateFromDetailsDb()
    {
        if (DbLoaderId != 0)
        {
            //perform updating from DB
            var dto = _spellsDb.GetById(DbLoaderId);


            _castingTime = dto.CastingTime;
            _components = dto.Components;
            _costlyComponents = dto.CostlyComponents;
            _range = dto.Range;
            _targets = dto.Targets;
            _effect = dto.Effect;
            //_dismissible = dto.Dismissible;
            //_area = dto.Area;
            //_duration = dto.Duration;
            //_shapeable = dto.shapeable;
            _savingThrow = dto.SavingThrow;
            _spellResistence = dto.SpellResistence;
            _description = dto.Description;
            _descriptionFormated = dto.DescriptionFormated;
            _acid = dto.Acid;
            _air = dto.Air;
            _chaotic = dto.Chaotic;
            _cold = dto.Cold;
            _curse = dto.Curse;
            _darkness = dto.Darkness;
            _death = dto.Death;
            _disease = dto.Disease;
            _earth = dto.Earth;
            _electricity = dto.Electricity;
            _emotion = dto.Emotion;
            _evil = dto.Evil;
            _fear = dto.Fear;
            _fire = dto.Fire;
            _force = dto.Force;
            _good = dto.Good;
            _language = dto.Language;
            _lawful = dto.Lawful;
            _light = dto.Light;
            _mindAffecting = dto.MindAffecting;
            _pain = dto.Pain;
            _poison = dto.Poison;
            _shadow = dto.Shadow;
            _sonic = dto.Sonic;
            _water = dto.Water;

            DbLoaderId = 0;
        }
    }

    [XmlIgnore]
    public string Nameforsort => RomanNumbers.FindAndReplace(Name);

    public override string ToString()
    {
        return Name;
    }

    [XmlIgnore]
    public SpellAdjuster Adjuster
    {
        get
        {
            if (_adjuster == null)
            {
                _adjuster = new SpellAdjuster(this);
            }

            return _adjuster;
        }
    }


    public class SpellAdjuster : SimpleNotifyClass
    {

        private static Dictionary<string, string> _classes = new Dictionary<string, string>();

        private Spell _spell;
        private ObservableCollection<LevelAdjusterInfo> _levels;
        private ObservableCollection<LevelAdjusterInfo> _unusedLevels;

        private bool _verbal;
        private bool _somatic;
        private bool _material;
        private bool _focus;
        private bool _divineFocus;
        private string _materialText;
        private string _focusText;
        private string _duration;
        private bool _dismissible;


        private bool _parsing;
        private bool _updatingText;
        private bool _updatingLevels;
        private bool _loadingLevels;


        static SpellAdjuster()
        {
            Classes["sor"] = "Sorcerer";
            Classes["wiz"] = "Wizard";
            Classes["cleric"] = "Cleric";
            Classes["druid"] = "Druid";
            Classes["ranger"] = "Ranger";
            Classes["bard"] = "Bard";
            Classes["paladin"] = "Paladin";
            Classes["alchemist"] = "Alchemist";
            Classes["summoner"] = "Summoner";
            Classes["witch"] = "Witch";
            Classes["inquisitor"] = "Inquisitor";
            Classes["oracle"] = "Oracle";
            Classes["antipaladin"] = "Antipaladin";
            Classes["assassin"] = "Assassin";
            Classes["adept"] = "Adept";
            Classes["red_mantis_assassin"] = "Red Mantis Assassin";
            Classes["magus"] = "Magus";
        }

        public static Dictionary<string, string> Classes => _classes;

        public SpellAdjuster(Spell spell)
        {
            _spell = spell;
            _spell.PropertyChanged += _spell_PropertyChanged;
            _levels = new ObservableCollection<LevelAdjusterInfo>();
            _unusedLevels = new ObservableCollection<LevelAdjusterInfo>();

            _levels.CollectionChanged += _Levels_CollectionChanged;

            LoadInfo();

            ParseComponents();

            ParseDuration();
        }



        public void UpdateComponents()
        {
            ParseComponents();
        }

        private void ParseComponents()
        {
            if (!_updatingText)
            {
                _parsing = true;
                try
                {
                    if (_spell.Components != null)
                    {
                        Match m = Regex.Match(_spell.Components,
                            "^(?<v>V)?(, )?(?<s>S)?(, )?" +
                            "((?<m>M(?<mdf>///DF)?( \\((?<mtext>[^)]+)\\))?))?" +
                            "(,? ?" + "((?<f>F)|(?<df>DF)|(?<fdf>///FDF))" +
                            "( \\((?<ftext>[^)]+)\\))?" + ")?"
                            );

                        if (m.Success)
                        {
                            Verbal = m.GroupSuccess("v");
                            Somatic = m.GroupSuccess("s");
                            Material = m.GroupSuccess("m");
                            Focus = m.AnyGroupSuccess(new string[] { "f", "fdf" });
                            DivineFocus = m.AnyGroupSuccess(new string[] { "df", "mdf", "fdf" });


                            MaterialText = m.Value("mtext");
                            FocusText = m.Value("ftext");
                        }

                    }
                }
                finally
                {
                    _parsing = false;
                }
            }

        }

        private void UpdateText()
        {
            if (!_parsing)
            {
                _updatingText = true;
                _spell.Components = ToString();
                _updatingText = false;
            }
        }

        public override string ToString()
        {
            string text = "";

            if (Verbal)
            {
                text = text.AppendListItem("V");
            }

            if (Somatic)
            {
                text = text.AppendListItem("S");
            }

            if (Material)
            {
                if (DivineFocus && !Focus)
                {
                    text = text.AppendListItem("M/DF");
                }
                else
                {
                    text = text.AppendListItem("M");
                }

                if (MaterialText != null && MaterialText.Trim().Length > 0)
                {
                    text += " (" + MaterialText.Trim() + ")";
                }
            }

            if (Focus || DivineFocus)
            {
                if (!Material && !Focus)
                {
                    text = text.AppendListItem("DF");
                }
                else if (Focus && DivineFocus)
                {
                    text = text.AppendListItem("F/DF");
                }
                else if (Focus && !DivineFocus)
                {
                    text = text.AppendListItem("F");
                }

                if (!(DivineFocus && Material && !Focus) && FocusText != null && FocusText.Length > 0)
                {
                    text += " (" + FocusText + ")";
                }
            }

            return text;

        }

        public bool Verbal
        {
            get => _verbal;
            set
            {
                if (_verbal != value)
                {
                    _verbal = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public bool Somatic
        {
            get => _somatic;
            set
            {
                if (_somatic != value)
                {
                    _somatic = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public bool Material
        {
            get => _material;
            set
            {
                if (_material != value)
                {
                    _material = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public bool Focus
        {
            get => _focus;
            set
            {
                if (_focus != value)
                {
                    _focus = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public bool DivineFocus
        {
            get => _divineFocus;
            set
            {
                if (_divineFocus != value)
                {
                    _divineFocus = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public string MaterialText
        {
            get => _materialText;
            set
            {
                if (_materialText != value)
                {
                    _materialText = value;
                    UpdateText();
                    Notify();
                }
            }
        }
        public string FocusText
        {
            get => _focusText;
            set
            {
                if (_focusText != value)
                {
                    _focusText = value;
                    UpdateText();
                    Notify();
                }
            }
        }


        public string Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    SetDuration();
                    Notify();
                }
            }
        }
        public bool Dismissible
        {
            get => _dismissible;
            set
            {
                if (_dismissible != value)
                {
                    _dismissible = value;
                    SetDuration();
                    Notify();
                }
            }
        }

        private void ParseDuration()
        {
            if (_spell.Duration != null)
            {
                Match m = Regex.Match(_spell.Duration, "(?<text>[^(]+) \\(D\\)");

                if (m.Success)
                {
                    _dismissible = true;
                    _duration = m.Value("text");
                }
                else
                {
                    _dismissible = false;
                    _duration = _spell.Duration;
                }

            }
        }

        private void SetDuration()
        {
            if (_duration != null)
            {
                _spell.Duration = _duration;
                if (_dismissible)
                {
                    _spell.Duration += " (D)";
                }
            }
        }



        void _Levels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (LevelAdjusterInfo info in e.NewItems)
                {
                    if (info != null)
                    {
                        info.PropertyChanged += info_PropertyChanged;
                    }
                }
            }

            if (!_loadingLevels)
            {

                List<LevelAdjusterInfo> ul = new List<LevelAdjusterInfo>(_unusedLevels);

                if (e.OldItems != null)
                {
                    foreach (LevelAdjusterInfo info in e.OldItems)
                    {
                        if (info != null)
                        {
                            if (ul.FirstOrDefault(a => a != null && a.Property == info.Property) == null)
                            {
                                ul.Add(info);
                            }
                        }

                    }
                }


                if (e.NewItems != null)
                {
                    foreach (LevelAdjusterInfo info in e.NewItems)
                    {
                        if (info != null)
                        {
                            info.PropertyChanged += info_PropertyChanged;

                            LevelAdjusterInfo oldInfo = ul.FirstOrDefault(a => a.Property == info.Property);
                            if (oldInfo != null)
                            {
                                ul.Remove(oldInfo);
                            }
                        }
                    }
                }
                ul.Sort((a, b) => a.Class.CompareTo(b.Class));

                System.Diagnostics.Debug.Assert(_unusedLevels != null);
                if (_unusedLevels != null)
                {
                    _unusedLevels.Clear();
                    foreach (LevelAdjusterInfo info in ul)
                    {
                        _unusedLevels.Add(info);
                    }
                }
            }


            SaveInfo();
        }

        void _spell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Classes.ContainsKey(e.PropertyName))
            {
                LoadInfo();
            }
        }

        public void LoadInfo()
        {

            if (!_updatingLevels)
            {
                _loadingLevels = true;
                _levels.Clear();
                _unusedLevels.Clear();
                List<LevelAdjusterInfo> ul = new List<LevelAdjusterInfo>();

                System.Type t = typeof(Spell);

                foreach (KeyValuePair<string, string> pair in _classes)
                {
                    PropertyInfo p = t.GetProperty(pair.Key);

                    string val = (string)p.GetGetMethod().Invoke(_spell, new object[] { });
                    bool added = false;

                    LevelAdjusterInfo info = new LevelAdjusterInfo();
                    info.Class = pair.Value;
                    info.Property = pair.Key;


                    if (val != null && val.Length > 0 && val != "NULL")
                    {
                        int level;
                        if (int.TryParse(val, out level))
                        {
                            info.Level = level;
                            _levels.Add(info);
                            added = true;

                        }
                    }

                    if (!added)
                    {
                        ul.Add(info);

                    }


                }

                ul.Sort((a, b) => a.Class.CompareTo(b.Class));

                foreach (var lev in ul)
                {
                    _unusedLevels.Add(lev);
                }

                _loadingLevels = false;
            }
        }

        public void SaveInfo()
        {
            if (!_loadingLevels)
            {
                _updatingLevels = true;

                System.Type t = typeof(Spell);
                foreach (KeyValuePair<string, string> pair in Classes)
                {

                    PropertyInfo p = t.GetProperty(pair.Key);

                    LevelAdjusterInfo info = LevelValue(_levels, pair.Key);

                    if (info == null)
                    {
                        p.GetSetMethod().Invoke(_spell, new object[] { null });
                    }
                    else
                    {
                        p.GetSetMethod().Invoke(_spell, new object[] { info.Level.ToString() });
                    }
                }

                List<LevelAdjusterInfo> list = new List<LevelAdjusterInfo>(_levels);


                string levelText = "";

                while (list.Count > 0)
                {
                    LevelAdjusterInfo info = list[0];

                    if (info == null)
                    {
                        System.Console.WriteLine("Info null");
                        list.RemoveAt(0);
                    }
                    else
                    {

                        string text = null;

                        if (info.Property == "sor" || info.Property == "wiz")
                        {
                            text = GetPairText(list, "sor", "wiz");
                        }
                        else if (info.Property == "cleric" || info.Property == "oracle")
                        {
                            text = GetPairText(list, "cleric", "oracle");
                        }

                        if (text == null)
                        {
                            text = info.Class + " " + info.Level;
                            list.RemoveAt(0);
                        }


                        if (levelText.Length > 0)
                        {
                            levelText += ", ";
                        }
                        levelText += text;
                    }
                }

                _spell.SpellLevel = levelText;

                _updatingLevels = false;
            }
        }

        private LevelAdjusterInfo LevelValue(IEnumerable<LevelAdjusterInfo> list, string property)
        {
            return list.FirstOrDefault(a => (a != null && a.Property == property));
        }

        private string GetPairText(List<LevelAdjusterInfo> list, string class1, string class2)
        {
            LevelAdjusterInfo info1 = LevelValue(list, class1);
            LevelAdjusterInfo info2 = LevelValue(list, class2);

            if (info1 != null && info2 != null && info1.Level == info2.Level)
            {
                list.RemoveAll(a => a != null && (a.Property == class1) || (a.Property == class2));

                string text = info1.Class + "/" + info2.Class + " " + info1.Level;

                return text;
            }
            else
            {
                return null;
            }
        }



        void info_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveInfo();
        }

        public ObservableCollection<LevelAdjusterInfo> Levels => _levels;

        public bool ContainsClass(string className)
        {
            foreach (LevelAdjusterInfo li in _levels)
            {
                if (li.Class == className)
                {
                    return true;
                }
            }
            return false;
        }

        public ObservableCollection<LevelAdjusterInfo> UnusedLevels => _unusedLevels;



    }

}

public class LevelAdjusterInfo : SimpleNotifyClass
{

    private string _class;
    private int _level;
    private string _property;


    public string Class
    {
        get => _class;
        set
        {
            if (_class != value)
            {
                _class = value;
                Notify();
            }
        }
    }
    public int Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
                Notify();
            }
        }
    }
    public string Property
    {
        get => _property;
        set
        {
            if (_property != value)
            {
                _property = value;
                Notify();
            }
        }
    }

}