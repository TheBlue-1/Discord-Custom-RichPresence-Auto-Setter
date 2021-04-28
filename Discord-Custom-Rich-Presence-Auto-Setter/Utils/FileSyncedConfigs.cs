#region
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public class FileSyncedConfigs {
		private const string ActivitiesKey = "activities";
		private const string ConfigsKey = "configs";
		private const string LobbiesKey = "lobbies";
		public GeneralizedObservableList<IListable, Activity> Activities { get; }
		public GeneralizedObservableList<IListable, Config> Configs { get; }
		public GeneralizedObservableList<IListable, Lobby> Lobbies { get; }

		public FileSyncedConfigs(string directoryPath) {
			Configs = new GeneralizedObservableList<IListable, Config>();
			Lobbies = new GeneralizedObservableList<IListable, Lobby>();
			Activities = new GeneralizedObservableList<IListable, Activity>();
			Configs.CollectionChanged += ConfigsChanged;
			Lobbies.CollectionChanged += LobbiesChanged;
			Activities.CollectionChanged += ActivitiesChanged;
			Configs.InnerPropertyChanged += ConfigsItemChanged;
			Lobbies.InnerPropertyChanged += LobbiesItemChanged;
			Activities.InnerPropertyChanged += ActivitiesItemChanged;
		}

		private void ActivitiesChanged(object sender, NotifyCollectionChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private void ActivitiesItemChanged(object sender, PropertyChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private void ConfigsChanged(object sender, NotifyCollectionChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private void ConfigsItemChanged(object sender, PropertyChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private async Task<GeneralizedObservableList<IListable, Activity>> LoadActivities() =>
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Activity>>(ActivitiesKey);

		private async Task<GeneralizedObservableList<IListable, Config>> LoadConfigs() =>

			//will currently load lobbies and activities in a bad way
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Config>>(ConfigsKey);

		private async Task<GeneralizedObservableList<IListable, Lobby>> LoadLobbies() =>
			await FileService.Instance.ReadJsonFromFile<GeneralizedObservableList<IListable, Lobby>>(LobbiesKey);

		private void LobbiesChanged(object sender, NotifyCollectionChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private void LobbiesItemChanged(object sender, PropertyChangedEventArgs e) {
			throw new NotImplementedException();
		}

		private async Task SaveActivities() {
			await FileService.Instance.SaveJsonToFile(ActivitiesKey, Activities);
		}

		private async Task SaveConfigs() {
			//will currently save lobbies and activities in a bad way
			await FileService.Instance.SaveJsonToFile(ConfigsKey, Configs);
		}

		private async Task SaveLobbies() {
			await FileService.Instance.SaveJsonToFile(LobbiesKey, Lobbies);
		}
	}
}
