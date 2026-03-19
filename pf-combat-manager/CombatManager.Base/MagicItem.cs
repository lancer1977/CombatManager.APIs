/*
 *  MagicItem.cs
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

using System.Diagnostics;
 using System.IO;
using System.Xml.Linq;
using CombatManager.Database;
using PolyhydraGames.Extensions;

namespace CombatManager
{


    public enum ItemLevel
    {
        Minor,
        Medium,
        Major
    }

    public class MagicItem : BaseDbClass
    {

        private string _name;
        private string _aura;
        private int _cl;
        private string _slot;
        private string _price;
        private string _weight;
        private string _description;
        private string _requirements;
        private string _cost;
        private string _group;
        private string _source;
        private string _fullText;
        private string _destruction;
        private string _minorArtifactFlag;
        private string _majorArtifactFlag;
        private string _abjuration;
        private string _conjuration;
        private string _divination;
        private string _enchantment;
        private string _evocation;
        private string _necromancy;
        private string _transmutation;
        private string _auraStrength;
        private string _weightValue;
        private string _priceValue;
        private string _costValue; 
        private string _al;
        private string _int;
        private string _wis;
        private string _cha;
        private string _ego;
        private string _communication;
        private string _senses;
        private string _powers;
        private string _magicItems;
        private string _descHtml;
        private string _mythic;
        private string _legendaryWeapon;




        private static Dictionary<string, MagicItem> _itemMap;
        private static SortedDictionary<string, string> _groups;
        private static SortedDictionary<int, int> _cls;

        private static Dictionary<int, MagicItem> _magicItemsByDetailsId;

        private static bool _magicItemsLoaded;

        public static void LoadMagicItems()
        {

            List<MagicItem> set = LoadMagicItemsFromXml("MagicItemsShort.xml");
            _magicItemsByDetailsId = new Dictionary<int, MagicItem>();


            _groups = new SortedDictionary<string, string>();
            _cls = new SortedDictionary<int, int>();
            _itemMap = new Dictionary<string, MagicItem>(new InsensitiveEqualityCompararer());

            foreach (MagicItem item in set)
            {
                _itemMap[item.Name] = item;

                _groups[item.Group] = item.Group;
                _cls[item.Cl] = item.Cl;

                _magicItemsByDetailsId[item.DetailsId] = item;
            }

            _magicItemsLoaded = true;
        }

        public static bool MagicItemsLoaded => _magicItemsLoaded;

        public static List<MagicItem> LoadMagicItemsFromXml(string filename)
        {
            XElement last = null;  
            try
            {

                List<MagicItem> magicItems = new List<MagicItem>();
    #if ANDROID
                XDocument doc = XDocument.Load(new StreamReader(CoreContext.Context.Assets.Open(filename)));
    #elif MONO

                XDocument doc = XDocument.Load(Path.Combine(XmlLoader<MagicItem>.AssemblyDir, filename));
                         

    #else
                XDocument doc = XDocument.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename));
    #endif
               
                foreach (var v in doc.Descendants("MagicItem"))
                {
                    last = v;
                    MagicItem m = new MagicItem();

                    m._detailsId = v.ElementIntValue("id");

                    Debug.Assert(m.DetailsId != 0);

                    m._name = v.ElementValue("Name");
                    if (v.ElementValue("CL") != "-")
                    {
                        m._cl = v.ElementIntValue("CL");
                    }
                    else
                    {
                        m._cl = 0;
                    }
                    m._group = v.ElementValue("Group");
                    m._source = v.ElementValue("Source");
                    m._magicItems = v.ElementValue("MagicItems");

                    magicItems.Add(m);
                }
                return magicItems;
            }
            catch (Exception)
            {
                throw;
            }

        
        }

        public static List<string> DetailsFields =>
            new() {
                "Aura",
                "Slot",
                "Price",
                "Weight",
                "Description",
                "Requirements",
                "Cost",
                "FullText",
                "Destruction",
                "MinorArtifactFlag",
                "MajorArtifactFlag",
                "Abjuration",
                "Conjuration",
                "Divination",
                "Enchantment",
                "Evocation",
                "Necromancy",
                "Transmutation",
                "AuraStrength",
                "WeightValue",
                "PriceValue",
                "CostValue", 
                "AL",
                "Int",
                "Wis",
                "Cha",
                "Ego",
                "Communication",
                "Senses",
                "Powers",
                "MagicItems",
                "DescHTML",
                "Mythic",
                "LegendaryWeapon",};

        public MagicItem()
        {
        }

        public MagicItem(MagicItem m)
        {
            CopyFrom(m);
        }

        public object Clone()
        {
            return new MagicItem(this);
        }

        public void CopyFrom(MagicItem magicItem)
        {
            if (magicItem == null)
            {
                return;
            }
            _detailsId = magicItem.DetailsId;
            _name = magicItem._name;
            _aura = magicItem._aura;
            _cl = magicItem._cl;
            _slot = magicItem._slot;
            _price = magicItem._price;
            _weight = magicItem._weight;
            _description = magicItem._description;
            _requirements = magicItem._requirements;
            _cost = magicItem._cost;
            _group = magicItem._group;
            _source = magicItem._source;
            _fullText = magicItem._fullText;
            _destruction = magicItem._destruction;
            _minorArtifactFlag = magicItem._minorArtifactFlag;
            _majorArtifactFlag = magicItem._majorArtifactFlag;
            _abjuration = magicItem._abjuration;
            _conjuration = magicItem._conjuration;
            _divination = magicItem._divination;
            _enchantment = magicItem._enchantment;
            _evocation = magicItem._evocation;
            _necromancy = magicItem._necromancy;
            _transmutation = magicItem._transmutation;
            _auraStrength = magicItem._auraStrength;
            _weightValue = magicItem._weightValue;
            _priceValue = magicItem._priceValue;
            _costValue = magicItem._costValue;
            _al = magicItem._al;
            _int = magicItem._int;
            _wis = magicItem._wis;
            _cha = magicItem._cha;
            _ego = magicItem._ego;
            _communication = magicItem._communication;
            _senses = magicItem._senses;
            _powers = magicItem._powers;
            _magicItems = magicItem._magicItems;
            _descHtml = magicItem._descHtml;
            _mythic = magicItem._mythic;
            _legendaryWeapon = magicItem._legendaryWeapon;
            DbLoaderId = magicItem.DbLoaderId;
        }

        void UpdateFromDetailsDb()
        {
            if (DetailsId != 0)
            {
                //perform updating from DB
                var list = DetailsDb.MagicItems.GetById(DetailsId);
                this.CopySharedProperties(list); 
                DbLoaderId = 0;

                _detailsId = 0;
            }
        }

        public static Dictionary<string, MagicItem> Items
        {
            get
            {
                if (_itemMap == null)
                {
                    LoadMagicItems();
                }
                return _itemMap;
            }
        }


        public static MagicItem ByDetailsId(int id)
        {
            
            if (_magicItemsByDetailsId == null)
            {
                LoadMagicItems();
            }
            MagicItem m;
            _magicItemsByDetailsId.TryGetValue(id, out m);
            return m;
        }

        public static MagicItem ByDbLoaderId(int id)
        {
            return null;
        }

        public static MagicItem ById(bool custom, int id)
        {
            if (custom)
            {
                return ByDbLoaderId(id); ;
            }
            else
            {
                return ByDetailsId(id);
            }
        }

        public static bool TryById(bool custom, int id, out MagicItem s)
        {
            s = ById(custom, id);
            return s != null;
        }

        public static ICollection<string> Groups
        {
            get
            {
                if (_itemMap == null)
                {
                    LoadMagicItems();
                }
                return _groups.Values;
            }
        }
        public static ICollection<int> CLs
        {
            get
            {
                if (_itemMap == null)
                {
                    LoadMagicItems();
                }
                return _cls.Values;
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
                    Notify();                }
            }
        }
        public string Aura
        {
            get { UpdateFromDetailsDb(); 
                return _aura; }
            set
            {
                if (_aura != value)
                {
                    _aura = value;
                    Notify();                }
            }
        }
        public int Cl
        {
            get => _cl;
            set
            {
                if (_cl != value)
                {
                    _cl = value;
                    Notify("CL");                }
            }
        }
        public string Slot
        {
            get
            {
                UpdateFromDetailsDb();
                return _slot;
            }
            set
            {
                if (_slot != value)
                {
                    _slot = value;
                    Notify();                }
            }
        }
        public string Price
        {
            get
            {
                UpdateFromDetailsDb();
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    Notify();                }
            }
        }
        public string Weight
        {
            get
            {
                UpdateFromDetailsDb();
                return _weight;
            }
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    Notify();                }
            }
        }
        public string Description
        {
            get {
                UpdateFromDetailsDb();

                return _description; 
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    Notify();                }
            }
        }
        public string Requirements
        {
            get
            {
                UpdateFromDetailsDb();

                return _requirements;
            }
            set
            {
                if (_requirements != value)
                {
                    _requirements = value;
                    Notify();                }
            }
        }
        public string Cost
        {
            get
            {
                UpdateFromDetailsDb();
                return _cost;
            }
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    Notify();                }
            }
        }
        public string Group
        {
            get => _group;
            set
            {
                if (_group != value)
                {
                    _group = value;
                    Notify();                }
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
                    Notify();                }
            }
        }
        public string FullText
        {
            get
            {
                UpdateFromDetailsDb();

                return _fullText;
            }
            set
            {
                if (_fullText != value)
                {
                    _fullText = value;
                    Notify();                }
            }
        }
        public string Destruction
        {
            get
            {
                UpdateFromDetailsDb();

                return _destruction;
            }
            set
            {
                if (_destruction != value)
                {
                    _destruction = value;
                    Notify();                }
            }
        }
        public string MinorArtifactFlag
        {
            get
            {
                UpdateFromDetailsDb();
                return _minorArtifactFlag;
            }
            set
            {
                if (_minorArtifactFlag != value)
                {
                    _minorArtifactFlag = value;
                    Notify();                }
            }
        }
        public string MajorArtifactFlag
        {
            get
            {
                UpdateFromDetailsDb();
                return _majorArtifactFlag;
            }
            set
            {
                if (_majorArtifactFlag != value)
                {
                    _majorArtifactFlag = value;
                    Notify();                }
            }
        }
        public string Abjuration
        {
            get
            {
                UpdateFromDetailsDb();
                return _abjuration;
            }
            set
            {
                if (_abjuration != value)
                {
                    _abjuration = value;
                    Notify();                }
            }
        }
        public string Conjuration
        {
            get
            {
                UpdateFromDetailsDb();
                return _conjuration;
            }
            set
            {
                if (_conjuration != value)
                {
                    _conjuration = value;
                    Notify();                }
            }
        }
        public string Divination
        {
            get
            {
                UpdateFromDetailsDb();
                return _divination;
            }
            set
            {
                if (_divination != value)
                {
                    _divination = value;
                    Notify();                }
            }
        }
        public string Enchantment
        {
            get
            {
                UpdateFromDetailsDb();
                return _enchantment;
            }
            set
            {
                if (_enchantment != value)
                {
                    _enchantment = value;
                    Notify();                }
            }
        }
        public string Evocation
        {
            get
            {
                UpdateFromDetailsDb();
                return _evocation;
            }
            set
            {
                if (_evocation != value)
                {
                    _evocation = value;
                    Notify();                }
            }
        }
        public string Necromancy
        {
            get
            {
                UpdateFromDetailsDb();
                return _necromancy;
            }
            set
            {
                if (_necromancy != value)
                {
                    _necromancy = value;
                    Notify();                }
            }
        }
        public string Transmutation
        {
            get
            {
                UpdateFromDetailsDb();
                return _transmutation;
            }
            set
            {
                if (_transmutation != value)
                {
                    _transmutation = value;
                    Notify();                }
            }
        }
        public string AuraStrength
        {
            get
            {
                UpdateFromDetailsDb();
                return _auraStrength;
            }
            set
            {
                if (_auraStrength != value)
                {
                    _auraStrength = value;
                    Notify();                }
            }
        }
        public string WeightValue
        {
            get
            {
                UpdateFromDetailsDb();
                return _weightValue;
            }
            set
            {
                if (_weightValue != value)
                {
                    _weightValue = value;
                    Notify();                }
            }
        }
        public string PriceValue
        {
            get
            {
                UpdateFromDetailsDb();
                return _priceValue;
            }
            set
            {
                if (_priceValue != value)
                {
                    _priceValue = value;
                    Notify();                }
            }
        }
        public string CostValue
        {
            get
            {
                UpdateFromDetailsDb();
                return _costValue;
            }
            set
            {
                if (_costValue != value)
                {
                    _costValue = value;
                    Notify();                }
            }
        }
        public string Al
        {
            get
            {
                UpdateFromDetailsDb();
                return _al;
            }
            set
            {
                if (_al != value)
                {
                    _al = value;
                    Notify("AL");                }
            }
        }
        public string Int
        {
            get
            {
                UpdateFromDetailsDb();
                return _int;
            }
            set
            {
                if (_int != value)
                {
                    _int = value;
                    Notify();                }
            }
        }
        public string Wis
        {
            get
            {
                UpdateFromDetailsDb();
                return _wis;
            }
            set
            {
                if (_wis != value)
                {
                    _wis = value;
                    Notify();                }
            }
        }
        public string Cha
        {
            get
            {
                UpdateFromDetailsDb();
                return _cha;
            }
            set
            {
                if (_cha != value)
                {
                    _cha = value;
                    Notify();                }
            }
        }
        public string Ego
        {
            get
            {
                UpdateFromDetailsDb();
                return _ego;
            }
            set
            {
                if (_ego != value)
                {
                    _ego = value;
                    Notify();                }
            }
        }
        public string Communication
        {
            get
            {
                UpdateFromDetailsDb();
                return _communication;
            }
            set
            {
                if (_communication != value)
                {
                    _communication = value;
                    Notify();                }
            }
        }
        public string Senses
        {
            get
            {
                UpdateFromDetailsDb();
                return _senses;
            }
            set
            {
                if (_senses != value)
                {
                    _senses = value;
                    Notify();                }
            }
        }
        public string Powers
        {
            get
            {
                UpdateFromDetailsDb();
                return _powers;
            }
            set
            {
                if (_powers != value)
                {
                    _powers = value;
                    Notify();                }
            }
        }

        public string MagicItems
        {
            get => _magicItems;
            set
            {
                UpdateFromDetailsDb();
                if (_magicItems != value && value != "NULL")
                {
                    _magicItems = value;
                    Notify("MagicItem");                }
            }
        }

        public string DescHtml
        {
            get
            {
                UpdateFromDetailsDb();
                return _descHtml;
            }
            set
            {
                if (_descHtml != value)
                {
                    _descHtml = value;
                    Notify("DescHTML");                }
            }
        }

        public string Mythic
        {
            get
            {
                UpdateFromDetailsDb();
                return _mythic;
            }
            set
            {
                if (_mythic != value)
                {
                    _mythic = value;
                    Notify();                }
            }
        }
        public string LegendaryWeapon
        {
            get
            {
                UpdateFromDetailsDb();
                return _legendaryWeapon;
            }
            set
            {
                if (_legendaryWeapon != value)
                {
                    _legendaryWeapon = value;
                    Notify();                }
            }
        }


        public static MagicItem ByName(string name)
        {
            MagicItem item = null;
            if (_itemMap.TryGetValue(name, out item))
            {
                return item;
            }
            return null;
        }

    }
}
