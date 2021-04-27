#region
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameSDK.GameSDK;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Lobby : IListable, INotifyPropertyChanged {
		private uint _capacity;
		private bool _locked;
		private Dictionary<string, string> _metadata;
		private string _name;
		private long _ownerId;
		private LobbyType _type;
		public static Lobby DefaultLobby => new("Default Lobby", 2, true, LobbyType.Private, 0, new Dictionary<string, string>());
		public uint Capacity {
			get => _capacity;
			set {
				_capacity = value;
				OnPropertyChanged();
			}
		}
		public bool Locked {
			get => _locked;
			set {
				_locked = value;
				OnPropertyChanged();
			}
		}
		public Dictionary<string, string> Metadata {
			get => _metadata;
			set {
				_metadata = value;
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
		public long OwnerId {
			get => _ownerId;
			set {
				_ownerId = value;
				OnPropertyChanged();
			}
		}
		public LobbyType Type {
			get => _type;
			set {
				_type = value;
				OnPropertyChanged();
			}
		}

		public Lobby(string name, uint capacity, bool locked, LobbyType type, long ownerId, Dictionary<string, string> metadata) {
			Name = name;
			Capacity = capacity;
			Locked = locked;
			Type = type;
			OwnerId = ownerId;
			Metadata = metadata;
		}

		public Lobby(Lobby lobby) {
			Name = lobby.Name;
			Capacity = lobby.Capacity;
			Locked = lobby.Locked;
			Type = lobby.Type;
			OwnerId = lobby.OwnerId;
			Metadata = lobby.Metadata;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public IListable Duplicate() => new Lobby(this);

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
