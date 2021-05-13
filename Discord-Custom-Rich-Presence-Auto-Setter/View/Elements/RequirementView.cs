#region
#endregion

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Discord_Custom_Rich_Presence_Auto_Setter.Models.Requirements;

namespace Discord_Custom_Rich_Presence_Auto_Setter.View.Elements {
	public class RequirementView : Grid {
		public static readonly DependencyProperty RequirementProperty = DependencyProperty.Register("Requirement", typeof (Requirement),
			typeof (RequirementView), new PropertyMetadata(RequirementChanged));

		public Requirement Requirement {
			get => (Requirement)GetValue(RequirementProperty);
			set => SetValue(RequirementProperty, value);
		}

		private void AddAt(UIElement element, int row, int column) {
			SetRow(element, row);
			SetColumn(element, column);
			Children.Add(element);
		}

		private void BindText(FrameworkElement element, string value) {
			Binding binding = new() { Source = value };
			element.SetBinding(TextBox.TextProperty, binding);
		}

		private static void RequirementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			RequirementView view = (RequirementView)d;
			view.RequirementChanged();
		}

		private void RequirementChanged() {
			switch (Requirement) {
				case TimeRequirement timeRequirement : break;
				case DayRequirement dayRequirement : break;
				case ProcessRequirement processRequirement : break;
			}
		}

		private void SetUpGrid(int rows) {
			Children.Clear();
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			ColumnDefinitions.Add(new ColumnDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			for (int i = 0; i < rows; i++) {
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
			}
		}

		private void ShowDayRequirement(DayRequirement dayRequirement) { }

		private void ShowProcessRequirement(ProcessRequirement processRequirement) { }

		private void ShowTimeRequirement(TimeRequirement timeRequirement) {
			SetUpGrid(2);
			Label startTimeLabel = new() { Content = "From" };
			Label endTimeLabel = new() { Content = "To" };
			TextBox startTime = new();
			TextBox endTime = new();
			AddAt(startTimeLabel, 0, 0);
			AddAt(startTime, 0, 1);
			AddAt(endTimeLabel, 1, 0);
			AddAt(endTime, 1, 1);
		}
	}
}
