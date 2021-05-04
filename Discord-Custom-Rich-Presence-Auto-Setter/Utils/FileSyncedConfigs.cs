#region
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils.Extensions;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public class FileSyncedConfigs {
		public GeneralizedObservableList<IListable, Activity> Activities { get; }
		private const string ActivitiesFileName = "activities";
		public GeneralizedObservableList<IListable, Config> Configs { get; }
		private const string ConfigsFileName = "configs";
		public GeneralizedObservableList<IListable, Lobby> Lobbies { get; }
		private IEnumerable<Lobby> EnumerableLobbies => Lobbies;
		private IEnumerable<Activity> EnumerableActivities => Activities;
		private const string LobbiesFileName = "lobbies";
		private object SaveActivitiesLock { get; } = new();
		private object SaveConfigsLock { get; } = new();
		private object SaveLobbiesLock { get; } = new();

		private bool SaveActivitiesAgain { get; set; }
		private bool SaveActivitiesRunning { get; set; }
		private bool SaveConfigsAgain { get; set; }
		private bool SaveConfigsRunning { get; set; }
		private bool SaveLobbiesAgain { get; set; }
		private bool SaveLobbiesRunning { get; set; }

		public FileSyncedConfigs() {
			Lobbies = FileService.Instance.JsonFileExists(LobbiesFileName) ? LoadLobbies() : new GeneralizedObservableList<IListable, Lobby>();
			Activities = FileService.Instance.JsonFileExists(ActivitiesFileName)
				? LoadActivities()
				: new GeneralizedObservableList<IListable, Activity>();
			Configs = FileService.Instance.JsonFileExists(ConfigsFileName) ? LoadConfigs() : new GeneralizedObservableList<IListable, Config>();
			
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

		private GeneralizedObservableList<IListable, Activity> LoadActivities() =>
			 FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Activity>>(ActivitiesFileName);

		private  GeneralizedObservableList<IListable, Config> LoadConfigs() {
			if (Lobbies == null || Activities == null) {
				throw new InvalidOperationException("Lobbies and activities must be loaded before configs");
			}
			GeneralizedObservableList<IListable, Config> configs=  FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Config>>(ConfigsFileName);
			foreach (Config config in configs) {
				config.Lobby = EnumerableLobbies.FirstOrDefault(lobby => lobby.Uuid == config.LobbyId);
				config.Activity = EnumerableActivities.FirstOrDefault(activity => activity.Uuid == config.LobbyId);

			}
			return configs;
		}

		private  GeneralizedObservableList<IListable, Lobby> LoadLobbies() =>
			 FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Lobby>>(LobbiesFileName);

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
