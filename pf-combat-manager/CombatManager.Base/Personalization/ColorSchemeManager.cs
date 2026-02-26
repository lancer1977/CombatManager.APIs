namespace CombatManager.Personalization
{
    public class ColorSchemeManager
    {
        List<ColorScheme> _colorSchemes;
        List<ColorScheme> _defaultSchemes;

        Dictionary<int, ColorScheme> _schemeDictionary;

        private static ColorSchemeManager _manager;
        public static ColorSchemeManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new ColorSchemeManager();
                }
                return _manager;
            }
        }

        private ColorSchemeManager()
        {
            _defaultSchemes = XmlListLoader<ColorScheme>.Load("DefaultColorSchemes.xml");
            _colorSchemes = new List<ColorScheme>();
            _colorSchemes.AddRange(_defaultSchemes);

            _schemeDictionary = new Dictionary<int, ColorScheme>();
            foreach (var v in _colorSchemes)
            {
                _schemeDictionary[v.Id] = v;
            }

        }

        public ColorScheme SchemeById(int id)
        {
            ColorScheme scheme;
            if (!_schemeDictionary.TryGetValue(id, out scheme))
            {
                return _schemeDictionary[0];
            }
            return scheme;
        }

        public List<ColorScheme> SortedSchemes
        {
            get
            {
                List<ColorScheme> list = new List<ColorScheme>();
                list.Add(_schemeDictionary[0]);
                list.AddRange(from v in _schemeDictionary.Values where v.Id != 0 orderby v.Name select v);
                return list;
                    
            }
        }

        public List<ColorScheme> ColorSchemes => _colorSchemes;
    }
}
