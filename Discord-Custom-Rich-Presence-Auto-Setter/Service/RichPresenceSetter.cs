#region
using System;
using System.Threading.Tasks;
using GameSDK.GameSDK;
using Activity = Discord_Custom_Rich_Presence_Auto_Setter.Models.Activity;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Service {
	public class RichPresenceSetter : IDisposable {
		private readonly Task _updater;
		private ActivityManager ActivityManager => Discord.GetActivityManager();
		public long ApplicationId { get; }

		private Discord Discord { get; }
		private LobbyManager LobbyManager => Discord.GetLobbyManager();

		public RichPresenceSetter(long applicationId) {
			ApplicationId = applicationId;
			Discord = new Discord(ApplicationId, (ulong)CreateFlags.Default);
			ActivityManager.RegisterCommand();

			_updater = UpdatePeriodically(new TimeSpan(0, 0, 0, 0, 500));
		}

		private async Task<Lobby> CreateLobby(Models.Lobby lobby) {
			LobbyTransaction transaction = LobbyManager.GetLobbyCreateTransaction();
			transaction.SetCapacity(lobby.Capacity);
			transaction.SetLocked(lobby.Locked);
			transaction.SetOwner(lobby.OwnerId);

			transaction.SetType(lobby.Type);
			foreach ((string key, string value) in lobby.Metadata) {
				transaction.SetMetadata(key, value);
			}

			TaskCompletionSource<Lobby> tcs = new();
			LobbyManager.CreateLobby(transaction, (Result result, ref Lobby createdLobby) => {
				if (result != Result.Ok) {
					throw new Exception("lobby creation failed");
				}
				tcs.SetResult(createdLobby);
			});
			return await tcs.Task;
		}

		public async Task UpdateActivity(Activity activity = null, Models.Lobby lobby = null) {
			activity ??= Activity.DefaultActivity;
			lobby ??= Models.Lobby.DefaultLobby;
			Lobby dcLobby = await CreateLobby(lobby);

			GameSDK.GameSDK.Activity dcActivity = new() {
				Name = activity.Name,
				State = activity.State,
				Type = activity.ActivityType,
				Instance = activity.Instance,
				Details = activity.Details,
				Timestamps = { Start = activity.StartTimestamp, End = activity.EndTimestamp },
				Assets = { LargeImage = activity.LargeImage, LargeText = activity.LargeText, SmallImage = activity.SmallImage, SmallText = activity.SmallText },
				Party = { Id = dcLobby.Id.ToString(), Size = { CurrentSize = LobbyManager.MemberCount(dcLobby.Id), MaxSize = (int)dcLobby.Capacity } },
				Secrets = { Join = LobbyManager.GetLobbyActivitySecret(dcLobby.Id) }
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
	}
}
