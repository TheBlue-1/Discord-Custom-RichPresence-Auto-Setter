#region
using System;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Requirements {
	public class TimeRequirement : Requirement {
		public override bool IsMet {
			get {
				DateTime now = DateTime.Now;
				DateTime time = new(0, 0, 0, now.Hour, now.Minute, now.Second);
				return (time > StartTime || StartTime == default) && (time < EndTime || EndTime == default);
			}
		}
		public DateTime? EndTime { get; set; }

		public DateTime? StartTime { get; set; }
	}
}
