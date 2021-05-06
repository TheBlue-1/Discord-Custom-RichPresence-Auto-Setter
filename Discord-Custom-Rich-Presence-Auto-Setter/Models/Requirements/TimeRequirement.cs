#region
using System;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class TimeRequirement : Requirement, ICloneable<TimeRequirement>, IValuesComparable<TimeRequirement> {
		private DateTime? _endTime;
		private DateTime? _startTime;
		public override bool IsMet {
			get {
				DateTime now = DateTime.Now;
				DateTime time = new(0, 0, 0, now.Hour, now.Minute, now.Second);
				return (time > StartTime || StartTime == default) && (time < EndTime || EndTime == default);
			}
		}
		public DateTime? EndTime {
			get => _endTime;
			set {
				_endTime = value;
				OnPropertyChanged();
			}
		}

		public DateTime? StartTime {
			get => _startTime;
			set {
				_startTime = value;
				OnPropertyChanged();
			}
		}

		public TimeRequirement() { }

		protected TimeRequirement(TimeRequirement timeRequirement) : base(timeRequirement.ShouldBeMet) {
			EndTime = timeRequirement.EndTime;
			StartTime = timeRequirement.StartTime;
		}

		TimeRequirement ICloneable<TimeRequirement>.Clone() => new(this);

		bool IValuesComparable<TimeRequirement>.ValuesCompare(TimeRequirement other) => throw new NotImplementedException();
	}
}
