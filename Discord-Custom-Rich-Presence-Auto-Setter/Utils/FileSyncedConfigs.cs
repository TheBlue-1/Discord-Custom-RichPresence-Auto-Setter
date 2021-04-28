#region
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils.Extensions;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public class FileSyncedConfigs {
		private readonly string _directoryName;
		public GeneralizedObservableList<IListable, Activity> Activities { get; }
		private string ActivitiesFileName => Path.Combine(_directoryName, "activities");
		public GeneralizedObservableList<IListable, Config> Configs { get; }
		private string ConfigsFileName => Path.Combine(_directoryName, "configs");
		public GeneralizedObservableList<IListable, Lobby> Lobbies { get; }
		private string LobbiesFileName => Path.Combine(_directoryName, "lobbies");
		private object SaveActivitiesLock { get; } = new();
		private object SaveConfigsLock { get; } = new();
		private object SaveLobbiesLock { get; } = new();

		private bool SaveActivitiesAgain { get; set; }
		private bool SaveActivitiesRunning { get; set; }
		private bool SaveConfigsAgain { get; set; }
		private bool SaveConfigsRunning { get; set; }
		private bool SaveLobbiesAgain { get; set; }
		private bool SaveLobbiesRunning { get; set; }

		public FileSyncedConfigs(string directoryName) {
			_directoryName = directoryName;
			Configs = FileService.Instance.JsonFileExists(ConfigsFileName) ? LoadConfigs().WaitForResult() : new GeneralizedObservableList<IListable, Config>();
			Lobbies = FileService.Instance.JsonFileExists(ConfigsFileName) ? LoadLobbies().WaitForResult() : new GeneralizedObservableList<IListable, Lobby>();
			Activities = FileService.Instance.JsonFileExists(ConfigsFileName)
				? LoadActivities().WaitForResult()
				: new GeneralizedObservableList<IListable, Activity>();
			Configs.CollectionChanged += ConfigsChanged;
			Lobbies.CollectionChanged += LobbiesChanged;
			Activities.CollectionChanged += ActivitiesChanged;
			Configs.InnerPropertyChanged += ConfigsItemChanged;
			Lobbies.InnerPropertyChanged += LobbiesItemChanged;
			Activities.InnerPropertyChanged += ActivitiesItemChanged;
		}

		private async void ActivitiesChanged(object sender, NotifyCollectionChangedEventArgs e) {
			await SaveActivities();
		}

		private async void ActivitiesItemChanged(object sender, PropertyChangedEventArgs e) {
			await SaveActivities();
		}

		private async void ConfigsChanged(object sender, NotifyCollectionChangedEventArgs e) {
			await SaveConfigs();
		}

		private async void ConfigsItemChanged(object sender, PropertyChangedEventArgs e) {
			await SaveConfigs();
		}

		private async Task<GeneralizedObservableList<IListable, Activity>> LoadActivities() =>
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Activity>>(ActivitiesFileName);

		private async Task<GeneralizedObservableList<IListable, Config>> LoadConfigs() =>

			//will currently load lobbies and activities in a bad way
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Config>>(ConfigsFileName);

		private async Task<GeneralizedObservableList<IListable, Lobby>> LoadLobbies() =>
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Lobby>>(LobbiesFileName);

		private async void LobbiesChanged(object sender, NotifyCollectionChangedEventArgs e) {
			await SaveLobbies();
		}

		private async void LobbiesItemChanged(object sender, PropertyChangedEventArgs e) {
			await SaveLobbies();
		}

		private async Task SaveActivities() {
			lock (SaveActivitiesLock) {
				if (SaveActivitiesRunning) {
					SaveActivitiesAgain = true;
					return;
				}
				SaveActivitiesRunning = true;
			}
			while (true) {
				await FileService.Instance.SaveJsonToFile(ActivitiesFileName, Activities);
				lock (SaveActivitiesLock) {
					if (SaveActivitiesAgain) {
						SaveActivitiesAgain = false;
						continue;
					}
					SaveActivitiesRunning = false;
					return;
				}
			}
		}

		private async Task SaveConfigs() {
			lock (SaveConfigsLock) {
				if (SaveConfigsRunning) {
					SaveConfigsAgain = true;
					return;
				}
				SaveConfigsRunning = true;
			}
			while (true) {
				//will currently save lobbies and activities in a bad way
				await FileService.Instance.SaveJsonToFile(ConfigsFileName, Configs);
				lock (SaveConfigsLock) {
					if (SaveConfigsAgain) {
						SaveConfigsAgain = false;
						continue;
					}
					SaveConfigsRunning = false;
					return;
				}
			}
		}

		private async Task SaveLobbies() {
			lock (SaveLobbiesLock) {
				if (SaveLobbiesRunning) {
					SaveLobbiesAgain = true;
					return;
				}
				SaveLobbiesRunning = true;
			}
			while (true) {
				await FileService.Instance.SaveJsonToFile(LobbiesFileName, Lobbies);
				lock (SaveLobbiesLock) {
					if (SaveLobbiesAgain) {
						SaveLobbiesAgain = false;
						continue;
					}
					SaveLobbiesRunning = false;
					return;
				}
			}
		}
	}
}
