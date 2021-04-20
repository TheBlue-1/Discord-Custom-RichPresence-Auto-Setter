#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter {
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

			await Rpc.UpdateActivity(config.Activity ?? RichPresenceSetter.DefaultActivity, config.Lobby ?? RichPresenceSetter.DefaultLobby);
		}

		public record Config(RichPresenceSetter.Activity Activity, RichPresenceSetter.Lobby Lobby, long? ApplicationId);
	}
}
