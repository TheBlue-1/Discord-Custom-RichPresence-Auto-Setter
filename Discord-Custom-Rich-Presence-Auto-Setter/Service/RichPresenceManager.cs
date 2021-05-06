#region
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord_Custom_Rich_Presence_Auto_Setter.Models;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Service {
	public class RichPresenceManager : IDisposable {
		public delegate void CurrentlyUsedConfigChangedHandler(Config config);

		public delegate void ExceptionHandler(Exception exception);

		private Config _currentConfig;
		private RichPresenceSetter _rpc;
		private Task _updater;
		public FileSyncedConfigs Configs { get; } = new();

		private long DefaultApplicationId { get; }

		private RichPresenceSetter Rpc {
			get => _rpc;
			set {
				_rpc?.Dispose();
				_rpc = value;
				if (_rpc != null) {
					_rpc.ExceptionOccurred += e => ExceptionOccurred?.Invoke(e);
				}
			}
		}

		public RichPresenceManager(long defaultApplicationId) => DefaultApplicationId = defaultApplicationId;
		public event CurrentlyUsedConfigChangedHandler CurrentlyUsedConfigChanged;
		public event ExceptionHandler ExceptionOccurred;

		public void Start() {
			if (_updater != null) {
				throw new InvalidOperationException("RP-Manager already running");
			}
			_updater = UpdatePeriodically(App.Settings.RequirementCheckSpan);
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

		public async Task UpdatePeriodically(TimeSpan time) {
			while (true) {
				await Task.Delay(time);
				Update();
			}

			// ReSharper disable once FunctionNeverReturns
		}

		private async Task UseConfig(Config config) {
			if (IValuesComparable.ValuesCompare(config, _currentConfig)) {
				return;
			}

			_currentConfig = ICloneable.Clone(config);

			if (config == null) {
				Rpc = null;
				CurrentlyUsedConfigChanged?.Invoke(null);
				return;
			}
			if (Rpc?.ApplicationId != (config.ApplicationId ?? DefaultApplicationId)) {
				Rpc = new RichPresenceSetter(config.ApplicationId ?? DefaultApplicationId);
			}

			if (Rpc == null) {
				throw new InvalidOperationException("no default application id set");
			}
			await Rpc.UpdateActivity(config.Activity, config.Lobby);
			CurrentlyUsedConfigChanged?.Invoke(config);
		}

		public void Dispose() {
			_rpc?.Dispose();
			_updater?.Dispose();
		}
	}
}
