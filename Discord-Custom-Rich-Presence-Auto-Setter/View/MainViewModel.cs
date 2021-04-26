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

		private GeneralizedObservableList<IListable, Activity> Activities { get; } = new();
		public RelayCommand ActivitiesClick => new(() => { List = Activities; }, () => List != Activities);
		public Visibility ActivityVisibility => SelectedActivity == null ? Visibility.Hidden : Visibility.Visible;
		private ObservableCollection<IListable> App { get; } = new();

		public RelayCommand AppClick => new(() => { List = App; }, () => List != App);
		public RelayCommand DeleteClick => new(() => { List.Remove(Selected); }, () => Selected!=null);
		public RelayCommand DuplicateClick => new(() => { List.Insert(SelectedIndex+1,Selected.Duplicate() ); }, () => Selected!=null);
		public RelayCommand UpClick => new(() => { List.Move(SelectedIndex,SelectedIndex-1); }, () => Selected!=null&&SelectedIndex>0);
		public RelayCommand DownClick => new(() => { List.Move(SelectedIndex, SelectedIndex + 1); }, () => Selected != null && SelectedIndex < List.Count-1);

		private GeneralizedObservableList<IListable, Config> Configs { get; } = new();
		public RelayCommand ConfigsClick => new(() => { List = Configs; }, () => List != Configs);
		public Visibility ConfigVisibility => SelectedConfig == null ? Visibility.Hidden : Visibility.Visible;
		private GeneralizedObservableList<IListable, Lobby> Lobbies { get; } = new();
		public RelayCommand LobbiesClick => new(() => { List = Lobbies; }, () => List != Lobbies);

		public Visibility LobbyVisibility => SelectedLobby == null ? Visibility.Hidden : Visibility.Visible;

		public Activity SelectedActivity => Selected as Activity;

		public Config SelectedConfig => Selected as Config;
		public Lobby SelectedLobby => Selected as Lobby;

		public ObservableCollection<IListable> List {
			get => _list;
			private set {
				_list = value;
				OnPropertyChanged();
			}
		}
		public int SelectedIndex {
			get;
			set;

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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
