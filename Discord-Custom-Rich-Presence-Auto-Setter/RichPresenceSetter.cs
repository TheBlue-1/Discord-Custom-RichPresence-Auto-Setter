#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameSDK.GameSDK;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter {
	public class RichPresenceSetter : IDisposable {
		private readonly Task _updater;
		private ActivityManager ActivityManager => Discord.GetActivityManager();

		public static Activity DefaultActivity => new("RichPresenceSetter", "running", "started", true, 0, ActivityType.Playing, 1,
			2, null, null, null, null, null, null,
			null, null, 0, 0);
		public static Lobby DefaultLobby => new(2, true, LobbyType.Private, 0, new Dictionary<string, string>());
		private Discord Discord { get; } = new(793125319657783306, (ulong)CreateFlags.Default);
		private LobbyManager LobbyManager => Discord.GetLobbyManager();

		public RichPresenceSetter() {
			ActivityManager.RegisterCommand();

			_updater = UpdatePeriodically(new TimeSpan(0, 0, 0, 0, 500));
		}

		private async Task<GameSDK.GameSDK.Lobby> CreateLobby(Lobby lobby) {
			LobbyTransaction transaction = LobbyManager.GetLobbyCreateTransaction();
			transaction.SetCapacity(lobby.Capacity);
			transaction.SetLocked(lobby.Locked);
			transaction.SetOwner(lobby.OwnerId);

			transaction.SetType(lobby.Type);
			foreach ((string key, string value) in lobby.Metadata) {
				transaction.SetMetadata(key, value);
			}

			TaskCompletionSource<GameSDK.GameSDK.Lobby> tcs = new();
			LobbyManager.CreateLobby(transaction, (Result result, ref GameSDK.GameSDK.Lobby createdLobby) => {
				if (result != Result.Ok) {
					throw new Exception("lobby creation failed");
				}
				tcs.SetResult(createdLobby);
			});
			return await tcs.Task;
		}

		public async Task UpdateActivity(Activity activity = null, Lobby lobby = null) {
			activity ??= DefaultActivity;
			lobby ??= DefaultLobby;
			GameSDK.GameSDK.Lobby dcLobby = await CreateLobby(lobby);

			GameSDK.GameSDK.Activity dcActivity = new() {
				Name = activity.Name,
				State = activity.State,
				Type = activity.ActivityType,
				Instance = activity.Instance,
				Details = activity.Details,
				Timestamps = { Start = activity.StartTimestamp, End = activity.EndTimestamp },
				Assets = { LargeImage = activity.LargeImage, LargeText = activity.LargeText, SmallImage = activity.SmallImage, SmallText = activity.SmallText },
				Party = { Id = activity.PartyId, Size = { CurrentSize = activity.CurrentPartySize, MaxSize = activity.MaxPartySize } },
				Secrets = { Join = activity.JoinSecret, Spectate = activity.SpectateSecret, Match = activity.MatchSecret }
			};
			TaskCompletionSource<object> tcs = new();
			ActivityManager.UpdateActivity(dcActivity, result => {
				if (result != Result.Ok) {
					throw new Exception("activity update failed");
				}
				tcs.SetResult(new object());
			});
			await tcs.Task;
		}

		public async Task UpdatePeriodically(TimeSpan time) {
			while (true) {
				await Task.Delay(time);
				Discord.RunCallbacks();
			}

			// ReSharper disable once FunctionNeverReturns
		}

		public void Dispose() {
			_updater.Dispose();
			Discord.Dispose();
		}

		public record Activity(string Name, string State, string Details, bool Instance, long ApplicationId, ActivityType ActivityType, long StartTimestamp,
			long EndTimestamp, string LargeImage, string LargeText, string SmallImage, string SmallText, string MatchSecret, string JoinSecret,
			string SpectateSecret, string PartyId, int MaxPartySize, int CurrentPartySize);

		public record Lobby(uint Capacity, bool Locked, LobbyType Type, long OwnerId, Dictionary<string, string> Metadata);
	}
}
