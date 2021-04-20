﻿#region
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Discord;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter {
	public class RichPresenceSetter {
		private ActivityManager ActivityManager => Discord.GetActivityManager();

		public Activity DefaultActivity { get; } = new("RichPresenceSetter", "running", "started", true, 0, ActivityType.Playing, 1,
			2, null, null, null, null, null, null,
			null, null, 0, 0);
		public Lobby DefaultLobby { get; } = new(2, true, LobbyType.Private, 0, new Dictionary<string, string>());
		private Discord.Discord Discord { get; }= new(418559331265675294, (ulong)CreateFlags.Default);
		private LobbyManager LobbyManager => Discord.GetLobbyManager();

		public RichPresenceSetter() {
		/*	Discord.SetLogHook(LogLevel.Debug, (level, message) =>
			{
				Console.WriteLine("Log[{0}] {1}", level, message);
			});*/
			_ = UpdatePeriodically(new TimeSpan(0, 0, 5));
		}

		private async Task<Discord.Lobby> CreateLobby(Lobby lobby) {
			LobbyTransaction transaction = LobbyManager.GetLobbyCreateTransaction();
			transaction.SetCapacity(lobby.Capacity);
			//transaction.SetLocked(lobby.Locked);
			//transaction.SetOwner(lobby.OwnerId);
		
			transaction.SetType(lobby.Type);
			foreach ((string key, string value) in lobby.Metadata) {
				transaction.SetMetadata(key, value);
			}

			TaskCompletionSource<Discord.Lobby> tcs = new();
			LobbyManager.CreateLobby(transaction, (Result result, ref Discord.Lobby createdLobby) => {
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
			Discord.Lobby dcLobby = await CreateLobby(lobby);

			Discord.Activity dcActivity = new() {
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

		public record Activity(string Name, string State, string Details, bool Instance, long ApplicationId, ActivityType ActivityType, long StartTimestamp,
			long EndTimestamp, string LargeImage, string LargeText, string SmallImage, string SmallText, string MatchSecret, string JoinSecret,
			string SpectateSecret, string PartyId, int MaxPartySize, int CurrentPartySize);

		public record Lobby(uint Capacity, bool Locked, LobbyType Type, long OwnerId, Dictionary<string, string> Metadata);
	}
}
