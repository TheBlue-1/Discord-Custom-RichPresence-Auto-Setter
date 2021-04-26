#region
using System.Collections.Generic;
using System.Windows;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Requirements;
using Discord_Custom_Rich_Presence_Auto_Setter.Service;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public Activity Activity { get; set; } = Activity.DefaultActivity;
		public Lobby Lobby { get; set; } = Lobby.DefaultLobby;

		public MainWindow() {
			InitializeComponent();
			RichPresenceManager.Init(793125319657783306);
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			_ = RichPresenceManager.Instance.UseConfig(new Config("Default", Activity, Lobby, new List<Requirement>(), null));
		}
	}
}
