/*
 *  FeatChangeControl.xaml.cs
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

 using System;
using System.Collections.Generic;
 using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
 using System.Windows.Input;
 using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CombatManager
{
	/// <summary>
	/// Interaction logic for FeatChangeControl.xaml
	/// </summary>
	public partial class FeatChangeControl : UserControl
	{
		Monster _Monster;
        ListCollectionView _FeatsView;
        ListCollectionView _CurrentFeatsView;
        ObservableCollection<ParsedFeat> _CurrentFeats;
		
		public FeatChangeControl()
		{
			this.InitializeComponent();
            //RemoveButton.IsEnabled = false;

            if (Feat.FeatMap != null)
            {

                _FeatsView = new ListCollectionView(new List<Feat>(Feat.FeatMap.Values));
                _FeatsView.Filter = FeatsViewFilter;
                _FeatsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                SelectFeatBox.DataContext = _FeatsView;
            }
        }

        private bool FeatsViewFilter(Object obj)
        {
            var feat = (Feat)obj;

            var text = FeatFilterTextBox.Text.Trim();

            if (text.Length == 0)
            {
                return true;
            }

            return feat.Name.ToUpper().Contains(text.ToUpper());
        }

        private void FeatFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_FeatsView != null)
            {
                _FeatsView.Refresh();
            }
        }

        private void SelectFeatBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CMUIUtilities.ClickedListBoxItem((ListBox)sender, e) != null)
            {
                AddSelectedFeats();
            }
        }

        private void AddSelectedFeats()
        {
            foreach (Feat feat in SelectFeatBox.SelectedItems)
            {
                var name = feat.Name;

                if (feat.AltName != null && feat.AltName.Length > 0)
                {
                    name = feat.AltName;
                }

                if (!_Monster.FeatsList.Contains(name))
                {
                    _Monster.AddFeat(name);
                    _CurrentFeats.Add(new ParsedFeat(name));
                }
            }
        }


        public Monster Monster
        {
            get
            {
                return _Monster;
            }
            set
            {
                _Monster = value;
                _CurrentFeats = new ObservableCollection<ParsedFeat>();
                foreach (var feat in _Monster.FeatsList)
                {
                    var cf = new ParsedFeat();
                    _CurrentFeats.Add(new ParsedFeat(feat));
                }
                _CurrentFeatsView = new ListCollectionView(_CurrentFeats);
                _CurrentFeatsView.SortDescriptions.Add(
                new SortDescription("Name", ListSortDirection.Ascending));
                CurrentFeatsBox.DataContext = _CurrentFeatsView;
            }
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var el = (FrameworkElement)sender;

            var feat = (ParsedFeat)el.DataContext;

            _CurrentFeats.Remove(feat);
            _Monster.RemoveFeat(feat.Text);

        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var el = (FrameworkElement)sender;

            var feat = (ParsedFeat)el.DataContext;
        	_Monster.RemoveFeat(feat.FeatSource);
            feat.FeatSource = feat.Text;
			_Monster.AddFeat(feat.FeatSource);
			
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			AddSelectedFeats();
        }

        private void CurrentFeatsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentFeatsBox.SelectedItem == null)
            {
                //RemoveButton.IsEnabled = false;
            }
            else
            {
                //RemoveButton.IsEnabled = true;
            }
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var feat = (ParsedFeat)CurrentFeatsBox.SelectedItem;

            if (feat != null)
            {
                _CurrentFeats.Remove(feat);
               _Monster.RemoveFeat(feat.Text);
            }
        }


    
	}
}