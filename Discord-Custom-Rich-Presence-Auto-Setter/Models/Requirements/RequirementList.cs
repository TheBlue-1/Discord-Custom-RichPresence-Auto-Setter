using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements
{
  public  class RequirementList : ObservableCollection<Requirement>
    {

		public RequirementList(IEnumerable<Requirement> collection) : base(collection) {
		}

	
		public RequirementList(List<Requirement> list) : base(list)
		{
		}
		public RequirementList() : base() { }

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
			base.OnCollectionChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (Requirement newItem in e.NewItems) {
                    newItem.RequirementTypeChangeRequested += RequirementTypeChangeRequested;
				}
			}else
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (Requirement oldItem in e.OldItems)
				{
					oldItem.RequirementTypeChangeRequested -= RequirementTypeChangeRequested;
				}
			} else 
				
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				foreach (Requirement oldItem in e.OldItems)
				{
					oldItem.RequirementTypeChangeRequested -= RequirementTypeChangeRequested;
				}
			}
			
		}

        private void RequirementTypeChangeRequested(Requirement sender, Requirement.RequirementType? requested) {
			int index= IndexOf(sender);
			RemoveAt(index);
			switch (requested)
			{
				case null:break;
				case Requirement.RequirementType.Day:

					Insert(index, new DayRequirement());
					break;
				case Requirement.RequirementType.Process:
					Insert(index, new ProcessRequirement());

					break;
				case Requirement.RequirementType.Time:
					Insert(index, new TimeRequirement());

					break;

			}
		}
    }
}
