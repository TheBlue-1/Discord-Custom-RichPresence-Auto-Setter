#region
using System.Windows;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private RichPresenceSetter Rps { get; } = new();

		public RichPresenceSetter.Activity Activity { get; set; } = RichPresenceSetter.DefaultActivity;
		public RichPresenceSetter.Lobby Lobby { get; set; } = RichPresenceSetter.DefaultLobby;

		public MainWindow() {
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			_ = Rps.UpdateActivity(Activity, Lobby);
		}
	}
}
