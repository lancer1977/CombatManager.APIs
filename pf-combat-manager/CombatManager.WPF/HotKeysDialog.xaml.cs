using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace CombatManager
{
    /// <summary>
    /// Interaction logic for HotKeysDialog.xaml
    /// </summary>
    public partial class HotKeysDialog : Window
    {
		 ObservableCollection<CombatHotKey> _CombatHotKeys;

        SortedDictionary<string, CharacterAction> names = new SortedDictionary<string, CharacterAction>();


        public HotKeysDialog()
        {
            InitializeComponent();


            foreach (CharacterAction type in Enum.GetValues(typeof(CharacterAction)))
            {
                var name = UICharacterActionHandler.Description(type);
                names[name] = type;
            }

        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var hk = (CombatHotKey)((FrameworkElement)sender).DataContext;
            _CombatHotKeys.Remove(hk);
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			DialogResult = false;
        	Close();
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			DialogResult = true;
        	Close();
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _CombatHotKeys.Add(new CombatHotKey());
        }       
		
		public List<CombatHotKey> CombatHotKeys 
        {
            get
            {
                return new List<CombatHotKey>(_CombatHotKeys);
            }
            set
            {
                _CombatHotKeys = new ObservableCollection<CombatHotKey>();
                foreach (var hk in value)
                {
                    _CombatHotKeys.Add(new CombatHotKey(hk));
                }
                KeyListBox.ItemsSource = _CombatHotKeys;
            }
        }


        private void CommandComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
            var typeCombo = (ComboBox)sender;

            var cbi = (ComboBoxItem)typeCombo.SelectedItem;
            var hk = (CombatHotKey)typeCombo.DataContext;
            hk.Type = (CharacterAction)cbi.DataContext;

            var subtypeCombo = typeCombo.GetSibling<ComboBox>("SubtypeComboBox");

            UpdateSubtypeCombo(subtypeCombo);

		}

        private void UpdateSubtypeCombo(ComboBox subtypeCombo)
        {

            if (subtypeCombo != null)
            {
                var cb = subtypeCombo;
                var hk = (CombatHotKey)cb.DataContext;

            
                subtypeCombo.Items.Clear();
                switch ((CharacterAction)hk.Type)
                {
                    case CharacterAction.Save:
                        subtypeCombo.IsEnabled = true;
                        subtypeCombo.Items.Add(new ComboBoxItem() { Content = "Fort" });
                        subtypeCombo.Items.Add(new ComboBoxItem() { Content = "Ref" });
                        subtypeCombo.Items.Add(new ComboBoxItem() { Content = "Will" });
                        SetIndexForString(cb, hk.Subtype);
                        break;
                    case CharacterAction.Skill:
                        subtypeCombo.IsEnabled = true;
                        foreach (var si in Monster.SkillsDetails.Values)
                        {
                            subtypeCombo.Items.Add(new ComboBoxItem() { Content = si.Name, Tag = si });
                        }
                        SetIndexForString(cb, hk.Subtype);
                        break;
                    case CharacterAction.ApplyCondition:
                        UpdateConditionSubtype(subtypeCombo, hk);
                        break;
                    default:
                        subtypeCombo.IsEnabled = false;
                        break;
                }
            }
            
        }


        private void UpdateConditionSubtype(ComboBox subtypeCombo, CombatHotKey hk)
        {
            subtypeCombo.IsEnabled = true;

            var hkcond = hk.Subtype;
            var found = false;

            foreach (var c in from x in Condition.Conditions where x.Spell == null select x )
            {
                var panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;


                var i = new Image();
                var bi = StringImageSmallIconConverter.FromName(c.Image);
                i.Source = bi;
                i.Width = 16;
                i.Height = 16;

                panel.Children.Add(i);
                panel.Children.Add(new TextBlock() { Text = c.Name });
                var cbi = new ComboBoxItem() { Content = panel, Tag = c };

                subtypeCombo.Items.Add(cbi);

                if (c.Name == hkcond)
                {
                    subtypeCombo.SelectedItem = cbi;
                    found = true;
                }
            }
            if (!found)
            {
                subtypeCombo.SelectedIndex = 0;
            }
        }

        private void SetIndexForString(ComboBox cb, String str)
        {
            var val = -1;
            for (var i = 0; i < cb.Items.Count; i++)
            {
                var it = (ComboBoxItem)cb.Items[i];
                if ((it.Content as string) == str)
                {
                    val = i;
                    break;
                }
            }

            if (val != -1)
            {

                cb.SelectedIndex = val;
            }
            else
            {
                cb.SelectedIndex = 0;
            }
        }




        private void KeyComboBox_Initialized(object sender, System.EventArgs e)
		{
            var cb = (ComboBox)sender;
            var hk = (CombatHotKey)cb.DataContext;
            

            foreach (var k in KeyToStringConverter.KeysList)
            {
                var item = new ComboBoxItem();
                item.Content = k.Key;
                item.DataContext = k.Value;
                cb.Items.Add(item);
            }

            var key = hk.Key.ToString();// (String)new KeyToStringConverter().Convert(hk.Key, 
                //typeof(String), null, System.Globalization.CultureInfo.CurrentCulture);


            SetIndexForString(cb, key);
		}

		private void CommandComboBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
		}

		private void CommandComboBox_Initialized(object sender, System.EventArgs e)
		{
            var cb = (ComboBox)sender;

            var hk = (CombatHotKey)cb.DataContext;

            ComboBoxItem selected = null;

            foreach (var kv in names)
            {
                var cbi = new ComboBoxItem();
                cbi.Content = kv.Key;
                cbi.DataContext = kv.Value;

                if (kv.Value == hk.Type)
                {
                    selected = cbi;
                }

                cb.Items.Add(cbi);
            }
            cb.SelectedItem = selected;
		}
       
		private void SubtypeComboBox_Initialized(object sender, System.EventArgs e)
		{
            var cb = (ComboBox)sender;
            UpdateSubtypeCombo(cb);

            
		}

		private void SubtypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
            var cb = ((ComboBox)sender);

			var hk = (CombatHotKey)cb.DataContext;
            if (cb.SelectedValue != null)
            {
                var item = (ComboBoxItem)cb.SelectedItem;
                if (item.Tag is Condition)
                {
                    hk.Subtype = ((Condition)item.Tag).Name;
                }
                else
                {
                    hk.Subtype = (String)((ComboBoxItem)cb.SelectedValue).Content;
                }
            }

		}

		private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            var cb = ((CheckBox)sender);
			var hk = (CombatHotKey)cb.DataContext;
            var parent = (Grid)VisualTreeHelper.GetParent(cb);
            UpdateBackground(parent, hk);
		}

        private void UpdateBackground(Grid grid, CombatHotKey hk)
        {
            if (hk.Modifier == ModifierKeys.None || hk.Modifier == ModifierKeys.Shift)
            {
                grid.Background = new SolidColorBrush(Colors.Pink);
            }
            else
            {
                grid.Background = null;
            }
        }

        private void ItemBackground_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {

            var cb = ((Grid)sender);
			var hk = (CombatHotKey)cb.DataContext;
            UpdateBackground(cb, hk);
        }

        private void ItemBackground_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            var cb = ((Grid)sender);
			var hk = (CombatHotKey)cb.DataContext;
            UpdateBackground(cb, hk);

            UpdateSubtypeCombo(cb.GetChild<ComboBox>("SubtypeCombo"));
        }

        private void KeyPressButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var hk = (CombatHotKey)((FrameworkElement)sender).DataContext;


            var ctrl = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
            var alt = Keyboard.Modifiers.HasFlag(ModifierKeys.Alt);
            var shift = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
            if (!KeyToStringConverter.IgnoreKeys.Contains(e.Key) && (ctrl | alt | shift))
            {
                hk.Key = e.Key;
                hk.CtrlKey = ctrl;
                hk.AltKey = alt;
                hk.ShiftKey = shift;

            }
        }
    }
}
