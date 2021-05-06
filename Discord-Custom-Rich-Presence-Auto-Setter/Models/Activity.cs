#region
using System;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using GameSDK.GameSDK;
using Newtonsoft.Json;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public class Activity : ListableBase, ICloneable<Activity>, IValuesComparable<Activity> {
		private ActivityType _activityType;
		private long _applicationId;
		private int _currentPartySize;
		private string _details;
		private long _endTimestamp;
		private bool _instance;
		private string _joinSecret;
		private string _largeImage;
		private string _largeText;
		private string _matchSecret;
		private int _maxPartySize;
		private string _partyId;
		private string _smallImage;
		private string _smallText;
		private string _spectateSecret;
		private long _startTimestamp;
		private string _state;
		public ActivityType ActivityType {
			get => _activityType;
			set {
				_activityType = value;
				OnPropertyChanged();
			}
		}
		public long ApplicationId {
			get => _applicationId;
			set {
				_applicationId = value;
				OnPropertyChanged();
			}
		}
		public int CurrentPartySize {
			get => _currentPartySize;
			set {
				_currentPartySize = value;
				OnPropertyChanged();
			}
		}
		public string Details {
			get => _details;
			set {
				_details = value;
				OnPropertyChanged();
			}
		}
		public long EndTimestamp {
			get => _endTimestamp;
			set {
				_endTimestamp = value;
				OnPropertyChanged();
			}
		}
		public bool Instance {
			get => _instance;
			set {
				_instance = value;
				OnPropertyChanged();
			}
		}
		public string JoinSecret {
			get => _joinSecret;
			set {
				_joinSecret = value;
				OnPropertyChanged();
			}
		}
		public string LargeImage {
			get => _largeImage;
			set {
				_largeImage = value;
				OnPropertyChanged();
			}
		}
		public string LargeText {
			get => _largeText;
			set {
				_largeText = value;
				OnPropertyChanged();
			}
		}
		public string MatchSecret {
			get => _matchSecret;
			set {
				_matchSecret = value;
				OnPropertyChanged();
			}
		}
		public int MaxPartySize {
			get => _maxPartySize;
			set {
				_maxPartySize = value;
				OnPropertyChanged();
			}
		}

		public string PartyId {
			get => _partyId;
			set {
				_partyId = value;
				OnPropertyChanged();
			}
		}
		public string SmallImage {
			get => _smallImage;
			set {
				_smallImage = value;
				OnPropertyChanged();
			}
		}
		public string SmallText {
			get => _smallText;
			set {
				_smallText = value;
				OnPropertyChanged();
			}
		}
		public string SpectateSecret {
			get => _spectateSecret;
			set {
				_spectateSecret = value;
				OnPropertyChanged();
			}
		}
		public long StartTimestamp {
			get => _startTimestamp;
			set {
				_startTimestamp = value;
				OnPropertyChanged();
			}
		}
		public string State {
			get => _state;
			set {
				_state = value;
				OnPropertyChanged();
			}
		}

		[JsonConstructor]
		public Activity() { }

		//public Activity(string name, string state, string details, bool instance, long applicationId, ActivityType activityType, long startTimestamp,
		//	long endTimestamp, string largeImage, string largeText, string smallImage, string smallText, string matchSecret, string joinSecret,
		//	string spectateSecret, string partyId, int maxPartySize, int currentPartySize) : base(name) {
		//	State = state;
		//	Details = details;
		//	Instance = instance;
		//	ApplicationId = applicationId;
		//	ActivityType = activityType;
		//	StartTimestamp = startTimestamp;
		//	EndTimestamp = endTimestamp;
		//	LargeImage = largeImage;
		//	LargeText = largeText;
		//	SmallImage = smallImage;
		//	SmallText = smallText;
		//	MatchSecret = matchSecret;
		//	JoinSecret = joinSecret;
		//	SpectateSecret = spectateSecret;
		//	PartyId = partyId;
		//	MaxPartySize = maxPartySize;
		//	CurrentPartySize = currentPartySize;
		//}
		protected Activity(Activity activity) : base(activity.Name) {
			State = activity.State;
			Details = activity.Details;
			Instance = activity.Instance;
			ApplicationId = activity.ApplicationId;
			ActivityType = activity.ActivityType;
			StartTimestamp = activity.StartTimestamp;
			EndTimestamp = activity.EndTimestamp;
			LargeImage = activity.LargeImage;
			LargeText = activity.LargeText;
			SmallImage = activity.SmallImage;
			SmallText = activity.SmallText;
			MatchSecret = activity.MatchSecret;
			JoinSecret = activity.JoinSecret;
			SpectateSecret = activity.SpectateSecret;
			PartyId = activity.PartyId;
			MaxPartySize = activity.MaxPartySize;
			CurrentPartySize = activity.CurrentPartySize;
		}

		Activity ICloneable<Activity>.Clone() => new(this);

		bool IValuesComparable<Activity>.ValuesCompare(Activity other) => throw new NotImplementedException();
	}
}
