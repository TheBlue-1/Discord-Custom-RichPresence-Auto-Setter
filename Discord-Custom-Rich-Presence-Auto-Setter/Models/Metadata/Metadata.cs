#region
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Metadata {
	public class Metadata : INotifyPropertyChanged {
		private string _key = "";
		private string _value = "";
		public string Key {
			get => _key;
			set {
				_key = value;
				OnPropertyChanged();
			}
		}
		public string Value {
			get => _value;
			set {
				_value = value;
				OnPropertyChanged();
			}
		}

		public void Deconstruct(out string key, out string value) {
			key = Key;
			value = Value;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
