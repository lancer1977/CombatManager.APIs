namespace CombatManager
{
    public class ParsedFeat : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _choice;
        private string _featSource;

        public ParsedFeat()
        {

        }

        public ParsedFeat(string details)
        {
            ParseFeat(details);
        }

        public void ParseFeat(string details)
        {
            Regex reg = new Regex("(?<name>.+?) \\((?<choice>.+?)\\)");


            Match m = reg.Match(details);

            if (m.Success)
            {

                this.Name = m.Groups["name"].Value;
                this.Choice = m.Groups["choice"].Value;
            }

            else
            {
                Name = details;
            }
            
            _featSource = details;


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
        public string Choice
        {
            get => _choice;
            set
            {
                if (_choice != value)
                {
                    _choice = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Choice")); }
                }
            }
        }
        
        public string FeatSource
        {
            
            get => _featSource;
            set
            {
                if (_featSource != value)
                {
                    _featSource = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("FeatSource")); }
                }
            }
        }

        [XmlIgnore]
        public string Text
        {
            get
            {
                string text = _name;

                if (_choice != null && _choice.Length > 0)
                {
                    text += " (" + _choice + ")";  
                }
                return text;
            }
        }

        public override string ToString()
        {
            return Text;
        }




    }
}

