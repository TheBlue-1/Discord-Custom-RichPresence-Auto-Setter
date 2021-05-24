#region
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces {
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	public interface IListable : INotifyPropertyChanged, ICloneable<IListable>, IValuesComparable<IListable> {
		public string Name { get; }
		public Guid Uuid { get; }
	}
}
