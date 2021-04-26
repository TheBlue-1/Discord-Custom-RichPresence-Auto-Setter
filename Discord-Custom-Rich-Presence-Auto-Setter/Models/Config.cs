#region
using System.Collections.Generic;
using Discord_Custom_Rich_Presence_Auto_Setter.Requirements;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public record Config(string Name, Activity Activity, Lobby Lobby, List<Requirement> Requirements, long? ApplicationId) : IListable {
		public static Config DefaultConfig => new("Default Config", Activity.DefaultActivity, Lobby.DefaultLobby, new List<Requirement>(), null);
		public IListable Duplicate() => new Config(this);
	}
}
