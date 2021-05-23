#region
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
using Discord_Custom_Rich_Presence_Auto_Setter.View;
using Newtonsoft.Json;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public abstract class Requirement : INotifyPropertyChanged, ICloneable<Requirement>, IValuesComparable<Requirement> {
		public delegate void RequirementTypeChangeRequestedHandler(Requirement sender, RequirementType? requested);

		private bool _shouldBeMet;
		public abstract bool IsMet { get; }

		protected abstract RequirementType RType { get; }

		public RelayCommand DeleteRequirement { get; }
		[JsonIgnore]
		public SelectionConverter<RequirementType> Type { get; }
		public bool ShouldBeMet {
			get => _shouldBeMet;
			set {
				_shouldBeMet = value;
				OnPropertyChanged();
			}
		}

		protected Requirement() {
			DeleteRequirement = new RelayCommand(Delete);

			// ReSharper disable once VirtualMemberCallInConstructor
			Type = new SelectionConverter<RequirementType>(RType);
			Type.SelectedT += RequirementTypeChanged;
		}

		protected Requirement(bool shouldBeMet) : this() => ShouldBeMet = shouldBeMet;
		public event RequirementTypeChangeRequestedHandler RequirementTypeChangeRequested;

		private void Delete() {
			RequirementTypeChangeRequested?.Invoke(this, null);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void RequirementTypeChanged(object sender, RequirementType e) {
			if (e == RType) {
				return;
			}
			RequirementTypeChangeRequested?.Invoke(this, e);
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

		public enum RequirementType {
			Day,
			Process,
			Time
		}
	}
}
