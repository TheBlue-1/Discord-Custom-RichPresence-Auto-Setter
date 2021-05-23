#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements {
	public class RequirementList : ObservableCollection<Requirement> {
		public RequirementList(IEnumerable<Requirement> collection) : base(collection) { }

		// ReSharper disable once UnusedMember.Global
		public RequirementList(List<Requirement> list) : base(list) { }

		public RequirementList() { }

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
