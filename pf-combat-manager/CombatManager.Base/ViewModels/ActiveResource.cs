namespace CombatManager.ViewModels
{
    public class ActiveResource : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _uses;
        private string _type;
        private int _min;
        private int _max;
        private int _current;

        public ActiveResource()
        {
        }

        public ActiveResource(ActiveResource resource)
        {
            _name = resource.Name;
            _uses = resource.Uses;
            _type = resource._type;
            _min = resource._min;
            _max = resource._max;
            _current = resource.Current;
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
        public string Uses
        {
            get => _uses;
            set
            {
                if (_uses != value)
                {
                    _uses = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Uses")); }
                }
            }
        }
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
        public int Min
        {
            get => _min;
            set
            {
                if (_min != value)
                {
                    _min = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Min")); }
                }
            }
        }
        public int Max
        {
            get => _max;
            set
            {
                if (_max != value)
                {
                    _max = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Max")); }
                }
            }
        }
        public int Current
        {
            get => _current;
            set
            {
                if (_current != value)
                {
                    _current = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Current")); }
                }
            }
        }

    }
}
