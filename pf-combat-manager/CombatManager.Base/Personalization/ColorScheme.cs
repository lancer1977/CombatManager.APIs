using System.Runtime.Serialization;

namespace CombatManager.Personalization
{
    public class ColorScheme : ICloneable, INotifyPropertyChanged
    {
        public static IReadOnlyList<string> Prefixes { get; } = new string[]
                { "PrimaryColor", "SecondaryColorA", "SecondaryColorB" };

        public static IReadOnlyList<string> Shades { get; } = new string[]
                { "Lighter", "Light", "Medium", "Dark", "Darker" };

        public static string GetColorName(int hue, int shade)
        {
            return Prefixes[hue] + Shades[shade];
        }

        public static string GetTextColorBaseName(bool fore)
        {
            return "ThemeText" + (fore ? "Foreground" : "Background");
        }

        public static string GetTextColorName(bool fore, bool dark)
        {
            return GetTextColorBaseName(fore) + ( dark ? "Dark" : "Light");
        }

        private List<List<string>> _colors;
        private string _name;

        private List<int> _textColors;

        private int _id;

        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public List<List<string>> Colors
        {
            get => _colors;
            set
            {
                if (_colors != value)
                {
                    _colors = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Colors"));
                }
            }
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        [DataMember]
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ID"));
                }
            }
        }

        [DataMember]
        public List<int> TextColors
        {
            get => _textColors;
            set
            {
                if (_textColors != value)
                {
                    _textColors = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextColors"));
                }
            }
        }

        public uint GetTextColor(bool fore, bool dark)
        {
            if (TextColors == null || TextColors.Count < 4)
            {
                return 0;
            }

            int index = TextColors[(dark ? 2 : 0) + (fore?0:1)];
            if (index == -1)
            {
                return 0;
            }
            else
            {
                return UInt32FromString(ColorFromIndex(index));
            }

        }

        public string ColorFromIndex(int index)
        {
            if (index == -1)
            {
                return null;
            }

            int list = index / 5;
            int level = index % 5;

            return Colors[list][index];

        }


        public ColorScheme()
        {
            /*colors = new List<List<string>>();

            for (int i = 0; i < 3; i++)
            {
                var sub = new List<string>();
                colors.Add(sub);
                for (int j = 0; j < 5; j++)
                {
                    sub.Add("FF000000");
                }
            }*/
        }

        public uint GetColorUInt32(int hue, int shade)
        {
            string color = Colors[hue][shade];
            return UInt32FromString(color);
        }

        private uint UInt32FromString (string color)
        {

            return uint.Parse(color, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        public void SetColorUInt32(int hue, int shade, uint value)
        {
            Colors[hue][shade] = value.ToString("X8");
        }



        public ColorScheme(ColorScheme old) 
        {
            _colors = new List<List<string>>();
            for (int i = 0; i < 3; i++)
            {
                var sub = new List<string>();
                _colors.Add(sub);
                for (int j = 0; j < 5; j++)
                {
                    sub.Add(old.Colors[i][j]);
                }
            }
            _textColors = new List<int>();
            _textColors.AddRange(old.TextColors);
        }



        public object Clone()
        {
            return new ColorScheme(this);
        }
    }

}
