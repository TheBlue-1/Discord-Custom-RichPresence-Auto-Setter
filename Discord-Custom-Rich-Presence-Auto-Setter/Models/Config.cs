#region
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
using Newtonsoft.Json;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Config : ListableBase, ICloneable<Config>, IValuesComparable<Config> {
		private Activity _activity = new();
		private Guid _activityId;
		private long? _applicationId;
		private Lobby _lobby = new();
		private Guid _lobbyId;
		private ObservableCollection<Requirement> _requirements = new();

		[JsonIgnore]
		public Activity Activity {
			get => _activity;
			set {
				_activity = value;
				OnPropertyChanged();
				ActivityId = value?.Uuid ?? default;
			}
		}
		public Guid ActivityId {
			get => _activityId;
			private set {
				_activityId = value;
				OnPropertyChanged();
			}
		}
		public long? ApplicationId {
			get => _applicationId;
			set {
				_applicationId = value;
				OnPropertyChanged();
			}
		}
		[JsonIgnore]
		public Lobby Lobby {
			get => _lobby;
			set {
				_lobby = value;

				OnPropertyChanged();

				LobbyId = value?.Uuid ?? default;
			}
		}
		public Guid LobbyId {
			get => _lobbyId;
			private set {
				_lobbyId = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Requirement> Requirements {
			get => _requirements;
			set {
				_requirements = value;
				OnPropertyChanged();
			}
		}

		[JsonConstructor]
		public Config() {
			_lobbyId = _lobby.Uuid;
			_activityId = _activity.Uuid;
		}

		private Config(Config config) : base(config.Name) {
			ApplicationId = config.ApplicationId;

			Activity = ICloneable.Clone(config.Activity);
			Lobby = ICloneable.Clone(config.Lobby);
			Lobby.Metadata = new ObservableDictionary<string, string>(config.Lobby.Metadata);
			Requirements = new ObservableCollection<Requirement>(config.Requirements.Select(ICloneable.Clone));
		}

		Config ICloneable<Config>.Clone() => new(this);

		bool IValuesComparable<Config>.ValuesCompare(Config other) {
			if (other.ApplicationId != ApplicationId) {
				return false;
			}
			if (other.LobbyId != LobbyId) {
				return false;
			}
			if (other.ActivityId != ActivityId) {
				return false;
			}
			if (!IValuesComparable.ValuesCompare(other.Lobby, Lobby)) {
				return false;
			}
			if (!IValuesComparable.ValuesCompare(other.Activity, Activity)) {
				return false;
			}

			if (other.Requirements?.Count != Requirements?.Count) {
				return false;
			}
			if (Requirements == null) {
				return true;
			}

			if (Requirements.Where((t, i) => other.Requirements[i] != t).Any()) {
				return false;
			}

			return other.Name != Name;
		}
	}
}
