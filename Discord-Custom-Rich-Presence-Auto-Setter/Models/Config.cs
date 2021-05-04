#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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


		public bool ValuesEqual(Config config) {
			config.


		}
		public static Config DefaultConfig =>
			new("Default Config", Activity.DefaultActivity, Lobby.DefaultLobby, new ObservableCollection<Requirement>(), null);
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

				LobbyId = value?.Uuid??default;
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
		public Config(string name):base(name) {
			
		}
		public Config(string name, Activity activity, Lobby lobby, ObservableCollection<Requirement> requirements, long? applicationId) : base(name) {
			Activity = activity;
			Lobby = lobby;
			Requirements = requirements;
			ApplicationId = applicationId;
		}

		public Config(Config config,bool deep=false) : base(config.Name) {
			
			ApplicationId = config.ApplicationId;
			if (deep) {
				Activity = new Activity(config.Activity);
				Lobby = new Lobby(config.Lobby) { Metadata = new Dictionary<string, string>(config.Lobby.Metadata) };
				Requirements = new ObservableCollection<Requirement>(config.Requirements.Select(r=>r.Clone()));
			} else {
				Activity = config.Activity;
				Lobby = config.Lobby;
				Requirements = config.Requirements;
			}
		}

		public override IListable Duplicate() => new Config(this);
	}
}
