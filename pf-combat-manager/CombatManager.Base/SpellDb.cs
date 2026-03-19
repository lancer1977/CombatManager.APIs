//namespace CombatManager;

//public class SpellDb
//{
//    private DBLoader<Spell> _dbLoader;

//    private static SpellDb _db;

//    private bool _mythicCorrected;

//    public static event Action<Spell> SpellUpdated;


//    private static ObservableCollection<Spell> _spells;

//    private static Dictionary<int, Spell> _spellsByDetailsId;

//    private static SortedDictionary<string, string> _schools;
//    private static SortedSet<string> _subschools;
//    private static SortedSet<string> _descriptors;
//    private static Dictionary<string, ObservableCollection<Spell>> _spellDictionary;

//    private static SortedSet<string> _castingTimeOptions;
//    private static SortedSet<string> _rangeOptions;
//    private static SortedSet<string> _areaOptions;
//    private static SortedSet<string> _targetsOptions;
//    private static SortedSet<string> _durationOptions;
//    private static SortedSet<string> _savingThrowOptions;
//    private static SortedSet<string> _spellResistanceOptions;
//    static void LoadSpells()
//    {
//        List<Spell> set = XmlListLoader<Spell>.Load("SpellsShort.xml");

//        List<Spell> remove = new List<Spell>();
//        foreach (var cur in set)
//        {

//            //This needs to be removed at some point.  We shouldn't change the duration/areas field to add tags to the ui
//            if (cur.Dismissible == "1")
//            {
//                if (!cur.Duration.Contains("(D)"))
//                {
//                    cur.Duration += " (D)";
//                }
//            }

//            if (cur.Shapeable == "1")
//            {
//                if (!cur.Area.Contains("(S)"))
//                {
//                    cur.Area += " (S)";
//                }
//            }

//            if (cur._duplicated)
//            {
//                remove.Add(cur);
//            }


//        }

//        foreach (Spell s in remove)
//        {
//            set.Remove(s);
//        }


//        _spellsByDetailsId = new Dictionary<int, Spell>();
//        foreach (Spell s in set)
//        {
//            _spellsByDetailsId[s.Detailsid] = s;

//        }

//        _spells = new ObservableCollection<Spell>(set);

//        if (DbSettings.UseDb)
//        {
//            _spellsDb = new DBLoader<Spell>("spells.db");

//            foreach (Spell s in _spellsDb.Items)
//            {
//                _spells.Add(s);
//            }
//        }

//        _spells.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_Spells_CollectionChanged);
//        _spellDictionary = new Dictionary<string, ObservableCollection<Spell>>(new InsensitiveEqualityCompararer());

//        _schools = new SortedDictionary<string, string>();
//        _subschools = new SortedSet<string>();
//        _descriptors = new SortedSet<string>();
//        _castingTimeOptions = new SortedSet<string>();
//        _rangeOptions = new SortedSet<string>();
//        _areaOptions = new SortedSet<string>();
//        _targetsOptions = new SortedSet<string>();
//        _durationOptions = new SortedSet<string>();
//        _savingThrowOptions = new SortedSet<string>();
//        _spellResistanceOptions = new SortedSet<string>();
//        foreach (Spell s in _spells)
//        {
//            string r = StringCapitalizer.Capitalize(s.School);
//            _schools[r] = r;
//            AddSpellDictionaryItem(s);

//            //add subschool
//            if (s.Subschool.NotNullString())
//            {
//                foreach (string subschool in s.Subschool.Split(new char[] { ',', ';' }))
//                {
//                    _subschools.Add(subschool.Trim());
//                }
//            }

//            if (s.Descriptor.NotNullString())
//            {
//                //add descriptions
//                foreach (string desc in s.Descriptor.Split(new char[] { ',', ';' }))
//                {
//                    _descriptors.Add(desc.Trim().TrimEnd(new char[] { 'U', 'M' }));
//                }
//            }



//        }

//        List<string> doubles = new List<string>(_subschools.Where(a => Regex.Match(a, "[a-zA-Z] or [a-zA-Z]").Success));
//        foreach (string dub in doubles)
//        {
//            _subschools.Remove(dub);
//        }

//        _descriptors.RemoveWhere(a => a.StartsWith("see text"));
//        List<string> ors = new List<string>(_descriptors.Where(a => a.StartsWith("or ")));
//        foreach (string orstring in ors)
//        {
//            _descriptors.Remove(orstring);
//            _descriptors.Add(orstring.Substring(3));
//        }
//        doubles = new List<string>(_descriptors.Where(a => Regex.Match(a, "[a-zA-Z] or [a-zA-Z]").Success));
//        foreach (string dub in doubles)
//        {
//            _descriptors.Remove(dub);
//        }

//        MakeOptions();


//    }

//    private static void MakeOptions()
//    {

//        //Casting Time
//        _castingTimeOptions.Add("1 hour");
//        _castingTimeOptions.Add("1 immediate action");
//        _castingTimeOptions.Add("1 minute");
//        _castingTimeOptions.Add("1 minute per page");
//        _castingTimeOptions.Add("1 minute/lb. created");
//        _castingTimeOptions.Add("1 round");
//        _castingTimeOptions.Add("1 round; see text");
//        _castingTimeOptions.Add("1 standard action");
//        _castingTimeOptions.Add("1 standard action or see text");
//        _castingTimeOptions.Add("10 minutes");
//        _castingTimeOptions.Add("10 minutes; see text");
//        _castingTimeOptions.Add("12 hours");
//        _castingTimeOptions.Add("2 rounds");
//        _castingTimeOptions.Add("24 hours");
//        _castingTimeOptions.Add("3 full rounds");
//        _castingTimeOptions.Add("3 rounds");
//        _castingTimeOptions.Add("30 minutes");
//        _castingTimeOptions.Add("6 rounds");
//        _castingTimeOptions.Add("at least 10 minutes; see text");
//        _castingTimeOptions.Add("see text");

//        //Range
//        _rangeOptions.Add("0 ft.");
//        _rangeOptions.Add("0 ft.; see text");
//        _rangeOptions.Add("1 mile");
//        _rangeOptions.Add("1 mile/level");
//        _rangeOptions.Add("10 ft.");
//        _rangeOptions.Add("120 ft.");
//        _rangeOptions.Add("15 ft.");
//        _rangeOptions.Add("2 miles");
//        _rangeOptions.Add("20 ft.");
//        _rangeOptions.Add("30 ft.");
//        _rangeOptions.Add("40 ft.");
//        _rangeOptions.Add("40 ft./level");
//        _rangeOptions.Add("5 miles");
//        _rangeOptions.Add("50 ft.");
//        _rangeOptions.Add("60 ft.");
//        _rangeOptions.Add("anywhere within the area to be warded");
//        _rangeOptions.Add("Area 10-ft.-radius emanation around the creature");
//        _rangeOptions.Add("close (25 ft. + 5 ft./2 levels)");
//        _rangeOptions.Add("close (25 ft. + 5 ft./2 levels) or see text");
//        _rangeOptions.Add("close (25 ft. + 5 ft./2 levels)/100 ft.; see text");
//        _rangeOptions.Add("close (25 ft. + 5 ft./2 levels); see text");
//        _rangeOptions.Add("long (400 ft. + 40 ft./level)");
//        _rangeOptions.Add("medium (100 ft. + 10 ft. level)");
//        _rangeOptions.Add("medium (100 ft. + 10 ft./level)");
//        _rangeOptions.Add("personal");
//        _rangeOptions.Add("personal and touch");
//        _rangeOptions.Add("personal or close (25 ft. + 5 ft./2 levels)");
//        _rangeOptions.Add("personal or touch");
//        _rangeOptions.Add("see text");
//        _rangeOptions.Add("touch");
//        _rangeOptions.Add("touch; see text");
//        _rangeOptions.Add("unlimited");
//        _rangeOptions.Add("up to 10 ft./level");

//        //Area
//        _areaOptions.Add("10-ft. square/level; see text");
//        _areaOptions.Add("10-ft.-radius emanation centered on you");
//        _areaOptions.Add("10-ft.-radius emanation from touched creature");
//        _areaOptions.Add("10-ft.-radius emanation, centered on you");
//        _areaOptions.Add("10-ft.-radius spherical emanation, centered on you");
//        _areaOptions.Add("10-ft.-radius spread");
//        _areaOptions.Add("120-ft. line");
//        _areaOptions.Add("20-ft.-radius burst");
//        _areaOptions.Add("20-ft.-radius emanation");
//        _areaOptions.Add("20-ft.-radius emanation centered on a creature, object, or point in space");
//        _areaOptions.Add("20-ft.-radius emanation centered on a point in space");
//        _areaOptions.Add("20-ft.-radius spread");
//        _areaOptions.Add("2-mile-radius circle, centered on you; see text");
//        _areaOptions.Add("30-ft. cube/level (S)");
//        _areaOptions.Add("40 ft./level radius cylinder 40 ft. high");
//        _areaOptions.Add("40-ft. radius emanating from the touched point");
//        _areaOptions.Add("40-ft.-radius emanation");
//        _areaOptions.Add("40-ft.-radius emanation centered on you");
//        _areaOptions.Add("50-ft.-radius burst, centered on you");
//        _areaOptions.Add("5-ft.-radius emanation centered on you");
//        _areaOptions.Add("5-ft.-radius spread; or one solid object or one crystalline creature");
//        _areaOptions.Add("60-ft. cube/level (S)");
//        _areaOptions.Add("60-ft. line from you");
//        _areaOptions.Add("60-ft. line-shaped emanation from you");
//        _areaOptions.Add("80-ft.-radius burst");
//        _areaOptions.Add("80-ft.-radius spread (S)");
//        _areaOptions.Add("all allies and foes within a 40-ft.-radius burst centered on you");
//        _areaOptions.Add("all magical effects and magic items within a 40-ft.-radius burst, or one magic item (see text)");
//        _areaOptions.Add("all metal objects within a 40-ft.-radius burst");
//        _areaOptions.Add("barred cage (20-ft. cube) or windowless cell (10-ft. cube)");
//        _areaOptions.Add("circle, centered on you, with a radius of 400 ft. + 40 ft./level");
//        _areaOptions.Add("cloud spreads in 20-ft. radius, 20 ft. high");
//        _areaOptions.Add("cone-shaped burst");
//        _areaOptions.Add("cone-shaped emanation");
//        _areaOptions.Add("creatures and objects within 10-ft.-radius spread");
//        _areaOptions.Add("creatures and objects within a 5-ft.-radius burst");
//        _areaOptions.Add("creatures in a 20-ft.-radius spread");
//        _areaOptions.Add("creatures within a 20-ft.-radius spread");
//        _areaOptions.Add("cylinder (10-ft. radius, 40-ft. high)");
//        _areaOptions.Add("cylinder (20-ft. radius, 40 ft. high)");
//        _areaOptions.Add("cylinder (40-ft. radius, 20 ft. high)");
//        _areaOptions.Add("dirt in an area up to 750 ft. square and up to 10 ft. deep (S)");
//        _areaOptions.Add("four 40-ft.-radius spreads, see text");
//        _areaOptions.Add("line from your hand");
//        _areaOptions.Add("living creatures within a 10-ft.-radius burst");
//        _areaOptions.Add("nonchaotic creatures in a 40-ft.-radius spread centered on you");
//        _areaOptions.Add("nonevil creatures in a 40-ft.-radius spread centered on you");
//        _areaOptions.Add("nongood creatures in a 40-ft.-radius spread centered on you");
//        _areaOptions.Add("nonlawful creatures in a 40-ft.-radius spread centered on you");
//        _areaOptions.Add("nonlawful creatures within a burst that fills a 30-ft. cube");
//        _areaOptions.Add("one 20-ft. cube/level (S)");
//        _areaOptions.Add("one 20-ft. square/level");
//        _areaOptions.Add("one 30-ft. cube/level (S)");
//        _areaOptions.Add("one or more living creatures within a 10-ft.-radius burst");
//        _areaOptions.Add("or Target one 20-ft. cube/level  or one fire-based magic item (S)");
//        _areaOptions.Add("plants in a 40-ft.-radius spread");
//        _areaOptions.Add("see text");
//        _areaOptions.Add("several living creatures within a 40-ft.-radius burst");
//        _areaOptions.Add("several living creatures, no two of which may be more than 30 ft. apart");
//        _areaOptions.Add("several undead creatures within a 40-ft.-radius burst");
//        _areaOptions.Add("Target object touched");
//        _areaOptions.Add("Target or object touched or up to 5 sq. ft./level");
//        _areaOptions.Add("Target or one creature, one object, or a 5-ft. cube");
//        _areaOptions.Add("Target or see text");
//        _areaOptions.Add("Target, Effect, or  see text");
//        _areaOptions.Add("The caster and all allies within a 50-ft. burst, centered on the caster");
//        _areaOptions.Add("two 10-ft. cubes per level (S)");
//        _areaOptions.Add("up to 10-ft.-radius/level emanation centered on you");
//        _areaOptions.Add("up to 200 sq. ft./level (S)");
//        _areaOptions.Add("up to one 10-ft. cube/level (S)");
//        _areaOptions.Add("up to two 10-ft. cubes/level (S)");
//        _areaOptions.Add("water in a volume of 10 ft./level by 10 ft./level by 2 ft./level (S)");

//        //Targets
//        _targetsOptions.Add("creature or object touchedobject touched");
//        _targetsOptions.Add("creature touched");
//        _targetsOptions.Add("one animal");
//        _targetsOptions.Add("one creature");
//        _targetsOptions.Add("one creature or object");
//        _targetsOptions.Add("one creature or object/level, no two of which can be more than 30 ft. apart");
//        _targetsOptions.Add("one creature/level");
//        _targetsOptions.Add("one creature/level in a 20-ft.-radius burst centered on you");
//        _targetsOptions.Add("one creature/level touched");
//        _targetsOptions.Add("one creature/level, no two of which can be more than 30 ft. apart");
//        _targetsOptions.Add("one humanoid creature");
//        _targetsOptions.Add("one humanoid creature/level, no two of which can be more than 30 ft. apart");
//        _targetsOptions.Add("one living creature");
//        _targetsOptions.Add("one living creature/level, no two of which may be more than 30 ft. apart");
//        _targetsOptions.Add("one melee weapon");
//        _targetsOptions.Add("one undead creature");
//        _targetsOptions.Add("see text");
//        _targetsOptions.Add("up to one creature per level, all within 30 ft. of each other");
//        _targetsOptions.Add("up to one touched creature/level");
//        _targetsOptions.Add("weapon touched");
//        _targetsOptions.Add("you");
//        _targetsOptions.Add("you or creature touched");

//        //Duration
//        _durationOptions.Add("1 round/level");
//        _durationOptions.Add("1 min/level");
//        _durationOptions.Add("1 hour/level");
//        _durationOptions.Add("1 day/level");
//        _durationOptions.Add("concentration");
//        _durationOptions.Add("instantaneous");
//        _durationOptions.Add("permanent");
//        _durationOptions.Add("see text");

//        //Saving Throw
//        _savingThrowOptions.Add("Fortitude half");
//        _savingThrowOptions.Add("Fortitude half; see text");
//        _savingThrowOptions.Add("Fortitude negates");
//        _savingThrowOptions.Add("Fortitude negates (harmless)");
//        _savingThrowOptions.Add("Fortitude partial");
//        _savingThrowOptions.Add("Fortitude partial; see text");
//        _savingThrowOptions.Add("Reflex half");
//        _savingThrowOptions.Add("Reflex half; see text");
//        _savingThrowOptions.Add("Reflex negates");
//        _savingThrowOptions.Add("Reflex negates (harmless)");
//        _savingThrowOptions.Add("Reflex partial");
//        _savingThrowOptions.Add("Reflex partial; see text");
//        _savingThrowOptions.Add("Will disbelief");
//        _savingThrowOptions.Add("Will half");
//        _savingThrowOptions.Add("Will half; see text");
//        _savingThrowOptions.Add("Will negates");
//        _savingThrowOptions.Add("Will negates (harmless)");
//        _savingThrowOptions.Add("Will partial");
//        _savingThrowOptions.Add("Will partial; see text");
//        _savingThrowOptions.Add("none");
//        _savingThrowOptions.Add("none; see text");
//        _savingThrowOptions.Add("see text");

//        //SpellResistance
//        _spellResistanceOptions.Add("no");
//        _spellResistanceOptions.Add("see text");
//        _spellResistanceOptions.Add("yes");
//        _spellResistanceOptions.Add("yes (harmless)");
//    }


//    public delegate void SpellsDbUpdatedDelegate(ICollection<Spell> added, ICollection<Spell> updated, ICollection<Spell> removed);
//    public static SpellsDbUpdatedDelegate SpellsDbUpdated;





//    static void Spell_PropertyChanged(object sender, PropertyChangedEventArgs e)
//    {
//        if (e.PropertyName == "name")
//        {
//            Spell s = (Spell)sender;
//            RemoveSpellDictionaryItem(s, s.Oldname);
//            AddSpellDictionaryItem(s);
//        }
//    }

//    private static void RemoveSpellDictionaryItem(Spell s, string oldname)
//    {
//        string checkname = oldname == null ? s.Name : oldname;

//        if (_spellDictionary.ContainsKey(checkname))
//        {
//            ObservableCollection<Spell> list = _spellDictionary[checkname];
//            list.Remove(s);
//            if (list.Count == 0)
//            {
//                _spellDictionary.Remove(checkname);
//            }
//            s.PropertyChanged -= new PropertyChangedEventHandler(Spell_PropertyChanged);

//        }
//    }

//    private static void AddSpellDictionaryItem(Spell s)
//    {
//        s.PropertyChanged += new PropertyChangedEventHandler(Spell_PropertyChanged);

//        if (_spellDictionary.ContainsKey(s.Name))
//        {

//            ObservableCollection<Spell> list = _spellDictionary[s.Name];
//            if (!list.Contains(s))
//            {
//                list.Add(s);
//            }
//        }
//        else
//        {
//            _spellDictionary[s.Name] = new ObservableCollection<Spell>() { s };
//        }
//    }

//    static void _Spells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//    {
//        if (e.OldItems != null)
//        {
//            foreach (Spell s in e.OldItems)
//            {
//                RemoveSpellDictionaryItem(s, null);
//            }
//        }
//        if (e.NewItems != null)
//        {
//            foreach (Spell s in e.NewItems)
//            {
//                AddSpellDictionaryItem(s);
//            }

//        }

//    }

//    public static Spell ByName(string name)
//    {
//        if (_spellDictionary.ContainsKey(name))
//        {
//            return _spellDictionary[name][0];
//        }
//        else if (_spellDictionary.ContainsKey(CmStringUtilities.DecommaText(name)))
//        {
//            return _spellDictionary[CmStringUtilities.DecommaText(name)][0];
//        }
//        else
//        {
//            name = StandarizeSpellName(name);

//            if (_spellDictionary.ContainsKey(name))
//            {
//                return _spellDictionary[name][0];
//            }
//        }

//        return null;
//    }

//    public static string StandarizeSpellName(string name)
//    {
//        name = StripMeta(name);

//        if (name.StartsWith("greater ", StringComparison.CurrentCultureIgnoreCase))
//        {
//            name = name.Substring("greater ".Length) + ", Greater";
//        }

//        if (name.StartsWith("lesser ", StringComparison.CurrentCultureIgnoreCase))
//        {
//            name = name.Substring("lesser ".Length) + ", Lesser";
//        }

//        if (name.StartsWith("mass ", StringComparison.CurrentCultureIgnoreCase))
//        {
//            name = name.Substring("mass ".Length) + ", Mass";
//        }

//        return name;
//    }

//    private static List<string> _metaAdj = new List<string> { "quickened", "selective", "maximized", "empowered", "enlarged", "extended", "heightened", "silenced", "stilled", "widened" };
//    private static string StripMeta(string name)
//    {
//        foreach (string meta in _metaAdj)
//        {
//            if (name.IndexOf(meta, StringComparison.CurrentCultureIgnoreCase) >= 0)
//            {
//                name = Regex.Replace(name, meta, string.Empty, RegexOptions.IgnoreCase);
//            }
//        }
//        return name.Trim();
//    }

//    public static ObservableCollection<Spell> Spells
//    {
//        get
//        {
//            if (_spells == null)
//            {
//                LoadSpells();
//            }
//            return _spells;
//        }
//    }


//    public static Spell ByDetailsId(int id)
//    {
//        if (_spellsByDetailsId == null)
//        {
//            LoadSpells();
//        }
//        Spell s;
//        _spellsByDetailsId.TryGetValue(id, out s);
//        return s;
//    }

//    public static Spell ByDbLoaderId(int id)
//    {
//        return DbSpells.FirstOrDefault(s => ((BaseDbClass)s).DbLoaderId == id);
//    }

//    public static Spell ById(bool custom, int id)
//    {
//        if (custom)
//        {
//            return ByDbLoaderId(id);
//        }
//        else
//        {
//            return ByDetailsId(id);
//        }
//    }

//    public static bool TryById(bool custom, int id, out Spell s)
//    {
//        s = ById(custom, id);
//        return s != null;
//    }


//    public static void AddCustomSpell(Spell s)
//    {
//        if (_spells == null)
//        {
//            LoadSpells();
//        }
//        _spellsDb.AddItem(s);
//        Spells.Add(s);
//        SpellsDbUpdated?.Invoke(new List<Spell>() { s }, new List<Spell>(), new List<Spell>());
//    }

//    public static void RemoveCustomSpell(Spell s)
//    {

//        _spellsDb.DeleteItem(s);
//        Spells.Remove(s);
//        SpellsDbUpdated?.Invoke(new List<Spell>(), new List<Spell>(), new List<Spell>() { s });
//    }

//    public static void UpdateCustomSpell(Spell s, bool updateData = false)
//    {
//        _spellsDb.UpdateItem(s);
//        if (updateData)
//        {
//            Spell old = Spells.FirstOrDefault((c) => ((BaseDbClass)c).DbLoaderId == ((BaseDbClass)s).DbLoaderId);
//            old.CopyFrom(s);
//        }
//        SpellsDbUpdated?.Invoke(new List<Spell>(), new List<Spell>() { s }, new List<Spell>());
//    }

//    public static IEnumerable<Spell> DbSpells
//    {
//        get
//        {
//            if (_spells == null)
//            {
//                LoadSpells();
//            }
//            return _spellsDb.Items;
//        }
//    }


//    public static ICollection<string> Schools
//    {
//        get
//        {
//            if (_schools == null)
//            {
//                LoadSpells();
//            }
//            return _schools.Values;
//        }
//    }
//    public static ICollection<string> Subschools
//    {
//        get
//        {
//            if (_subschools == null)
//            {
//                LoadSpells();
//            }
//            return _subschools;
//        }
//    }
//    public static ICollection<string> Descriptors
//    {
//        get
//        {
//            if (_descriptors == null)
//            {
//                LoadSpells();
//            }
//            return _descriptors;
//        }
//    }


//    public static ICollection<string> CastingTimeOptions => _castingTimeOptions;

//    public static ICollection<string> RangeOptions => _rangeOptions;

//    public static ICollection<string> AreaOptions => _areaOptions;

//    public static ICollection<string> TargetsOptions => _targetsOptions;

//    public static ICollection<string> DurationOptions => _durationOptions;

//    public static ICollection<string> SavingThrowOptions => _savingThrowOptions;

//    public static ICollection<string> SpellResistanceOptions => _savingThrowOptions;


//}