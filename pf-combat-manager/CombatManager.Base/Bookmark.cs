namespace CombatManager
{
    public class Bookmark : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _type;
        private string _name;
        private string _id;
        private string _data;

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Type")); }
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
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
                }
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ID")); }
                }
            }
        }
        public string Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Data")); }
                }
            }
        }
        
    }
}
