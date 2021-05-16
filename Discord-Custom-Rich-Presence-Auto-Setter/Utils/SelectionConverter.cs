using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils
{
	public class SelectionConverter<T>
	{
		private T _selected;
		public Visibility this[T value] => Equals(value, Selected)?Visibility.Visible:Visibility.Hidden;
		public T Selected
		{
			get => _selected;
			set
			{
				_selected = value;
				SelectedT?.Invoke(this, value);
			}
		}

		public SelectionConverter(T selected) => Selected = selected;

		public event EventHandler<T> SelectedT;
	}
}
