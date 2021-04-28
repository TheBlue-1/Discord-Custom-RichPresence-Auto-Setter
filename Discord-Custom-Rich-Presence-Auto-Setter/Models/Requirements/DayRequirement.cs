#region
using System;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class DayRequirement : Requirement {
		private NumberEquality? _dateEquality;
		private DateTime? _maxDate;
		private DateTime? _minDate;
		private DayOfWeek? _weekDay;
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

		public NumberEquality? DateEquality {
			get => _dateEquality;
			set {
				_dateEquality = value;
				OnPropertyChanged();
			}
		}
		public DateTime? MaxDate {
			get => _maxDate;
			set {
				_maxDate = value;
				OnPropertyChanged();
			}
		}
		public DateTime? MinDate {
			get => _minDate;
			set {
				_minDate = value;
				OnPropertyChanged();
			}
		}

		//isMonday,is equal date usw
		public DayOfWeek? WeekDay {
			get => _weekDay;
			set {
				_weekDay = value;
				OnPropertyChanged();
			}
		}

		public enum NumberEquality {
			Equal,
			Unequal
		}
	}
}
