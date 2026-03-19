using System.Windows;

namespace CombatManager
{
	/// <summary>
	/// Interaction logic for PlayersOrMonstersDialog.xaml
	/// </summary>
	public partial class PlayersOrMonstersDialog : Window
	{
		public PlayersOrMonstersDialog()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void PlayersButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            Monsters = false;
            Close();
		}

		private void MonstersButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            DialogResult = true;
            Monsters = true;
            Close();
		}

		private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            DialogResult = false;
            Close();
		}

        public bool Monsters { get; set; }

        public string Filename { get; set; }

	}
}