/*
 *  TreasureGenerator.cs
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
    public class TreasureGenerator : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private int _level;
        private int _coin;
        private int _goods;
        private int _items;




        private static SortedDictionary<int, SortedDictionary<int, string>> _coinChart;
        private static SortedDictionary<int, SortedDictionary<int, string>> _goodsChart;
        private static SortedDictionary<int, SortedDictionary<int, string>> _itemChart;


        private static SortedDictionary<int, List<Spell>> _potionChart;
        private static SortedDictionary<int, List<Spell>> _arcaneScrollChart;
        private static SortedDictionary<int, List<Spell>> _divineScrollChart;
        private static SortedDictionary<int, List<Spell>> _wandChart;
        private static List<int> _potionLevelTotals;
        private static List<int> _arcaneScrollLevelTotals;
        private static List<int> _divineScrollLevelTotals;
        private static List<int> _wandLevelTotals;

        private static SortedDictionary<int, GemChart> _gemTypeChart;
        private static SortedDictionary<int, ArtChart> _artChart;

        private static RandomWeightChart<RandomItemType> _minorChart;
        private static RandomWeightChart<RandomItemType> _mediumChart;
        private static RandomWeightChart<RandomItemType> _majorChart;

        private static List<Equipment> _equipmentByVal;

        private static Random _rand = new Random();


        static TreasureGenerator()
        {
            LoadEquipmentChart();
            LoadTreasureChart();
            LoadGemChart();
            LoadArtChart();
            LoadRandomItemChart();
            LoadSpellItemCharts();


        }

        private static void LoadSpellItemCharts()
        {
            _potionChart = new SortedDictionary<int, List<Spell>>();
            _arcaneScrollChart = new SortedDictionary<int, List<Spell>>();
            _divineScrollChart = new SortedDictionary<int, List<Spell>>();
            _wandChart = new SortedDictionary<int, List<Spell>>();

            _potionLevelTotals = new List<int>();
            _arcaneScrollLevelTotals = new List<int>();
            _divineScrollLevelTotals = new List<int>();
            _wandLevelTotals = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                _potionChart[i] = new List<Spell>();
                _potionLevelTotals.Add(0);
            }
            for (int i = 0; i < 10; i++)
            {
                _arcaneScrollChart[i] = new List<Spell>();
                _arcaneScrollLevelTotals.Add(0);
            }
            for (int i = 0; i < 10; i++)
            {
                _divineScrollChart[i] = new List<Spell>();
                _divineScrollLevelTotals.Add(0);
            }
            for (int i = 0; i < 5; i++)
            {
                _wandChart[i] =  new List<Spell>();
                _wandLevelTotals.Add(0);
            }

            foreach (Spell spell in Spell.Spells)
            {
                if (spell.PotionWeight != null && spell.PotionLevel != null)
                {
                    int iLevel;
                    int weight;
                    if (int.TryParse(spell.PotionLevel, out iLevel) && int.TryParse(spell.PotionWeight, out weight))
                    {
                        _potionChart[iLevel].Add(spell);
                        _potionLevelTotals[iLevel] = _potionLevelTotals[iLevel] + weight;
                    }

                }
                if (spell.ArcaneScrollWeight != null && spell.ArcaneScrollLevel != null)
                {
                    try
                    {
                        int iLevel;
                        if (int.TryParse(spell.ArcaneScrollLevel, out iLevel))
                        {
                            _arcaneScrollChart[iLevel].Add(spell);
                            _arcaneScrollLevelTotals[iLevel] = _arcaneScrollLevelTotals[iLevel] + int.Parse(spell.ArcaneScrollWeight);
                        }
                    }
                    catch (Exception)
                    {
                        System.Diagnostics.Debug.WriteLine(spell.name + " " + spell.ArcaneScrollLevel + " " + spell.ArcaneScrollCost + " " + spell.ArcaneScrollWeight);
                        throw;
                    }


                }
                if (spell.DivineScrollWeight != null && spell.DivineScrollLevel != null)
                {

                    int iLevel;
                    int weight;
                    if (int.TryParse(spell.DivineScrollLevel, out iLevel) &&  int.TryParse(spell.DivineScrollWeight, out weight))
                    {
                        _divineScrollChart[iLevel].Add(spell);
                        _divineScrollLevelTotals[iLevel] = _divineScrollLevelTotals[iLevel] + weight;
                    }

                }
                if (spell.WandWeight != null && spell.WandLevel != null)
                {
                    int iLevel;
                    int weight;
                    if (int.TryParse(spell.WandLevel, out iLevel) && int.TryParse(spell.WandWeight, out weight))
                    {
                        _wandChart[iLevel].Add(spell);
                        _wandLevelTotals[iLevel] = _wandLevelTotals[iLevel] + weight;
                    }
                }
            }
        }

        private static void LoadTreasureChart()
        {

            List<TreasureChart> list = XmlListLoader<TreasureChart>.Load("TreasureChart.xml");


            _coinChart = new SortedDictionary<int, SortedDictionary<int, string>>();
            _goodsChart = new SortedDictionary<int, SortedDictionary<int, string>>();
            _itemChart = new SortedDictionary<int, SortedDictionary<int, string>>();
            foreach (TreasureChart item in list)
            {
                if (item.CoinRoll != null)
                {
                    if (!_coinChart.ContainsKey(item.Level))
                    {
                        _coinChart[item.Level] = new SortedDictionary<int, string>();
                    }
                    _coinChart[item.Level][item.CoinRoll.Value] = item.CoinValue;
                }
                if (item.GoodsRoll != null)
                {
                    if (!_goodsChart.ContainsKey(item.Level))
                    {
                        _goodsChart[item.Level] = new SortedDictionary<int, string>();
                    }
                    _goodsChart[item.Level][item.GoodsRoll.Value] = item.GoodsValue;
                }
                if (item.ItemsRoll != null)
                {
                    if (!_itemChart.ContainsKey(item.Level))
                    {
                        _itemChart[item.Level] = new SortedDictionary<int, string>();
                    }
                    _itemChart[item.Level][item.ItemsRoll.Value] = item.ItemsValue;
                }

            }
        }

        private static void LoadGemChart()
        {
            List<GemChart> list = XmlListLoader<GemChart>.Load("GemChart.xml");
            _gemTypeChart = new SortedDictionary<int, GemChart>();

            foreach (GemChart item in list)
            {
                _gemTypeChart.Add(item.Roll, item);
            }

        }

        private static void LoadArtChart()
        {
            List<ArtChart> list = XmlListLoader<ArtChart>.Load("ArtChart.xml");
            _artChart = new SortedDictionary<int, ArtChart>();

            foreach (ArtChart item in list)
            {
                _artChart.Add(item.Roll, item);
            }

        }

        private static void LoadEquipmentChart()
        {
            _equipmentByVal = new List<Equipment>();

            string[] validTypes = new string[] {
                "Adventuring Gear",
                "Tools and Skill Kits",
                "Special Substances",
                "Clothing",
                "Black Market"};

            foreach (Equipment eq in Equipment.AllItems)
            {
                if (validTypes.Contains(eq.Type))
                {
                    if (!eq.Name.StartsWith("Slave") && !eq.Name.StartsWith("Tatoo"))
                    {
                        _equipmentByVal.Add(eq);
                    }
                }
            }

            _equipmentByVal.Sort((a, b) => a.CoinCost.GpValue.CompareTo(b.CoinCost.GpValue));
        }

        [Flags]
        public enum RandomItemType
        {
            None = 0x0,
            Mundane10 = 0x1,
            Mundane11T50 = 0x2,
            Mundane51T100 = 0x4,
            Mundane100 = 0x8,
            Armor = 0x10,
            Weapon = 0x20,
            Potion = 0x40,
            Scroll = 0x80,
            MinorWondrous = 0x100,
            MagicalArmor = 0x200,
            MagicalWeapon = 0x400,
            Wand = 0x800,
            Ring = 0x1000,
            MediumWondrous = 0x2000,
            Rod = 0x4000,
            Staff = 0x8000,
            MajorWondrous = 0x10000
        }

        public static string RandomItemString(RandomItemType type)
        {
            switch (type)
            {
            case RandomItemType.Potion:
                return "Potion";
            case RandomItemType.Scroll:
                return "Scroll";
            case RandomItemType.MinorWondrous:
                return "Wondrous Item";
            case RandomItemType.MagicalArmor:
                return "Armor";
            case RandomItemType.MagicalWeapon:
                return "Weapon";
            case RandomItemType.Wand:
                return "Wand";
            case RandomItemType.Ring:
                return "Ring";
            case RandomItemType.MediumWondrous:
                return "Wondrous Item";
            case RandomItemType.Rod:
                return "Rod";
            case RandomItemType.Staff:
                return "Staff";
            case RandomItemType.MajorWondrous:
                return "Wondrous Item";

            }
            return "";
        }


        private static void LoadRandomItemChart()
        {
            _minorChart = new RandomWeightChart<RandomItemType>();
            _mediumChart = new RandomWeightChart<RandomItemType>();
            _majorChart = new RandomWeightChart<RandomItemType>();

            _minorChart.AddItem(16, RandomItemType.Mundane10);
            _minorChart.AddItem(14, RandomItemType.Mundane11T50);
            _minorChart.AddItem(10, RandomItemType.Mundane51T100);
            _minorChart.AddItem(4, RandomItemType.Mundane100);
            _minorChart.AddItem(11, RandomItemType.Armor);
            _minorChart.AddItem(14, RandomItemType.Weapon);
            _minorChart.AddItem(8, RandomItemType.Potion);
            _minorChart.AddItem(6, RandomItemType.Scroll);
            _minorChart.AddItem(5, RandomItemType.MinorWondrous);
            _minorChart.AddItem(3, RandomItemType.MagicalArmor);
            _minorChart.AddItem(5, RandomItemType.MagicalWeapon);
            _minorChart.AddItem(2, RandomItemType.Wand);
            _minorChart.AddItem(2, RandomItemType.Ring);

            _mediumChart.AddItem(4, RandomItemType.Mundane51T100);
            _mediumChart.AddItem(10, RandomItemType.Mundane100);
            _mediumChart.AddItem(2, RandomItemType.Armor);
            _mediumChart.AddItem(3, RandomItemType.Weapon);
            _mediumChart.AddItem(2, RandomItemType.Rod);
            _mediumChart.AddItem(2, RandomItemType.Staff);
            _mediumChart.AddItem(12, RandomItemType.Potion);
            _mediumChart.AddItem(10, RandomItemType.Scroll);
            _mediumChart.AddItem(8, RandomItemType.MinorWondrous);
            _mediumChart.AddItem(15, RandomItemType.MagicalArmor);
            _mediumChart.AddItem(15, RandomItemType.MagicalWeapon);
            _mediumChart.AddItem(8, RandomItemType.Wand);
            _mediumChart.AddItem(4, RandomItemType.Ring);
            _mediumChart.AddItem(5, RandomItemType.MediumWondrous);



            _majorChart.AddItem(10, RandomItemType.Potion);
            _majorChart.AddItem(12, RandomItemType.Scroll);
            _majorChart.AddItem(4, RandomItemType.MinorWondrous);
            _majorChart.AddItem(12, RandomItemType.MagicalArmor);
            _majorChart.AddItem(18, RandomItemType.MagicalWeapon);
            _majorChart.AddItem(10, RandomItemType.Wand);
            _majorChart.AddItem(8, RandomItemType.Ring);
            _majorChart.AddItem(10, RandomItemType.MediumWondrous);
            _majorChart.AddItem(6, RandomItemType.Rod);
            _majorChart.AddItem(4, RandomItemType.Staff);
            _majorChart.AddItem(6, RandomItemType.MajorWondrous);
                       
        }             


        public TreasureGenerator()
        {            
            _level = 1;
            _coin = 1;
            _goods = 1;
            _items = 1;
        }


        public int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Level")); }
                }
            }
        }

        public Treasure Generate()
        {
            Treasure treasure = new Treasure();
            treasure.Coin = new Coin();

            if (_coin > 0)
            {
                for (int i = 0; i < _coin; i++)
                {
                    treasure.Coin = treasure.Coin + GenerateCoin();
                }
                
            }

            if (_goods > 0)
            {
                for (int i = 0; i < _goods; i++)
                {
                    treasure.Goods.AddRange(GenerateGoods());
                }
            }
            treasure.Goods.Sort((a, b) => a.Name.CompareTo(b.Name));

            if (_items > 0)
            {
                for (int i = 0; i < _items; i++)
                {
                    treasure.Items.AddRange(GenerateItems());
                }
            }

            treasure.Items.Sort((a, b) => a.Name.CompareTo(b.Name));

            return treasure;
        }

        private Coin GenerateCoin()
        {
            int roll = _rand.Next(1, 101);

            int actualLevel = Math.Min(Level, 20);

            string coinString = null;

            foreach (KeyValuePair<int, string> pair in _coinChart[actualLevel])
            {
                if (pair.Key >= roll)
                {
                    coinString = pair.Value;
                    break;
                }
            }


            return RandomCoinFromString(coinString);
        }

        private List<Good> GenerateGoods()
        {
            int roll = _rand.Next(1, 101);

            int actualLevel = Math.Min(Level, 20);


            string goodsString = null;

            foreach (KeyValuePair<int, string> pair in _goodsChart[actualLevel])
            {
                if (pair.Key >= roll)
                {
                    goodsString = pair.Value;
                    break;
                }
            }

            return RandomGoodsFromString(goodsString);

        }

        private List<TreasureItem> GenerateItems()
        {


            int roll = _rand.Next(1, 101);

            int actualLevel = Math.Min(Level, 20);

            string itemString = null;

            foreach (KeyValuePair<int, string> pair in _itemChart[actualLevel])
            {
                if (pair.Key >= roll)
                {
                    itemString = pair.Value;
                    break;
                }
            }


            return RandomItemsFromString(itemString);
        }



        public static Coin RandomCoinFromString(string coinString)
        {
            Coin coin = new Coin();
            Regex coinVal = new Regex("(?<die>[0-9]+d[0-9]+)(?<hasbase> (x|×) (?<base>[0-9]+))? (?<type>(pp|gp|sp|cp))");

            if (coinString != null)
            {
                Match m = coinVal.Match(coinString);

                if (m.Success)
                {
                    string die = m.Groups["die"].Value;

                    int baseVal = 1;

                    if (m.Groups["hasbase"].Success)
                    {
                        baseVal = int.Parse(m.Groups["base"].Value);
                    }
                    string type = m.Groups["type"].Value;

                    DieRoll dieroll = DieRoll.FromString(die);

                    double mult = dieroll.Step.RollDouble(false);
                    int val = (int)(mult * (double)baseVal);

                    if (type == "pp")
                    {
                        coin.Pp = val;
                    }
                    else if (type == "gp")
                    {
                        coin.Gp = val;
                    }
                    else if (type == "sp")
                    {
                        coin.Sp = val;
                    }
                    else if (type == "cp")
                    {
                        coin.Cp = val;
                    }

                }
            }
            return coin;
        }

        public List<Good> RandomGoodsFromString(string goodsString)
        {
            List<Good> goods = new List<Good>();

            if (goodsString != null)
            {
                Regex regGoods = new Regex("((?<single>1)|(?<die>[0-9]+d[0-9]+)) ((?<type>art|gem))");


                Match m = regGoods.Match(goodsString);

                if (m.Success)
                {
                    int count = 0;

                    if (m.Groups["single"].Success)
                    {
                        count = 1;
                    }
                    else
                    {
                        DieRoll roll = DieRoll.FromString(m.Groups["die"].Value);

                        count = roll.Step.Roll();
                    }

                    bool gems = m.Groups["type"].Value == "gem";

                    for (int i = 0; i < count; i++)
                    {
                        goods.Add(gems ? GenerateRandomGem() : GenerateRandomArt());
                    }

                }
            }

            return goods;
        }

        public List<TreasureItem> RandomItemsFromString(string itemString)
        {
            List<TreasureItem> items = new List<TreasureItem>();

            if (itemString != null)
            {
                Regex regGoods = new Regex("((?<single>1)|(?<die>[0-9]+d[0-9]+)) ((?<type>mundane|minor|medium|major))");

                Match m = regGoods.Match(itemString);

                if (m.Success)
                {
                    int count = 0;

                    if (m.Groups["single"].Success)
                    {
                        count = 1;
                    }
                    else
                    {
                        DieRoll roll = DieRoll.FromString(m.Groups["die"].Value);

                        count = roll.Step.Roll();
                    }

                    string type = m.Groups["type"].Value;


                    for (int i = 0; i < count; i++)
                    {
                        if (type == "mundane")
                        {

                        }
                        else if (type == "minor")
                        {
                            TreasureItem item = GenerateRandomItem(ItemLevel.Minor);
                            if (item != null)
                            {
                                items.Add(item);
                            }
                        }
                        else if (type == "medium")
                        {

                            TreasureItem item = GenerateRandomItem(ItemLevel.Medium);
                            if (item != null)
                            {
                                items.Add(item);
                            }
                        }
                        else if (type == "major")
                        {
                            TreasureItem item = GenerateRandomItem(ItemLevel.Major);
                            if (item != null)
                            {
                                items.Add(item);
                            }

                        }
                    }

                }
            }

            return items;
        }

        public Good GenerateRandomGem()
        {
            Good g = new Good();

            int roll = _rand.Next(1, 101);

            foreach (KeyValuePair<int, GemChart> pair in _gemTypeChart)
            {
                if (pair.Key >= roll)
                {
                    GemChart chart = pair.Value;

                    g.Value = RandomCoinFromString(chart.Value).Gp;
                    g.Name = RandomItemFromList(chart.Gem);

                    break;
                }
            }

            return g;
        }

        public Good GenerateRandomArt()
        {
            Good g = new Good();

            int roll = _rand.Next(1, 101);

            foreach (KeyValuePair<int, ArtChart> pair in _artChart)
            {
                if (pair.Key >= roll)
                {
                    ArtChart chart = pair.Value;

                    g.Value = RandomCoinFromString(chart.Value).Gp; ;
                    g.Name = RandomItemFromList(chart.Art);

                    break;
                }
            }

            return g;

        }

        
        public TreasureItem GenerateRandomItem(ItemLevel level)
        {
            RandomItemType types = 0;
            foreach (RandomItemType type in Enum.GetValues(typeof(RandomItemType)))
            {
                types |= type;
            }
            return GenerateRandomItem(level, types);
        }

        public TreasureItem GenerateRandomItem(ItemLevel level, RandomItemType types)
        {
            TreasureItem item = null;


            RandomWeightChart<RandomItemType> list;

            switch (level)
            {
                case ItemLevel.Minor:
                    list = _minorChart;
                    break;
                case ItemLevel.Medium:
                    list = _mediumChart;
                    break;
                case ItemLevel.Major:
                default:
                    list = _majorChart;
                    break;
            }

            while (item == null)
            {

                RandomItemType type = list.GetRandomItem();

                if (types.HasFlag(type))
                {

                    switch (type)
                    {
                        case RandomItemType.Mundane10:
                            item = GenerateMundaneEquipment(0, 10);
                            break;
                        case RandomItemType.Mundane11T50:
                            item = GenerateMundaneEquipment(11, 50);
                            break;
                        case RandomItemType.Mundane51T100:
                            item = GenerateMundaneEquipment(51, 100);
                            break;
                        case RandomItemType.Mundane100:
                            item = GenerateMundaneEquipment(100, int.MaxValue);
                            break;
                        case RandomItemType.Armor:
                            item = GenerateArmor(true);
                            break;
                        case RandomItemType.Weapon:
                            item = GenerateWeapon(true);
                            break;
                        case RandomItemType.Scroll:
                            item = GenerateScroll(level);
                            break;
                        case RandomItemType.Potion:
                            item = GeneratePotion(level);
                            break;
                        case RandomItemType.MagicalArmor:
                            item = GenerateMagicalArmor(level);
                            break;
                        case RandomItemType.MagicalWeapon:
                            item = GenerateMagicalWeapon(level);
                            break;
                        case RandomItemType.MajorWondrous:
                            item = GenerateWondrousItem(ItemLevel.Major);
                            break;
                        case RandomItemType.MediumWondrous:
                            item = GenerateWondrousItem(ItemLevel.Medium);
                            break;
                        case RandomItemType.MinorWondrous:
                            item = GenerateWondrousItem(ItemLevel.Minor);
                            break;
                        case RandomItemType.Rod:
                            item = GenerateRod(level);
                            break;
                        case RandomItemType.Staff:
                            item = GenerateStaff(level);
                            break;
                        case RandomItemType.Wand:
                            item = GenerateWand(level);
                            break;
                        case RandomItemType.Ring:
                            item = GenerateRing(level);
                            break;
                    }
                }
            }

            return item;

        }

        public TreasureItem GenerateMundaneEquipment(int minval, int maxval)
        {
            TreasureItem item = null;

            int minItem = -1;
            int maxItem = -1;

            for (int i = 0; i < _equipmentByVal.Count; i++)
            {
                if (minItem == -1 && _equipmentByVal[i].CoinCost.GpValue > minval)
                {
                    minItem = i;
                }
                if (maxItem == -1 && _equipmentByVal[i].CoinCost.GpValue > maxval)
                {
                    maxItem = i;
                }
            }

            if (minItem == -1)
            {
                minItem = 0;
            }
            if (maxItem == -1)
            {
                maxItem = _equipmentByVal.Count;
            }

            if (minItem < maxItem)
            {

                int roll = _rand.Next(minItem, maxItem);
                Equipment eq = _equipmentByVal[roll];

                item = new TreasureItem();
                item.Equipment = eq;
                item.Name = eq.Name;
                item.Value = eq.CoinCost.GpValue;
                item.Type = "Equipment";
            }

            return item;
        }

        public TreasureItem GenerateArmor(bool masterwork)
        {
            int roll = _rand.Next(1, 101);

            string type = "";

            if (roll <= 55)
            {
                type = "Armor";
            }
            else if (roll <= 100)
            {
                type = "Shield";
            }



            return GetArmorWeaponChartItem("Armor", type, masterwork);
        }


        public TreasureItem GenerateWeapon(bool masterwork)
        {
            int roll = _rand.Next(1, 101);

            string type = "";

            if (roll <= 45)
            {
                type = "Simple";
            }
            else if (roll <= 80)
            {
                type = "Martial";
            }
            else if (roll <= 100)
            {
                type = "Exotic";
            }



            return GetArmorWeaponChartItem("Weapon", type, masterwork);
        }

        private TreasureItem GetArmorWeaponChartItem(string type, string subtype, bool masterwork)
        {
            TreasureItem item = null;

            int val = ArmorWeaponChart.TotalWeights[subtype];

            string withName = null;
            int withValue = 0;
            while (item == null)
            {


                int totalWeight = 0;
                int cRoll = _rand.Next(1, val);
                foreach (ArmorWeaponChart chart in
                        ArmorWeaponChart.Chart.Where(a => string.Compare(a.Type, subtype) == 0))
                {
                    totalWeight += int.Parse(chart.Weight);

                    if (totalWeight >= cRoll)
                    {

                        if (chart.Name.StartsWith("with"))
                        {
                            if (withName == null)
                            {
                                withName = chart.Name;
                                withValue = GpToInt(chart.Cost);
                            }

                        }
                        else
                        {
                            item = new TreasureItem();
                            item.Name = (masterwork ? "Masterwork " : "") + chart.Name + ((withName != null) ? (" " + withName) : "");

                            item.Type = type;
                            item.Value = GpToInt(chart.Cost) + withValue;

                        }
                        break;

                    }
                }
            }

            return item;

        }

        public TreasureItem GeneratePotion(ItemLevel level)
        {
            int roll = _rand.Next(1, 101);

            int potionLevel = 0;

            switch (level)
            {
                case ItemLevel.Minor:
                    if (roll <= 20)
                    {
                        potionLevel = 0;
                    }
                    else if (roll <= 60)
                    {
                        potionLevel = 1;
                    }
                    else if (roll <= 100)
                    {
                        potionLevel = 2;
                    }

                    break;
                case ItemLevel.Medium:
                    if (roll <= 20)
                    {
                        potionLevel = 1;
                    }
                    else if (roll <= 60)
                    {
                        potionLevel = 2;
                    }
                    else if (roll <= 100)
                    {
                        potionLevel = 3;
                    }

                    break;
                case ItemLevel.Major:
                    if (roll <= 20)
                    {
                        potionLevel = 2;
                    }
                    else if (roll <= 100)
                    {
                        potionLevel = 3;
                    }

                    break;

            }


            return GeneratePotionOfLevel(potionLevel);
        }
        public TreasureItem GeneratePotionOfLevel(int level)
        {
            TreasureItem item = null;
            Spell spell = null;

            int roll = _rand.Next(0, _potionLevelTotals[level]);
            int current = 0;
            foreach (Spell val in _potionChart[level])
            {
                current += int.Parse(val.PotionWeight);

                if (current > roll)
                {
                    spell = val;
                    break;
                }
            }

            if (spell != null)
            {
                item = new TreasureItem();
                item.Spell = spell;
                item.Type = "Potion";
                item.Name = "Potion of " + spell.name;
                item.Value = GpToInt(spell.PotionCost);
            }

            return item;
        }
        public TreasureItem GenerateArcaneScrollOfLevel(int level)
        {
            TreasureItem item = null;
            Spell spell = null;

            int roll = _rand.Next(0, _arcaneScrollLevelTotals[level]);
            int current = 0;
            foreach (Spell val in _arcaneScrollChart[level])
            {
                current += int.Parse(val.ArcaneScrollWeight);

                if (current > roll)
                {
                    spell = val;
                    break;
                }
            }

            if (spell != null)
            {
                item = new TreasureItem();
                item.Spell = spell;
                item.Type = "Scroll";
                item.Name = "Scroll of " + spell.name;
                item.Value = GpToInt(spell.ArcaneScrollCost);
            }

            return item;
        }
        public TreasureItem GenerateDivineScrollOfLevel(int level)
        {
            TreasureItem item = null;
            Spell spell = null;

            int roll = _rand.Next(0, _divineScrollLevelTotals[level]);
            int current = 0;
            foreach (Spell val in _divineScrollChart[level])
            {
                current += int.Parse(val.DivineScrollWeight);

                if (current > roll)
                {
                    spell = val;
                    break;
                }
            }

            if (spell != null)
            {
                item = new TreasureItem();
                item.Spell = spell;
                item.Type = "Scroll";
                item.Name = "Scroll of " + spell.name;
                item.Value = GpToInt(spell.DivineScrollCost);
            }

            return item;
        }

        public TreasureItem GenerateWandOfLevel(int level, bool fullCharge)
        {
            TreasureItem item = null;
            Spell spell = null;

            int roll = _rand.Next(0, _wandLevelTotals[level]);
            int current = 0;
            foreach (Spell val in _wandChart[level])
            {
                current += int.Parse(val.WandWeight);

                if (current > roll)
                {
                    spell = val;
                    break;
                }
            }

            if (spell != null)
            {
                int charges = 50;

                if (!fullCharge)
                {
                    _rand.Next(1, 51);
                }


                item = new TreasureItem(); 
                item.Spell = spell;
                item.Type = "Wand";
                item.Name = "Wand of " + spell.name + ", " + charges + " charges";
                item.Value = GpToInt(spell.WandCost);

                if (charges < 50)
                {
                    item.Value = item.Value * charges / 50;
                }
            }

            return item;
        }

        public static int GpToInt(string gp)
        {
            int val = 0;

            if (gp != null)
            {

                string parseGp = Regex.Replace(gp, "gp|,|\\+", "");

                int.TryParse(parseGp, out val);
            }

            return val;
        }

        public TreasureItem GenerateScroll(ItemLevel level)
        {

            int roll = _rand.Next(1, 101);

            if (roll <= 70)
            {
                return GenerateScroll(level, true);
            }
            else
            {
                return GenerateScroll(level, false);
            }

        }

        public TreasureItem GenerateScroll(ItemLevel level, bool arcane)
        {

            int roll = _rand.Next(1, 101);

            int scrollLevel = 0;

            switch (level)
            {
                case ItemLevel.Minor:
                    if (roll <= 5)
                    {
                        scrollLevel = 0;
                    }
                    else if (roll <= 50)
                    {
                        scrollLevel = 1;
                    }
                    else if (roll <= 95)
                    {
                        scrollLevel = 2;
                    }
                    else if (roll <= 100)
                    {
                        scrollLevel = 3;
                    }

                    break;
                case ItemLevel.Medium:
                    if (roll <= 5)
                    {
                        scrollLevel = 2;
                    }
                    else if (roll <= 65)
                    {
                        scrollLevel = 3;
                    }
                    else if (roll <= 95)
                    {
                        scrollLevel = 4;
                    }
                    else if (roll <= 100)
                    {
                        scrollLevel = 5;
                    }

                    break;
                case ItemLevel.Major:
                    if (roll <= 5)
                    {
                        scrollLevel = 4;
                    }
                    else if (roll <= 50)
                    {
                        scrollLevel = 5;
                    }
                    else if (roll <= 70)
                    {
                        scrollLevel = 6;
                    }
                    else if (roll <= 86)
                    {
                        scrollLevel = 7;
                    }
                    else if (roll <= 95)
                    {
                        scrollLevel = 8;
                    }
                    else if (roll <= 100)
                    {
                        scrollLevel = 9;
                    }

                    break;
            }

            if (arcane)
            {
                return GenerateArcaneScrollOfLevel(scrollLevel);
            }
            else
            {
                return GenerateDivineScrollOfLevel(scrollLevel);
            }
        }
        public TreasureItem GenerateWondrousItem(ItemLevel level)
        {
            TreasureItem item = new TreasureItem();

            SpecificItemChart chart = GenerateSpecificItem(level, "Wondrous Item");

            item.Name = chart.Name;

            if (MagicItem.Items.ContainsKey(chart.Name))
            {
                item.MagicItem = MagicItem.Items[chart.Name];
            }

            item.Value = GpToInt(chart.Cost);
            item.Type = "Wondrous Item";

            return item;
        }

        public TreasureItem GenerateMagicalWeapon(ItemLevel level)
        {
            TreasureItem item = null;


            item = GenerateWeapon(false);

            int bonus = 0;
            int specialBonus = 0;
            int specialCost = 0;
            bool specificWeapon = false;
            string weaponSpecial = "";

            RunSpecialType addSpecial = delegate()
            {
                int bonusIncrease = 0;
                int costIncrease = 0;

                Weapon wp = Weapon.Find(item.Name);

                string type = "Melee";

                if (wp != null && wp.Ranged)
                {
                    type = "Ranged";
                }

                

                string newSpecial = GenerateSpecial(type, level, weaponSpecial, out bonusIncrease, out costIncrease);

                if (bonusIncrease + specialBonus <= 9)
                {
                    if (weaponSpecial.Length > 0)
                    {
                        weaponSpecial += " ";
                    }
                    weaponSpecial += newSpecial;
                    specialBonus += bonusIncrease;
                    specialCost += costIncrease;
                }
            };



            while (bonus == 0 && !specificWeapon)
            {
                int roll = _rand.Next(1, 101);
                switch (level)
                {
                    case ItemLevel.Minor:
                        if (roll <= 70)
                        {
                            bonus = 1;
                        }
                        else if (roll <= 85)
                        {
                            bonus = 2;
                        }
                        else if (roll <= 90)
                        {
                            specificWeapon = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                    case ItemLevel.Medium:
                        if (roll <= 10)
                        {
                            bonus = 1;
                        }
                        else if (roll <= 29)
                        {
                            bonus = 2;
                        }
                        else if (roll <= 58)
                        {
                            bonus = 3;
                        }
                        else if (roll <= 62)
                        {
                            bonus = 4;
                        }
                        else if (roll <= 68)
                        {
                            specificWeapon = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                    case ItemLevel.Major:
                        if (roll <= 20)
                        {
                            bonus = 3;
                        }
                        else if (roll <= 38)
                        {
                            bonus = 4;
                        }
                        else if (roll <= 49)
                        {
                            bonus = 5;
                        }
                        else if (roll <= 63)
                        {
                            specificWeapon = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                }
            }

            if (bonus > 0)
            {
                if (bonus + specialBonus > 10)
                {
                    bonus = 10 - specialBonus;
                }


                item.Name = "+" + bonus + ((weaponSpecial.Length > 0) ? (" " + weaponSpecial + " ") : " ") + item.Name;
                item.Type = "Magical Weapon";
                item.Value = item.Value + BonusGpValue(bonus + specialBonus, true) + specialCost;
            }
            else if (specificWeapon)
            {
                SpecificItemChart chart = GenerateSpecificItem(level, "Weapon");

                item.Name = chart.Name;
                item.Value = GpToInt(chart.Cost);
                item.Type = "Magical Weapon";
                if (MagicItem.Items.ContainsKey(chart.Name))
                {
                    item.MagicItem = MagicItem.Items[chart.Name];
                }

            }


            return item;
        }

        delegate void RunSpecialType();

        public TreasureItem GenerateMagicalArmor(ItemLevel level)
        {
            TreasureItem item = null;

            //genrate item used (if not specific armor or sheild)
            item = GenerateArmor(false);


            int bonus = 0;
            int specialBonus = 0;
            int specialCost = 0;
            bool specificShield = false;
            bool specificArmor = false;
            string weaponSpecial = "";

            RunSpecialType addSpecial = delegate()
                {
                    int bonusIncrease = 0;
                    int costIncrease = 0;
                    string newSpecial = GenerateSpecial(item.Type, level, weaponSpecial, out bonusIncrease, out costIncrease);


                    if (bonusIncrease + specialBonus <= 9)
                    {
                        if (weaponSpecial.Length > 0)
                        {
                            weaponSpecial += " ";
                        }
                        weaponSpecial += newSpecial;
                        specialBonus += bonusIncrease;
                        specialCost += costIncrease;
                    }
                };



            while (bonus == 0 && !specificArmor && !specificShield)
            {

                int roll = _rand.Next(1, 101);
                switch (level)
                {
                    case ItemLevel.Minor:
                        if (roll <= 80)
                        {
                            bonus = 1;
                        }
                        else if (roll <= 87)
                        {
                            bonus = 2;
                        }
                        else if (roll <= 89)
                        {
                            specificArmor = true;
                        }
                        else if (roll <= 91)
                        {
                            specificShield = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                    case ItemLevel.Medium:
                        if (roll <= 10)
                        {
                            bonus = 1;
                        }
                        else if (roll <= 30)
                        {
                            bonus = 2;
                        }
                        else if (roll <= 50)
                        {
                            bonus = 3;
                        }
                        else if (roll <= 57)
                        {
                            bonus = 4;
                        }
                        else if (roll <= 60)
                        {
                            specificArmor = true;
                        }
                        else if (roll <= 63)
                        {
                            specificShield = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                    case ItemLevel.Major:
                        if (roll <= 16)
                        {
                            bonus = 3;
                        }
                        else if (roll <= 38)
                        {
                            bonus = 4;
                        }
                        else if (roll <= 57)
                        {
                            bonus = 5;
                        }
                        else if (roll <= 60)
                        {
                            specificArmor = true;
                        }
                        else if (roll <= 63)
                        {
                            specificShield = true;
                        }
                        else if (roll <= 100)
                        {
                            addSpecial();
                        }
                        break;
                }
            }

            if (bonus > 0)
            {
                if (bonus + specialBonus > 10)
                {
                    bonus = 10 - specialBonus;
                }

                item.Name = "+" + bonus + ((weaponSpecial.Length > 0) ? (" " + weaponSpecial + " ") : " ") + item.Name;
                item.Type = "Magical " + item.Type;
                item.Value = item.Value + BonusGpValue(bonus + specialBonus, false) + specialCost;
            }
            else if (specificArmor)
            {

                SpecificItemChart chart = GenerateSpecificItem(level, "Armor");
                if (MagicItem.Items.ContainsKey(chart.Name))
                {
                    item.MagicItem = MagicItem.Items[chart.Name];
                }

                item.Name = chart.Name;
                item.Value = GpToInt(chart.Cost);
                item.Type = "Magical Weapon";
            }
            else if (specificShield)
            {

                SpecificItemChart chart = GenerateSpecificItem(level, "Shield");

                item.Name = chart.Name;
                item.Value = GpToInt(chart.Cost);
                item.Type = "Magical Weapon";
            }


            return item;
        }


        public string GenerateSpecial(string type, ItemLevel level, string currentSpecial, out int bonusIncrease, out int costIncrease)
        {
            bonusIncrease = 0;
            costIncrease = 0;

            List<ArmorWeaponSpecialChart> subchart = new List<ArmorWeaponSpecialChart>(ArmorWeaponSpecialChart.Chart.Where(
                delegate(ArmorWeaponSpecialChart chart)
                {
                    if (chart.Type != type)
                    {
                        return false;
                    }

                    return chart.LevelWeight(level) != null;

                }));

            int roll = _rand.Next(1, 101);

            ArmorWeaponSpecialChart special = null;

            int val = 0;

            foreach (ArmorWeaponSpecialChart chart in subchart)
            {
                val += int.Parse(chart.LevelWeight(level));

                if (val >= roll)
                {
                    special = chart;
                    break;
                }
            }

            string specOut = "";

            if (special == null)
            {
                //run twice
                int bonus = 0;
                int cost = 0;

                specOut = GenerateSpecial(type, level, currentSpecial, out bonus, out cost);                                                
                bonusIncrease += bonus;
                costIncrease += cost;

                currentSpecial = specOut + " " + currentSpecial;

                specOut += " " + GenerateSpecial(type, level, currentSpecial, out bonus, out cost);
                bonusIncrease += bonus;
                costIncrease += cost;

            }
            else
            {
                if (!currentSpecial.Contains(special.Name))
                {

                    specOut = special.Name;
                    if (special.Bonus != null && special.Bonus.Length > 0)
                    {
                        bonusIncrease = int.Parse(special.Bonus);
                    }
                    if (special.Cost != null && special.Cost.Length > 0)
                    {
                        costIncrease = int.Parse(special.Cost);
                    }
                }
            }

            return specOut;
        }


        public int BonusGpValue(int bonus, bool weapon)
        {
            return bonus*bonus*(weapon?2000:1000);
        }

        public TreasureItem GenerateWand(ItemLevel level)
        {
           int roll = _rand.Next(1, 101);

            int wandLevel = 0;

            switch (level)
            {
                case ItemLevel.Minor:
                    if (roll <= 5)
                    {
                        wandLevel = 0;
                    }
                    else if (roll <= 60)
                    {
                        wandLevel = 1;
                    }
                    else if (roll <= 100)
                    {
                        wandLevel = 2;
                    }
                    break;
                case ItemLevel.Medium:
                    if (roll <= 60)
                    {
                        wandLevel = 2;
                    }
                    else if (roll <= 100)
                    {
                        wandLevel = 3;
                    }
                    break;
                case ItemLevel.Major:
                    if (roll <= 60)
                    {
                        wandLevel = 3;
                    }
                    else if (roll <= 100)
                    {
                        wandLevel = 4;
                    }
                    break;
            }
            return GenerateWandOfLevel(wandLevel, false);
        }
        public TreasureItem GenerateRing(ItemLevel level)
        {
            TreasureItem item = new TreasureItem();

            SpecificItemChart chart = GenerateSpecificItem(level, "Ring");
            item.Name = "Ring of " + chart.Name;
            if (MagicItem.Items.ContainsKey(item.Name))
            {
                item.MagicItem = MagicItem.Items[item.Name];
            }

            item.Value = GpToInt(chart.Cost);
            item.Type = "Ring";

            return item;
        }
        public TreasureItem GenerateRod(ItemLevel level)
        {
            TreasureItem item = new TreasureItem();

            SpecificItemChart chart = GenerateSpecificItem(level, "Rod");
            item.Name = "Rod of " + chart.Name;
            item.MagicItem = MagicItem.ByName(item.Name);
            item.Value = GpToInt(chart.Cost);
            item.Type = "Rod";

            return item;
        }
        public TreasureItem GenerateStaff(ItemLevel level)
        {
            TreasureItem item = new TreasureItem();

            SpecificItemChart chart = GenerateSpecificItem(level, "Staff");
            item.Name = "Staff of " + chart.Name;
            if (MagicItem.Items.ContainsKey(chart.Name))
            {
                item.MagicItem = MagicItem.Items[item.Name];
            }

            item.Value = GpToInt(chart.Cost);
            item.Type = "Staff";

            return item;
        }

        public SpecificItemChart GenerateSpecificItem(ItemLevel level, string type)
        {
            int roll = _rand.Next(1, SpecificItemChart.ChartTotal(level, type) + 1);
            int val = 0;

            foreach (SpecificItemChart chart in SpecificItemChart.Subchart(level, type))
            {
                val += int.Parse(chart.LevelWeight(level));

                if (val >= roll)
                {
                    return chart;
                }
            }

            return null;


        }



        private static string RandomItemFromList(string items)
        {
            List<string> list = TreasureStringList(items);

            if (list.Count > 0)
            {
                return list[_rand.Next(0, list.Count)];
            }

            return "";
        }

        public static List<string> TreasureStringList(string items)
        {
            List<string> list = new List<string>();

            Regex regList = new Regex("(?<value>.+?)(; |$)");

            foreach (Match m in regList.Matches(items))
            {
                list.Add(m.Groups["value"].Value);
            }

            return list;
        }

        public int Coin
        {
            get => _coin;
            set
            {
                if (_coin != value)
                {
                    _coin = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Coin")); }
                }
            }
        }
        public int Goods
        {
            get => _goods;
            set
            {
                if (_goods != value)
                {
                    _goods = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Goods")); }
                }
            }
        }
        public int Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Items")); }
                }
            }
        }

    }
    public class GemChart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _roll;
        private string _value;
        private int _averageValue;
        private string _gem;

        public int Roll
        {
            get => _roll;
            set
            {
                if (_roll != value)
                {
                    _roll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Roll")); }
                }
            }
        }
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
                }
            }
        }
        public int AverageValue
        {
            get => _averageValue;
            set
            {
                if (_averageValue != value)
                {
                    _averageValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AverageValue")); }
                }
            }
        }
        public string Gem
        {
            get => _gem;
            set
            {
                if (_gem != value)
                {
                    _gem = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Gem")); }
                }
            }
        }


    }

    public class ArtChart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _roll;
        private string _value;
        private int _averageValue;
        private string _art;

        public int Roll
        {
            get => _roll;
            set
            {
                if (_roll != value)
                {
                    _roll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Roll")); }
                }
            }
        }
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
                }
            }
        }
        public int AverageValue
        {
            get => _averageValue;
            set
            {
                if (_averageValue != value)
                {
                    _averageValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AverageValue")); }
                }
            }
        }
        public string Art
        {
            get => _art;
            set
            {
                if (_art != value)
                {
                    _art = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Art")); }
                }
            }
        }


    }

    public class TreasureChart : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int _level;
        private int? _coinRoll;
        private string _coinValue;
        private int? _goodsRoll;
        private string _goodsValue;
        private int? _itemsRoll;
        private string _itemsValue;

        public int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Level")); }
                }
            }
        }
        public int? CoinRoll
        {
            get => _coinRoll;
            set
            {
                if (_coinRoll != value)
                {
                    _coinRoll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CoinRoll")); }
                }
            }
        }
        public string CoinValue
        {
            get => _coinValue;
            set
            {
                if (_coinValue != value)
                {
                    _coinValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CoinValue")); }
                }
            }
        }
        public int? GoodsRoll
        {
            get => _goodsRoll;
            set
            {
                if (_goodsRoll != value)
                {
                    _goodsRoll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("GoodsRoll")); }
                }
            }
        }
        public string GoodsValue
        {
            get => _goodsValue;
            set
            {
                if (_goodsValue != value)
                {
                    _goodsValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("GoodsValue")); }
                }
            }
        }
        public int? ItemsRoll
        {
            get => _itemsRoll;
            set
            {
                if (_itemsRoll != value)
                {
                    _itemsRoll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ItemsRoll")); }
                }
            }
        }
        public string ItemsValue
        {
            get => _itemsValue;
            set
            {
                if (_itemsValue != value)
                {
                    _itemsValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ItemsValue")); }
                }
            }
        }


    }
}
