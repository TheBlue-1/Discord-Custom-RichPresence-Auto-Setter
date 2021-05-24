#region
using System.Linq;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Metadata;
using GameSDK.GameSDK;
using Newtonsoft.Json;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Lobby : ListableBase, ICloneable<Lobby>, IValuesComparable<Lobby> {
		private uint _capacity = 1;
		private bool _locked;
		private MetaDataList _metadata;
		private long _ownerId;
		private LobbyType _type = LobbyType.Private;
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
		public MetaDataList Metadata {
			get => _metadata;
			set {
				if (_metadata != null) {
					_metadata.CollectionChanged -= MetadataChanged;
					_metadata.InnerPropertyChanged -= MetadataChanged;
				}

				_metadata = value;
				if (_metadata != null) {
					_metadata.CollectionChanged += MetadataChanged;
					_metadata.InnerPropertyChanged += MetadataChanged;
				}
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

		[JsonConstructor]
		public Lobby() => Metadata = new MetaDataList();

		protected Lobby(Lobby lobby) : base(lobby.Name) {
			Capacity = lobby.Capacity;
			Locked = lobby.Locked;
			Type = lobby.Type;
			OwnerId = lobby.OwnerId;
			Metadata = lobby.Metadata;
		}

		private void MetadataChanged(object sender, object e) {
			OnPropertyChanged(nameof (Metadata));
		}

		Lobby ICloneable<Lobby>.Clone() => new(this);

		bool IValuesComparable<Lobby>.ValuesCompare(Lobby other) {
			if (other.Capacity != Capacity) {
				return false;
			}
			if (other.Locked != Locked) {
				return false;
			}
			if (other.Type != Type) {
				return false;
			}
			if (other.OwnerId != OwnerId) {
				return false;
			}

			if (other.Metadata?.Count != Metadata?.Count) {
				return false;
			}
			if (Metadata == null) {
				return true;
			}

			if (Metadata.Where((t, i) => other.Metadata[i] != t).Any()) {
				return false;
			}

			return other.Name == Name;
		}
	}
}
