/*
 *  CampaignEvent.cs
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
    public class CampaignEvent : INotifyPropertyChanged, IDbLoadable
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int _dbLoaderId;
        private int _campaignId;
        private DateTime _start;
        private DateTime _lastStart;
        private DateTime _end;
        private bool _allDay;
        private string _title;
        private string _details;


        public CampaignEvent()
        {

        }

        public CampaignEvent(CampaignEvent old)
        {
            _start = old._start;
            _end = old.End;
            _allDay = old._allDay;
            _title = old._title;
            _details = old._details;

        }

        public object Clone()
        {
            return new CampaignEvent(this);
        }

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _lastStart = _start;
                    _start = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Start")); }
                }
            }
        }

        [XmlIgnore]
        public DateTime LastStart => _lastStart;

        public DateTime End
        {
            get => _end;
            set
            {
                if (_end != value)
                {
                    _end = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("End")); }
                }
            }
        }
        public bool AllDay
        {
            get => _allDay;
            set
            {
                if (_allDay != value)
                {
                    _allDay = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AllDay")); }
                }
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Title")); }
                }
            }
        }
        public string Details
        {
            get => _details;
            set
            {
                if (_details != value)
                {
                    _details = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Details")); }
                }
            }
        }


        public int DbLoaderId
        {
            get => _dbLoaderId;
            set
            {
                if (_dbLoaderId != value)
                {
                    _dbLoaderId = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DBLoaderID")); }
                }
            }
        }

        public int CampaignId
        {
            get => _campaignId;
            set
            {
                if (_campaignId != value)
                {
                    _campaignId = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CampaignID")); }
                }
            }
        }
    }

}
