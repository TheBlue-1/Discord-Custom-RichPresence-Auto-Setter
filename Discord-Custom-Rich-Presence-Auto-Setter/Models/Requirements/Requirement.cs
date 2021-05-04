#region
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public abstract class Requirement : INotifyPropertyChanged,ICloneable<Requirement>,IValuesComparable<Requirement> {
		private bool _shouldBeMet;
		public abstract bool IsMet { get; }
		public bool ShouldBeMet {
			get => _shouldBeMet;
			set {
				_shouldBeMet = value;
				OnPropertyChanged();
			}
		}


		
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;


		public bool ValuesEqual(Requirement other) {
			switch (this)
			{
				case DayRequirement dayRequirement:
					return new DayRequirement(dayRequirement);
					break;
				case ProcessRequirement processRequirement:
					return new ProcessRequirement(processRequirement); break;
				case TimeRequirement timeRequirement:
					return new TimeRequirement(timeRequirement); break;
				default: throw new NotImplementedException("Unknown RequirementType at Clone");
			}
		}


		//protected Requirement Clone() {
		//	switch (this)
		//	{
		//		case DayRequirement dayRequirement:
		//			return dayRequirement.Clone();
					
		//		case ProcessRequirement processRequirement:
		//			return processRequirement.Clone();
		//		case TimeRequirement timeRequirement:
		//			return timeRequirement.Clone(); 
		//		default: throw new NotImplementedException("Unknown RequirementType at Clone");
		//	}
		//}

		public void Test() {
			ICloneable.Clone((Requirement)new TimeRequirement());
			ICloneable.Clone(new TimeRequirement());
		}

		Requirement ICloneable<Requirement>.Clone() {
			return this;
		}
	}
}
