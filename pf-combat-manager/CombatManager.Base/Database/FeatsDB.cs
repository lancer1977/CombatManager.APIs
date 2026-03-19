using System.Diagnostics;
using CombatManager.Interfaces;
using CombatManager.Utilities;

namespace CombatManager.Database;

/// <summary>
/// This acts as a bridge communicating with the DB, and exposing items to the app in quick short term memory.
/// </summary>
public   class FeatsRepository
{
    public FeatsRepository(ISQLService<Feat> featsDb)
    {
        _featsDb = featsDb;
    }
    private  Dictionary<string, Feat> _featMap;
    private  Dictionary<string, string> _altFeatMap;
    private  SortedDictionary<string, string> _types;
    private  ObservableCollection<Feat> _feats;

    private  bool _featsLoaded; 
    private  Dictionary<int, Feat> _featsByDetailsId;
    public ISQLService<Feat> _featsDb;

    public  void LoadFeats()
    {


        List<Feat> set = XmlListLoader<Feat>.Load("Feats.xml");

        _featMap = new Dictionary<string, Feat>();
        _altFeatMap = new Dictionary<string, string>();
        _types = new SortedDictionary<string, string>();
        _featsByDetailsId = new Dictionary<int, Feat>();


        foreach (Feat feat in set)
        {
            bool changed;
            string commaName = CmStringUtilities.DecommaText(feat.Name, out changed);

            if (changed)
            {
                feat.AltName = feat.Name;
                feat.Name = commaName;
                _altFeatMap.Add(feat.AltName, feat.Name);
            }

            _featMap[feat.Name] = feat;



            //add to types list
            foreach (string type in feat.Types)
            {
                _types[type] = type;
            }

            if (feat.Types.Count == 0)
            {
                Debug.WriteLine((string)feat.Name);
            }


            _featsByDetailsId[feat.Id] = feat;

            _feats = new ObservableCollection<Feat>();

        }
        foreach (var f in FeatMap.Values)
        {
            _feats.Add(f);
        }

        if (DbSettings.UseDb)
        { 

            foreach (Feat f in _featsDb.GetAll())
            {
                _feats.Add(f);

                if (!FeatMap.ContainsKey(f.Name))
                {
                    FeatMap[f.Name] = f;
                }
            }
        }

        _featsLoaded = true;

    }

    public  bool FeatsLoaded => _featsLoaded;


    public  Dictionary<string, Feat> FeatMap => _featMap;

    public  ObservableCollection<Feat> Feats
    {
        get
        {
            if (_feats == null)
            {
                LoadFeats();
            }
            return _feats;
        }
    }

    public  Feat ByDetailsId(int id)
    {
        if (_featsByDetailsId == null)
        {
            LoadFeats();
        }
        Feat s;
        _featsByDetailsId.TryGetValue(id, out s);
        return s;
    }

    public  Feat ByDbLoaderId(int id)
    {
        return DbFeats.FirstOrDefault(s => ((BaseDbClass)s).DbLoaderId == id);
    }

    public  Feat ById(bool custom, int id)
    {
        if (custom)
        {
            return ByDbLoaderId(id);
        }
        else
        {
            return ByDetailsId(id);
        }
    }

    public  bool TryById(bool custom, int id, out Feat s)
    {
        s = ById(custom, id);
        return s != null;
    }

    public  Dictionary<string, string> AltFeatMap
    {
        get
        {
            if (_altFeatMap == null)
            {
                LoadFeats();
            }
            return _altFeatMap;
        }
    }

    public  IEnumerable<string> FeatTypes => _types.Values;
    public  void AddCustomFeat(Feat f)
    {
        if (_feats == null)
        {
            LoadFeats();
        }

        _featsDb.AddItem(f);
        if (!_featMap.ContainsKey(f.Name))
        {
            _featMap[f.Name] = f;
        }
        _feats.Add(f);
    }

    public  void RemoveCustomFeat(Feat f)
    {

        _featsDb.DeleteItem(f);
        _feats.Remove(f);

        Feat old;
        if (FeatMap.TryGetValue(f.Name, out old))
        {
            if (old == f)
            {
                FeatMap.Remove(f.Name);
                Feat replace = _feats.FirstOrDefault(a => a.Name == f.Name);

                if (replace != null)
                {
                    FeatMap[replace.Name] = replace;
                }
            }
        }

    }

    public  void UpdateCustomFeat(Feat f)
    {
        _featsDb.UpdateItem(f);
    }

    public  IEnumerable<Feat> DbFeats
    {
        get
        {
            if (_feats == null)
            {
                LoadFeats();
            }
            return _featsDb.GetAll();
        }
    }

}