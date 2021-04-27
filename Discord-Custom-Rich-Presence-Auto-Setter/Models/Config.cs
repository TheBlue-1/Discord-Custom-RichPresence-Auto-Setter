#region
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Requirements;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Config : IListable, INotifyPropertyChanged {
		private Activity _activity;
		private long? _applicationId;
		private Lobby _lobby;
		private string _name;
		private List<Requirement> _requirements;
		public static Config DefaultConfig => new("Default Config", Activity.DefaultActivity, Lobby.DefaultLobby, new List<Requirement>(), null);
		public Activity Activity {
			get => _activity;
			set {
				_activity = value;
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
		public Lobby Lobby {
			get => _lobby;
			set {
				_lobby = value;
				OnPropertyChanged();
			}
		}
		public string Name {
			get => _name;
			set {
				_name = value;
				OnPropertyChanged();
			}
		}
		public List<Requirement> Requirements {
			get => _requirements;
			set {
				_requirements = value;
				OnPropertyChanged();
			}
		}

		public Config(string name, Activity activity, Lobby lobby, List<Requirement> requirements, long? applicationId) {
			Name = name;
			Activity = activity;
			Lobby = lobby;
			Requirements = requirements;
			ApplicationId = applicationId;
		}

		public Config(Config config) {
			Name = config.Name;
			Activity = config.Activity;
			Lobby = config.Lobby;
			Requirements = config.Requirements;
			ApplicationId = config.ApplicationId;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public IListable Duplicate() => new Config(this);

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
