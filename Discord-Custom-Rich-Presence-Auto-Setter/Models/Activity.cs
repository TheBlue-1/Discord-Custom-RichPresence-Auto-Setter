#region
using GameSDK.GameSDK;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public record Activity(string Name, string State, string Details, bool Instance, long ApplicationId, ActivityType ActivityType, long StartTimestamp,
		long EndTimestamp, string LargeImage, string LargeText, string SmallImage, string SmallText, string MatchSecret, string JoinSecret,
		string SpectateSecret, string PartyId, int MaxPartySize, int CurrentPartySize) : IListable
	{
		public static Activity DefaultActivity => new("Default Activity", "running", "started", true, 0, ActivityType.Playing, 1,
			2, null, null, null, null, null, null,
			null, null, 0, 0);
		public IListable Duplicate() => new Activity(this);
	}
}
