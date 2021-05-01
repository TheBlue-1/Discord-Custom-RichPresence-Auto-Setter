﻿#region
using Discord_Custom_Rich_Presence_Auto_Setter.Service;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		public MainWindow()
		{

			RichPresenceManager.Init(App.Settings.DefaultApplicationId);

			InitializeComponent();
		}
	}
}
