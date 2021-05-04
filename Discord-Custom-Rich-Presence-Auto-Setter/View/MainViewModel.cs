#region
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Service;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
using Activity = Discord_Custom_Rich_Presence_Auto_Setter.Models.Activity;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	public class MainViewModel : INotifyPropertyChanged {
		private ObservableCollection<IListable> _list;
		private IListable _selected;
		private string _status = "Application started";

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
		public Visibility AppVisibility => List == App ?  Visibility.Visible: Visibility.Hidden ;
		public ApplicationSettings ApplicationSettings  =>Discord_Custom_Rich_Presence_Auto_Setter.App.Settings;

		public string Status {
			get => _status;
			set { _status = value;OnPropertyChanged(); FileService.Instance.Log($"Status: {value}");}
		}

		private RichPresenceManager RichPresenceManager { get; }
		private FileSyncedConfigs Data { get; } 
		public RelayCommand DeleteClick => new(() => { List.Remove(Selected); }, () => Selected != null);

		public RelayCommand OpenDataDirectory => new RelayCommand(() => { Process.Start("explorer.exe", FileService.ApplicationFolderPath); });
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
				OnPropertyChanged(nameof(AppVisibility));
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

		public MainViewModel() {
			RichPresenceManager = new RichPresenceManager(ApplicationSettings.DefaultApplicationId);
			Data = RichPresenceManager.Configs;
			List = App;
            RichPresenceManager.CurrentlyUsedConfigChanged += RichPresenceChanged;
            RichPresenceManager.ExceptionOccured += ManagerExceptionOccured;
			RichPresenceManager.Start();
		}

        private void ManagerExceptionOccured(System.Exception exception) {
			Status = $"Error: '{exception.Message}'";
		}

        private void RichPresenceChanged(Config config) {
			Status = config != null ? $"{config.Name} is currently set as your rich presence" : "No rich presence is currently set";
		}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
