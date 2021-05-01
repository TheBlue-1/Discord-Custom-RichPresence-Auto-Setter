#region
using System;
using System.Collections.ObjectModel;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements;
using Newtonsoft.Json;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Config : ListableBase {
		private Activity _activity;
		private Guid _activityId;
		private long? _applicationId;
		private Lobby _lobby;
		private Guid _lobbyId;
		private ObservableCollection<Requirement> _requirements;
		public static Config DefaultConfig =>
			new("Default Config", Activity.DefaultActivity, Lobby.DefaultLobby, new ObservableCollection<Requirement>(), null);
		[JsonIgnore]
		public Activity Activity {
			get => _activity;
			set {
				_activity = value;
				OnPropertyChanged();
				ActivityId = value.Uuid;
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
				LobbyId = value.Uuid;
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

		public Config(string name, Activity activity, Lobby lobby, ObservableCollection<Requirement> requirements, long? applicationId) : base(name) {
			Activity = activity;
			Lobby = lobby;
			Requirements = requirements;
			ApplicationId = applicationId;
		}

		public Config(Config config) : base(config.Name) {
			Activity = config.Activity;
			Lobby = config.Lobby;
			Requirements = config.Requirements;
			ApplicationId = config.ApplicationId;
		}

		public override IListable Duplicate() => new Config(this);
	}
}
