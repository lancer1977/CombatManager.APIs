namespace CombatManager
{
    public class BookmarkList
    {
        static BookmarkList _list;

        ObservableCollection<Bookmark> _bookmarks = new ObservableCollection<Bookmark>();


        public static BookmarkList List
        {
            get
            {
                if (_list == null)
                {
                    _list = XmlLoader<BookmarkList>.Load("BookmarkList.xml", true);

                }
                if (_list == null)
                {
                    _list = new BookmarkList();
                }
                return _list;
            }
        }

        private static void SaveList()
        {
            XmlLoader<BookmarkList>.Save(_list, "BookmarkList.xml", true);
        }

        public bool AddFeat(Feat feat)
        {
            Bookmark b = new Bookmark();
            b.Type = "feat";
            b.Name = feat.Name;
            b.Id = feat.Name;

            return AddBookmark(b);

        }

        public void AddMonster(Monster monster)
        {
            //Bookmark b = new Bo
        }

        public void AddSpell(Spell spell)
        {

        }

        public void AddRule(Rule rule)
        {

        }

        public void AddTreasure(Treasure treasure)
        {

        }

        private bool AddBookmark(Bookmark b)
        {
            if (_bookmarks.FirstOrDefault((a) => (a.Name == b.Name && a.Type == b.Type && a.Id == b.Id  && a.Data == b.Data)) == null)
            {
                _bookmarks.Add(b);

                SaveList();
                return true;
            }
            else
            {
                return false;
            }
        }

        public ObservableCollection<Bookmark> Bookmarks => _list._bookmarks;
    }
}
