/*
 *  Feat.cs
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

namespace CombatManager
{
    public class Feat : BaseDbClass
    {


        private string _name;
        private string _altName;
        private string _type;
        private string _prerequistites;
        private string _summary;
        private string _source;
        private string _system;
        private string _license;
        private string _url;
        private string _detail; 
        private string _benefit;
        private string _normal;
        private string _special;
        private SortedDictionary<string, string> _types;

        private bool _detailParsed;




        public Feat()
        {
            _types = new SortedDictionary<string, string>();
            _detailParsed = false;
        }

        public Feat(Feat f)
        {
            CopyFrom(f);
        }

        public object Clone()
        {
            return new Feat(this);
        }

        public void CopyFrom(Feat f)
        {
            _name = f._name;
            _altName = f._altName;
            _type = f._type;
            _prerequistites = f._prerequistites;
            _summary = f._summary;
            _source = f._source;
            _system = f._system;
            _license = f._license;
            _url = f._url;
            _detail = f._detail; 
            _benefit = f._benefit;
            _normal = f._normal;
            _special = f._special;

            if (f._types != null)
            {
                _types = new SortedDictionary<string, string>();

                foreach (string s in f._types.Values)
                {
                    _types[s] = s;
                }
            }
            else
            {
                _types = null;
            }
            
            _detailParsed = f._detailParsed;
            DbLoaderId = f.DbLoaderId;
            _detailsId = f.DetailsId;
        }

        protected override void SelfPropertyChanged(string name)
        {
            if (name == "DetailsID")
            {
                Notify("Id");
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
                    Notify();
                }
            }
        }
        public string AltName
        {
            get => _altName;
            set
            {
                if (_altName != value)
                {
                    _altName = value;
                    Notify();
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

                    _types.Clear();
                    foreach (string str in _type.Split(new char[]{','}))
                    {
                        _types[str.Trim()] = str.Trim();
                    }

                    Notify();
                }
            }
        }
        public string Prerequistites
        {
            get => _prerequistites;
            set
            {
                if (_prerequistites != value)
                {
                    _prerequistites = value;
                    Notify();
                }
            }
        }
        public string Summary
        {
            get => _summary;
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    Notify();
                }
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
                    Notify();
                }
            }
        }
        public string System
        {
            get => _system;
            set
            {
                if (_system != value)
                {
                    _system = value;
                    Notify();
                }
            }
        }
        public string License
        {
            get => _license;
            set
            {
                if (_license != value)
                {
                    _license = value;
                    Notify();
                }
            }
        }

        public string Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    Notify("URL");
                }
            }
        }

        public string Detail
        {
            get => _detail;
            set
            {
                if (_detail != value)
                {
                    _detail = value;
                    Notify();
                }
            }
        }
        public string Benefit
        {
            get 
            {
                if (!DetailParsed)
                {
                    ParseDetail();
                }
                return _benefit; 
                
            }
            set
            {
                if (_benefit != value)
                {
                    _benefit = value;
                    Notify();
                }
            }
        }
        public string Normal
        {
            get
            {
                if (!DetailParsed)
                {
                    ParseDetail();
                } 
                return _normal;
            }
            set
            {
                if (_normal != value)
                {
                    _normal = value;
                    Notify();
                }
            }
        }
        public string Special
        {
            get
            {
                if (!DetailParsed)
                {
                    ParseDetail();
                }
                return _special;
            }
            set
            {
                if (_special != value)
                {
                    _special = value;
                    Notify();
                }
            }
        }


        public int Id
        {
            get => DetailsId;
            set
            {
                if (DetailsId != value)
                {
                    _detailsId = value;
                    Notify("DetailsId");
                    Notify();

                }
            }
        }


        [XmlIgnore]
        public ICollection<string> Types => _types.Values;

        public bool DetailParsed
        {
            get => _detailParsed;
            set => _detailParsed = value;
        }

        public void ParseDetail()
        {
            if (!DetailParsed)
            {
                if (_detail != null)
                {
                    Match matchStart;

                    string detailCheck = _detail;



                    Regex regexBenefit = CreateHeaderRegex("Benefit");
                    matchStart = (regexBenefit.Match(detailCheck));
                    if (matchStart.Success)
                    {
                        Benefit = FinishItem(matchStart, detailCheck);
                    }

                    Regex regexNormal = CreateHeaderRegex("Normal");
                    matchStart = (regexNormal.Match(detailCheck));
                    if (matchStart.Success)
                    {
                        Normal = FinishItem(matchStart, detailCheck);
                    }

                    Regex regexSpecial = CreateHeaderRegex("Special");
                    matchStart = (regexSpecial.Match(detailCheck));
                    if (matchStart.Success)
                    {
                        Special = FinishItem(matchStart, detailCheck);
                    }
                }
                DetailParsed = true;
            }
        }

        private Regex CreateHeaderRegex(string item)
        {
            string spanTag = "<span(.|\n|\t)*?>";

            return new Regex("(<b>|" + spanTag + ") *" + item + "s? *:? *(</b>|</span>|" + spanTag + ")+:?", RegexOptions.IgnoreCase);
        }

        private string FinishItem(Match matchStart, string detailCheck)
        {
            int start = matchStart.Index + matchStart.Length;

            Regex regexFont = new Regex("(</span>)?(</font> *(</div>|</p>)|</p> *</font>)");

            Regex regexReplace = new Regex("<[^<>]+>");


            Match matchEnd = regexFont.Match(_detail, start);
            if (matchEnd.Success)
            {
                return regexReplace.Replace(_detail.Substring(start, matchEnd.Index - start), "").Trim();
            }
            else
            {
                return null;
            }

        }


    }
}
