#region
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	public class MainViewModel : INotifyPropertyChanged {
		private ObservableCollection<IListable> _list;
		private IListable _selected;

		public RelayCommand ActivitiesClick => new(() => { List = Data.Activities; }, () => List != Data.Activities);
		public Visibility ActivityVisibility => SelectedActivity == null ? Visibility.Hidden : Visibility.Visible;
		public RelayCommand AddClick => new(() => {
			if (List == Data.Configs) {
				Data.Configs.Add(Config.DefaultConfig);
			} else if (List == Data.Activities) {
				Data.Activities.Add(Activity.DefaultActivity);
			} else if (List == Data.Lobbies) {
				Data.Lobbies.Add(Lobby.DefaultLobby);
			}
		}, () => List != App);
		private ObservableCollection<IListable> App { get; } = new();

		public RelayCommand AppClick => new(() => { List = App; }, () => List != App);

		public RelayCommand ConfigsClick => new(() => { List = Data.Configs; }, () => List != Data.Configs);
		public Visibility ConfigVisibility => SelectedConfig == null ? Visibility.Hidden : Visibility.Visible;

		private FileSyncedConfigs Data { get; } = new("data");
		public RelayCommand DeleteClick => new(() => { List.Remove(Selected); }, () => Selected != null);

		public RelayCommand DownClick => new(() => { List.Move(SelectedIndex, SelectedIndex + 1); }, () => Selected != null && SelectedIndex < List.Count - 1);
		public RelayCommand DuplicateClick => new(() => { List.Insert(SelectedIndex + 1, Selected.Duplicate()); }, () => Selected != null);
		public RelayCommand LobbiesClick => new(() => { List = Data.Lobbies; }, () => List != Data.Lobbies);

		public Visibility LobbyVisibility => SelectedLobby == null ? Visibility.Hidden : Visibility.Visible;

		public Activity SelectedActivity => Selected as Activity;

		public Config SelectedConfig => Selected as Config;
		public Lobby SelectedLobby => Selected as Lobby;
		public RelayCommand UpClick => new(() => { List.Move(SelectedIndex, SelectedIndex - 1); }, () => Selected != null && SelectedIndex > 0);

		public ObservableCollection<IListable> List {
			get => _list;
			private set {
				_list = value;
				OnPropertyChanged();
			}
		}
		public IListable Selected {
			get => _selected;
			set {
				_selected = value;
				OnPropertyChanged(nameof (Selected));
				OnPropertyChanged(nameof (SelectedLobby));
				OnPropertyChanged(nameof (SelectedActivity));
				OnPropertyChanged(nameof (SelectedConfig));
				OnPropertyChanged(nameof (LobbyVisibility));
				OnPropertyChanged(nameof (ActivityVisibility));
				OnPropertyChanged(nameof (ConfigVisibility));
			}
		}
		public int SelectedIndex { get; set; }

		public MainViewModel() => List = App;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
