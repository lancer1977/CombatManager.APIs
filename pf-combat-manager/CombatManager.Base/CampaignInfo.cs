/*
 *  CampaignInfo.cs
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
    public class CampaignInfo : INotifyPropertyChanged
    {
        private int _campaignId;
        private DateTime _currentDate;
        private DateTime _displayDate;
        private DateTime _selectedDate;

        private Dictionary<DateTime, ObservableCollection<CampaignEvent>> _events;

#if !MONO
    //private SQL_Lite sql;
#else
		
        //private static SqliteConnection eventDB;
#endif

        private static void PrepareDb()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CampaignInfo()
        {
            DateTime now = DateTime.Now;
            _currentDate = new DateTime(now.Year + 2700, 1, 1, 12, 0, 0);
            _displayDate = new DateTime(now.Year + 2700, 1, 1, 12, 0, 0);
            _selectedDate = new DateTime(now.Year + 2700, 1, 1, 12, 0, 0);
            _events = new Dictionary<DateTime, ObservableCollection<CampaignEvent>>();
        }


        public void AddEvent(CampaignEvent e)
        {
            ObservableCollection<CampaignEvent> list = new ObservableCollection<CampaignEvent>();
            if (_events.ContainsKey(e.Start.Date))
            {
                list = _events[e.Start.Date];
            }
            e.PropertyChanged += EventPropertyChanged;
            System.Diagnostics.Debug.Assert(!list.Contains(e));
            list.Add(e);
            _events[e.Start.Date] = list;

            PropertyChanged(this, new PropertyChangedEventArgs("Events"));
        }

        void EventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Start")
            {                
                CampaignEvent ev = (CampaignEvent)sender;

                ObservableCollection<CampaignEvent> list = _events[ev.LastStart.Date];
                list.Remove(ev);
                if (_events.ContainsKey(ev.Start.Date))
                {
                    list = _events[ev.Start.Date];
                }
                else
                {
                    list = new ObservableCollection<CampaignEvent>();
                    _events[ev.Start.Date] = list;
                }

                list.Add(ev);
                PropertyChanged(this, new PropertyChangedEventArgs("Events"));
            }
        }

        public void RemoveEvent(CampaignEvent e)
        {
            ObservableCollection<CampaignEvent> list = _events[e.Start.Date];
            list.Remove(e);
            PropertyChanged(this, new PropertyChangedEventArgs("Events"));
        }




        public DateTime CurrentDate 
        {
            get => _currentDate;
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CurrentDate")); }
                }
            }
        }


        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SelectedDate")); }
                }
            }
        }


        public DateTime DisplayDate
        {
            get => _displayDate;
            set
            {
                if (_displayDate != value)
                {
                    _displayDate = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DisplayDate")); }
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

        public IDictionary<DateTime, ObservableCollection<CampaignEvent>> Events => _events;

        public List<CampaignEvent> EventsForDate(DateTime date)
        {
            DateTime useDate = date.Date;

            List<CampaignEvent> list = new List<CampaignEvent>();

            if (_events.ContainsKey(date))
            {
                list.AddRange(_events[date]);
                list.Sort((a, b) => a.Start.CompareTo(b.Start));
            }

            return list;

               
        }

    }
}
