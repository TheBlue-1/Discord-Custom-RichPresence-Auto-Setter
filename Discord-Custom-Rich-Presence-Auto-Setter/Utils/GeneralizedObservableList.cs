#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public sealed class GeneralizedObservableList<T1, T2> : ObservableCollection<T1>, IList<T2>, IList, IReadOnlyList<T2>
		where T2 : T1 {
		public bool IsReadOnly { get; } = false;

		public new T2 this[int index] {
			get => (T2)base[index];
			set => base[index] = value;
		}

		public GeneralizedObservableList() => CollectionChanged += CollectionChangedHandler;

		public event PropertyChangedEventHandler InnerPropertyChanged;

		private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e) {
			if (e.OldItems != null) {
				foreach (INotifyPropertyChanged item in e.OldItems) {
					item.PropertyChanged -= InnerPropertyChangedHandler;
				}
			}
			if (e.NewItems == null) {
				return;
			}

			foreach (INotifyPropertyChanged item in e.NewItems) {
				item.PropertyChanged += InnerPropertyChangedHandler;
			}
		}

		private void InnerPropertyChangedHandler(object sender, PropertyChangedEventArgs e) {
			InnerPropertyChanged?.Invoke(sender, e);
		}

		public void Add(T2 item) {
			base.Add(item);
		}

		public bool Contains(T2 item) => base.Contains(item);

		public void CopyTo(T2[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool Remove(T2 item) => base.Remove(item);
		public int IndexOf(T2 item) => base.IndexOf(item);

		public void Insert(int index, T2 item) {
			base.Insert(index, item);
		}

		public IEnumerator<T2> GetEnumerator() => Items.Cast<T2>().GetEnumerator();
	}
}
