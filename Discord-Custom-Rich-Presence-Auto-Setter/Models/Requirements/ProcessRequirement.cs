#region
using System.Diagnostics;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using Discord_Custom_Rich_Presence_Auto_Setter.Utils;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class ProcessRequirement : Requirement, ICloneable<ProcessRequirement>, IValuesComparable<ProcessRequirement> {
		private string _processName = "";
		public override bool IsMet => Process.GetProcessesByName(ProcessName).Length > 0;
		protected override RequirementType RType { get; } = RequirementType.Process;

		public string ProcessName {
			get => _processName;
			set {
				_processName = value;
				OnPropertyChanged();
			}
		}

		public ProcessRequirement() { }

		protected ProcessRequirement(ProcessRequirement processRequirement) : base(processRequirement.ShouldBeMet) =>
			ProcessName = processRequirement.ProcessName;

		ProcessRequirement ICloneable<ProcessRequirement>.Clone() => new(this);

		bool IValuesComparable<ProcessRequirement>.ValuesCompare(ProcessRequirement other) {
			if (other.ProcessName != ProcessName) {
				return false;
			}

			return other.ShouldBeMet == ShouldBeMet;
		}
	}
}
