#region
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
using Newtonsoft.Json;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public abstract class Requirement : INotifyPropertyChanged, ICloneable<Requirement>, IValuesComparable<Requirement> {
		private bool _shouldBeMet;
		public abstract bool IsMet { get; }
		public bool ShouldBeMet {
			get => _shouldBeMet;
			set {
				_shouldBeMet = value;
				OnPropertyChanged();
			}
		}
		[JsonIgnore]
		public abstract SelectionConverter<RequirementType> Type { get; }

	

		public enum RequirementType {
			Day,
			Process,
			Time
		}


		protected Requirement() { }

		protected Requirement(bool shouldBeMet) => ShouldBeMet = shouldBeMet;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		Requirement ICloneable<Requirement>.Clone() {
			return this switch {
				DayRequirement dayRequirement => ICloneable.Clone(dayRequirement),
				ProcessRequirement processRequirement => ICloneable.Clone(processRequirement),
				TimeRequirement timeRequirement => ICloneable.Clone(timeRequirement),
				_ => throw new NotImplementedException("Unknown RequirementType at Clone")
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;

		bool IValuesComparable<Requirement>.ValuesCompare(Requirement other) {
			return this switch {
				DayRequirement dayRequirement => IValuesComparable.ValuesCompare(dayRequirement, other as DayRequirement),
				ProcessRequirement processRequirement => IValuesComparable.ValuesCompare(processRequirement, other as ProcessRequirement),
				TimeRequirement timeRequirement => IValuesComparable.ValuesCompare(timeRequirement, other as TimeRequirement),
				_ => throw new NotImplementedException("Unknown RequirementType at ValuesCompare")
			};
		}
	}
}
