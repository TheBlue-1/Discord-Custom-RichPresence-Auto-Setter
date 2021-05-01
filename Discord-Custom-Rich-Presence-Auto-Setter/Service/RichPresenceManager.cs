#region
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Service {
	public sealed class RichPresenceManager {
		private static RichPresenceManager _instance;
		private RichPresenceSetter _rpc;

		public FileSyncedConfigs Configs { get; } = new();

		private long DefaultApplicationId { get; }
		public static RichPresenceManager Instance => _instance ?? throw new InvalidOperationException("RichPresenceManager must be initiated before");

		private RichPresenceSetter Rpc {
			get => _rpc;
			set {
				if (_rpc != null) {
					_rpc.GameLoopEnded -= Update;
					_rpc.Dispose();
				}
				_rpc = value;
				if (_rpc != null) {
					_rpc.GameLoopEnded += Update;
				}
			}
		}

		private RichPresenceManager(long defaultApplicationId) => DefaultApplicationId = defaultApplicationId;

		public static void Init(long defaultApplicationId) {
			if (_instance != null) {
				throw new InvalidOperationException("RichPresenceManager can only be initiated once");
			}
			_instance = new RichPresenceManager(defaultApplicationId);
		}

		private async void Update() {
			foreach (Config config in Configs.Configs) {
				if (config.Requirements.Any(r => r.IsMet != r.ShouldBeMet)) {
					continue;
				}
				await UseConfig(config);
				return;
			}
			await UseConfig(null);
		}

		private async Task UseConfig(Config config) {
			if (config == null) {
				Rpc = null;
				return;
			}
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
