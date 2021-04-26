#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Service {
	public sealed class RichPresenceManager {
		private static RichPresenceManager _instance;

		//set in ui, save in file
		public List<Config> Configs { get; } = new();

		//set in ui, save in file
		private long DefaultApplicationId { get; }
		public static RichPresenceManager Instance => _instance ?? throw new InvalidOperationException("RichPresenceManager must be initiated before");

		private RichPresenceSetter Rpc { get; set; }

		private RichPresenceManager(long defaultApplicationId) => DefaultApplicationId = defaultApplicationId;

		public static void Init(long defaultApplicationId) {
			if (_instance != null) {
				throw new InvalidOperationException("RichPresenceManager can only be initiated once");
			}
			_instance = new RichPresenceManager(defaultApplicationId);
		}

		public async Task UseConfig(Config config) {
			if (Rpc?.ApplicationId != (config.ApplicationId ?? DefaultApplicationId)) {
				Rpc = new RichPresenceSetter(config.ApplicationId ?? DefaultApplicationId);
			}

			if (Rpc == null) {
				throw new InvalidOperationException("no default application id set");
			}
			await Rpc.UpdateActivity(config.Activity ?? Activity.DefaultActivity, config.Lobby ?? Lobby.DefaultLobby);
		}
	}
}
