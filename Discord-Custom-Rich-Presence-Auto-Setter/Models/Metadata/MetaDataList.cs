#region
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Metadata {
	public class MetaDataList : ObservableCollection<Metadata> {
		public MetaDataList(IEnumerable<Metadata> collection) : base(collection) { }

		// ReSharper disable once UnusedMember.Global
		public MetaDataList(IReadOnlyCollection<Metadata> list) : this(list.AsEnumerable()) { }

		public MetaDataList() : this(new List<Metadata>()) { }
		public event PropertyChangedEventHandler InnerPropertyChanged;

		private void InnerPropertyChangedAdder(NotifyCollectionChangedEventArgs e) {
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

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
			base.OnCollectionChanged(e);

			InnerPropertyChangedAdder(e);
		}
	}
}
