#region
using System.Diagnostics;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Requirements {
	public class ProcessRequirement : Requirement {
		public override bool IsMet => Process.GetProcessesByName(ProcessName).Length > 0;

		public string ProcessName { get; set; }
	}
}
