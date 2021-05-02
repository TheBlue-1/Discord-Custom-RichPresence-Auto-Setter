using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements;

namespace Discord_Custom_Rich_Presence_Auto_Setter.View.Elements
{
public    class RequirementView:Grid
    {
		
	

		public static readonly DependencyProperty RequirementProperty =
			DependencyProperty.Register(
				"Requirement", typeof(Requirement), typeof(RequirementView),new PropertyMetadata(new PropertyChangedCallback(RequirementChanged)));

		private static void RequirementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			RequirementView view = (RequirementView)d;
			view.RequirementChanged();
		}

		private void RequirementChanged() {
			switch (Requirement) {
				case TimeRequirement timeRequirement : break;
				case DayRequirement dayRequirement : break;
				case ProcessRequirement processRequirement:break;
			}
		}

		private void SetUpGrid(int rows) {
			Children.Clear();
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			ColumnDefinitions.Add(new ColumnDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < rows; i++) {
				RowDefinitions.Add(new RowDefinition(){Height = new GridLength(30)});

			}
		}

		private void AddAt(UIElement element,int row, int column) {
			SetRow(element,row);
			SetColumn(element,column);
			Children.Add(element);
		}

		private void BindText(TextBox element, string value) {
			var binding = new Binding();
			binding.Source = value;
			element.SetBinding(TextBox.TextProperty, binding);
		}

		private void ShowTimeRequirement(TimeRequirement timeRequirement)
		{SetUpGrid(2);
			Label startTimeLabel = new Label() { Content = "From" };
			Label endTimeLabel = new Label() { Content = "To" };
			TextBox startTime = new TextBox() {};
			TextBox endTime = new TextBox() {};
			AddAt(startTimeLabel,0,0);
			AddAt(startTime,0,1);
			AddAt(endTimeLabel,1,0);
			AddAt(endTime,1,1);
			
		}

		private void ShowDayRequirement(DayRequirement dayRequirement)
		{

		}

		private void ShowProcessRequirement(ProcessRequirement processRequirement)
		{

		}




		public Requirement Requirement
		{
			get { return (Requirement)GetValue(RequirementProperty); }
			set { SetValue(RequirementProperty, value); }

		}

		public RequirementView() {
			
		}


	}
}
