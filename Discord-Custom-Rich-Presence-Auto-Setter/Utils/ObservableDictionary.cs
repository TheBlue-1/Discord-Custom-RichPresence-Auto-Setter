#region
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Utils {
	public class ObservableDictionary<TKey, TValue> : INotifyCollectionChanged, INotifyPropertyChanged, IDictionary<TKey, TValue> {
		private ObservableCollection<KeyValuePair<TKey, TValue>> Collection { get; } = new();

		public int Count => Collection.Count;

		public ImmutableList<KeyValuePair<TKey, TValue>> ImmutableCollection => Collection.ToImmutableList();
		public bool IsReadOnly => ((IList)Collection).IsReadOnly;
		public ICollection<TKey> Keys => new List<TKey>(Collection.Select(kvp => kvp.Key));
		public ICollection<TValue> Values => new List<TValue>(Collection.Select(kvp => kvp.Value));

		public TValue this[TKey key] {
			get => Collection.First(kvp => kvp.Key.Equals(key)).Value;
			set {
				KeyValuePair<TKey, TValue> val = new(key, value);
				if (ContainsKey(key)) {
					int index = Collection.IndexOf(Collection.First(kvp => kvp.Key.Equals(key)));
					Collection[index] = val;
				}
				Collection.Add(val);
			}
		}

		public ObservableDictionary() { }

		public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) =>
			Collection = new ObservableCollection<KeyValuePair<TKey, TValue>>(collection);

		public void Add(KeyValuePair<TKey, TValue> item) {
			Collection.Add(item);
		}

		public void Clear() {
			Collection.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item) => Collection.Contains(item);

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			Collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item) => Collection.Remove(item);

		public void Add(TKey key, TValue value) {
			Collection.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		public bool ContainsKey(TKey key) => Collection.Any(kvp => kvp.Key.Equals(key));

		public bool Remove(TKey key) => Collection.Remove(Collection.First(kvp => kvp.Key.Equals(key)));

		public bool TryGetValue(TKey key, out TValue value) {
			if (ContainsKey(key)) {
				value = this[key];
				return true;
			}
			value = default;
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Collection.GetEnumerator();

		public event NotifyCollectionChangedEventHandler CollectionChanged {
			add => Collection.CollectionChanged += value;
			remove => Collection.CollectionChanged -= value;
		}
		public event PropertyChangedEventHandler PropertyChanged {
			add => ((INotifyPropertyChanged)Collection).PropertyChanged += value;
			remove => ((INotifyPropertyChanged)Collection).PropertyChanged -= value;
		}
	}
}
