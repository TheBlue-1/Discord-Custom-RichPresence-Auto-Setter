#region
using System;
using System.Linq.Expressions;
using System.Windows.Input;

// ReSharper disable UnusedMember.Global
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	public class RelayCommand : ICommand {
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;

		public RelayCommand(Action execute, Predicate<object> canExecute) : this(_ => execute(), canExecute) { }

		public RelayCommand(Action execute, Func<bool> canExecute) : this(execute, _ => canExecute()) { }

		public RelayCommand(Action execute) : this(_ => execute()) { }

		public RelayCommand(Action<object> execute, Func<bool> canExecute) : this(execute, _ => canExecute()) { }

		public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
			_canExecute = canExecute;
			_execute = execute;
		}

		public RelayCommand(Action<object> execute) : this(execute, _ => true) { }

		public static implicit operator RelayCommand(Expression<Action> expression) => new(expression.Compile());
		public static implicit operator RelayCommand(Expression<Action<object>> expression) => new(expression.Compile());

		public event EventHandler CanExecuteChanged {
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute(object parameter) => _canExecute(parameter);

		public void Execute(object parameter) {
			_execute(parameter);
		}
	}
}
