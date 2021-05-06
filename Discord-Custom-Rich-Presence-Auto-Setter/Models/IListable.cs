#region
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	public interface IListable : INotifyPropertyChanged, ICloneable<IListable>, IValuesComparable<IListable> {
		public string Name { get; }
		public Guid Uuid { get; }
	}
}
