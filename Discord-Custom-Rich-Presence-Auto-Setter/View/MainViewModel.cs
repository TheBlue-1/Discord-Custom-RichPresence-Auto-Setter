#region
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	public class MainViewModel : INotifyPropertyChanged {
		private ObservableCollection<IListable> Activities { get; } = new();
		public RelayCommand ActivitiesClick => new(() => {
			List = Activities;
			OnPropertyChanged(nameof (List));
		}, () => List != Activities);
		private ObservableCollection<IListable> App { get; } = new();

		public RelayCommand AppClick => new(() => {
			List = App;
			OnPropertyChanged(nameof (List));
		}, () => List != App);

		private ObservableCollection<IListable> Configs { get; } = new();
		public RelayCommand ConfigsClick => new(() => {
			List = Configs;
			OnPropertyChanged(nameof (List));
		}, () => List != Configs);
		private ObservableCollection<IListable> Lobbies { get; } = new();
		public RelayCommand LobbiesClick => new(() => {
			List = Lobbies;
			OnPropertyChanged(nameof (List));
		}, () => List != Lobbies);

		public ObservableCollection<IListable> List { get; private set; }

		public MainViewModel() {
			List = App;
			LoadTestData();
		}

		private void LoadTestData() {
			Activities.Add(Activity.DefaultActivity);
			Lobbies.Add(Lobby.DefaultLobby);
			Configs.Add(Config.DefaultConfig);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
