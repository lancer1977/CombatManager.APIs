/*
 *  SpellEditorWindow.xaml.cs
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

using System.Windows;
using System.Windows.Controls;

namespace CombatManager
{
	/// <summary>
	/// Interaction logic for SpellEditorWindow.xaml
	/// </summary>
	public partial class SpellEditorWindow : Window
	{
		private bool _Initialized;
		
		public SpellEditorWindow()
		{
			this.InitializeComponent();

			
			
            foreach (var s in SpellSchoolIndexConverter.Schools)
            {
                SchoolComboBox.Items.Add(s.Capitalize());
            }
			
			_Initialized = true;
			
			UpdateOK();
		}

        private Spell _Spell;

        public Spell Spell
        {
            get { return _Spell; }
            set
            {
                if (_Spell != value)
                {
                    _Spell = value;
					DataContext = _Spell;

                    for (var i = 0; i < SchoolComboBox.Items.Count; i++)
                    {
                        var sch = (string)SchoolComboBox.Items[i];
                        if (sch == StringCapitalizeConverter.Capitalize(_Spell.school))
                        {
                            SchoolComboBox.SelectedIndex = i;
                            break;
                        }
                    }
					
					_Spell.Adjuster.Levels.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Levels_CollectionChanged);


                    CustomBonusBorder.DataContext = _Spell.Bonus;

                    CustomBonusCheckBox.IsChecked = (_Spell.Bonus != null);
                }
            }
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var info = (Spell.SpellAdjuster.LevelAdjusterInfo)
                ((FrameworkElement)sender).DataContext;

            Spell.Adjuster.Levels.Remove(info);
        }
        private void UnusedClassesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var box = ((ListBox)sender);



            var info = (Spell.SpellAdjuster.LevelAdjusterInfo)
                box.SelectedItem;

            Spell.Adjuster.Levels.Add(info);
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            var b = (ConditionBonus)CustomBonusBorder.DataContext;

            _Spell.Bonus = b;

        	DialogResult = true;
            Close();
        }

        private void SpellNameText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        	UpdateOK();
        }

        private void Levels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        	UpdateOK();
        }
		
		private void UpdateOK()
		{
			if (_Initialized)
			{
				OKButton.IsEnabled = SpellNameText.Text.Length > 0 && Spell.Adjuster.Levels.Count > 0;
			}
		}

		private void CustomBonusCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{

            CustomBonusBorder.DataContext = new ConditionBonus();
		}

        private void CustomBonusCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            CustomBonusBorder.DataContext = null;
        }

		
	}
}