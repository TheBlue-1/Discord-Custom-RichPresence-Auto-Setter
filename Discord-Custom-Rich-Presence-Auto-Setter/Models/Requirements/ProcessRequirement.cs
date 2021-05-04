#region
using System.Diagnostics;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class ProcessRequirement : Requirement {
		private string _processName;
		public override bool IsMet => Process.GetProcessesByName(ProcessName).Length > 0;

		public ProcessRequirement(ProcessRequirement processRequirement) {
			ProcessName = processRequirement.ProcessName;
			ShouldBeMet = processRequirement.ShouldBeMet;
		}

		public string ProcessName {
			get => _processName;
			set {
				_processName = value;
				OnPropertyChanged();
			}
		}
	}
}
