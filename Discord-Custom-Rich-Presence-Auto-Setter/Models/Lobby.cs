#region
using System.Collections.Generic;
using GameSDK.GameSDK;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public record Lobby(string Name, uint Capacity, bool Locked, LobbyType Type, long OwnerId, Dictionary<string, string> Metadata) : IListable {
		public static Lobby DefaultLobby => new("Default Lobby", 2, true, LobbyType.Private, 0, new Dictionary<string, string>());
		public IListable Duplicate() => new Lobby(this);
	}
}
