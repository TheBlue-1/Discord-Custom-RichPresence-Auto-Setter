namespace Discord_Custom_Rich_Presence_Auto_Setter.Requirements {
	public abstract class Requirement {
		public abstract bool IsMet { get; }
		public bool ShouldBeMet { get; set; }
	}
}
