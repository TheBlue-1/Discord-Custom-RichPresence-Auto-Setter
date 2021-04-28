#region
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public abstract class ListableBase : IListable {
		private string _name;
		public Guid Uuid { get; }
		public string Name {
			get => _name;
			set {
				_name = value;
				OnPropertyChanged();
			}
		}

		protected ListableBase(string name) {
			Name = name;
			Uuid = Guid.NewGuid();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public abstract IListable Duplicate();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
