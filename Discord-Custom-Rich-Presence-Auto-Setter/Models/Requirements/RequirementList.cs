#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class RequirementList : ObservableCollection<Requirement> {
		public RequirementList(IEnumerable<Requirement> collection) : base(collection) { }

		// ReSharper disable once UnusedMember.Global
		public RequirementList(IReadOnlyCollection<Requirement> list) : this(list.AsEnumerable()) { }

		public RequirementList() : this(new List<Requirement>()) { }
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
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add : {
					if (e.NewItems != null) {
						foreach (Requirement newItem in e.NewItems) {
							newItem.RequirementTypeChangeRequested += RequirementTypeChangeRequested;
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Remove : {
					if (e.OldItems != null) {
						foreach (Requirement oldItem in e.OldItems) {
							oldItem.RequirementTypeChangeRequested -= RequirementTypeChangeRequested;
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Reset : {
					if (e.OldItems != null) {
						foreach (Requirement oldItem in e.OldItems) {
							oldItem.RequirementTypeChangeRequested -= RequirementTypeChangeRequested;
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Replace : break;
				case NotifyCollectionChangedAction.Move : break;
				default : throw new ArgumentOutOfRangeException();
			}
			InnerPropertyChangedAdder(e);
		}

		private void RequirementTypeChangeRequested(Requirement sender, Requirement.RequirementType? requested) {
			int index = IndexOf(sender);
			RemoveAt(index);
			switch (requested) {
				case null : break;
				case Requirement.RequirementType.Day :

					Insert(index, new DayRequirement());
					break;
				case Requirement.RequirementType.Process :
					Insert(index, new ProcessRequirement());

					break;
				case Requirement.RequirementType.Time :
					Insert(index, new TimeRequirement());

					break;
				default : throw new ArgumentOutOfRangeException(nameof (requested), requested, null);
			}
		}
	}
}
