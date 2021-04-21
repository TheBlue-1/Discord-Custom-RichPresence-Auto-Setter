#region
using System;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Requirements {
	public class DayRequirement : Requirement {
		public override bool IsMet {
			get {
				DateTime now = DateTime.Now;
				if (now < MinDate && MinDate != default || now > MaxDate && MinDate != default) {
					return false;
				}

				if (now.Day % 2 == 0) {
					if (DateEquality == NumberEquality.Unequal) {
						return false;
					}
				} else {
					if (DateEquality == NumberEquality.Equal) {
						return false;
					}
				}
				return WeekDay == default || now.DayOfWeek == WeekDay;
			}
		}

		public NumberEquality? DateEquality { get; set; }
		public DateTime? MaxDate { get; set; }
		public DateTime? MinDate { get; set; }

		//isMonday,is equal date usw
		public DayOfWeek? WeekDay { get; set; }

		public enum NumberEquality {
			Equal,
			Unequal
		}
	}
}
