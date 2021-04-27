#region
using System.ComponentModel;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public interface IListable : INotifyPropertyChanged {
		public string Name { get; }

		public IListable Duplicate();
	}
}
