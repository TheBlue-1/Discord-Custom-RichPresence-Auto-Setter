#region
using System.Windows;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public RichPresenceSetter.Activity Activity { get; set; } = RichPresenceSetter.DefaultActivity;
		public RichPresenceSetter.Lobby Lobby { get; set; } = RichPresenceSetter.DefaultLobby;

		public MainWindow() {
			InitializeComponent();
			RichPresenceManager.Init(793125319657783306);
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			_ = RichPresenceManager.Instance.UseConfig(new RichPresenceManager.Config(Activity, Lobby, null));
		}
	}
}
