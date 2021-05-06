#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements;
using Newtonsoft.Json;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Config : ListableBase, ICloneable<Config>, IValuesComparable<Config> {
		private Activity _activity;
		private Guid _activityId;
		private long? _applicationId;
		private Lobby _lobby;
		private Guid _lobbyId;
		private ObservableCollection<Requirement> _requirements;

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
		public Config() { }

		//public Config(string name, Activity activity, Lobby lobby, ObservableCollection<Requirement> requirements, long? applicationId) : base(name) {
		//	Activity = activity;
		//	Lobby = lobby;
		//	Requirements = requirements;
		//	ApplicationId = applicationId;
		//}

		private Config(Config config) : base(config.Name) {
			ApplicationId = config.ApplicationId;

			Activity = ICloneable.Clone(config.Activity);
			Lobby = ICloneable.Clone(config.Lobby);
			Lobby.Metadata = new Dictionary<string, string>(config.Lobby.Metadata);
			Requirements = new ObservableCollection<Requirement>(config.Requirements.Select(ICloneable.Clone));
		}

		Config ICloneable<Config>.Clone() => new(this);

		bool IValuesComparable<Config>.ValuesCompare(Config other) => throw new NotImplementedException();
	}
}
