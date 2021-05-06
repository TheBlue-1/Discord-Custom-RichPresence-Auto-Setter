#region
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces;
using ICloneable = Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces.ICloneable;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models {
	public abstract class ListableBase : IListable, ICloneable<ListableBase>, IValuesComparable<ListableBase> {
		private string _name;
		public Guid Uuid { get; }
		public string Name {
			get => _name;
			set {
				_name = value;
				OnPropertyChanged();
			}
		}
		protected ListableBase() { }

		protected ListableBase(string name) {
			Name = name;
			Uuid = Guid.NewGuid();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		IListable ICloneable<IListable>.Clone() => ICloneable.Clone(this);

		ListableBase ICloneable<ListableBase>.Clone() {
			return this switch {
				Lobby lobby => ICloneable.Clone(lobby),
				Activity activity => ICloneable.Clone(activity),
				Config config => ICloneable.Clone(config),
				_ => throw new NotImplementedException("Unknown ListableType at Clone")
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;

		bool IValuesComparable<IListable>.ValuesCompare(IListable other) => IValuesComparable.ValuesCompare(this, other);

		bool IValuesComparable<ListableBase>.ValuesCompare(ListableBase other) {
			return this switch {
				Lobby lobby => IValuesComparable.ValuesCompare(lobby, other as Lobby),
				Activity activity => IValuesComparable.ValuesCompare(activity, other as Activity),
				Config config => IValuesComparable.ValuesCompare(config, other as Config),
				_ => throw new NotImplementedException("Unknown ListableType at ValuesCompare")
			};
		}
	}
}
