#region
using System;
using System.Linq.Expressions;
using System.Windows.Input;
#endregion

namespace Discord_Custom_Rich_Presence_Auto_Setter.View {
	public class RelayCommand : ICommand {
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;

		public RelayCommand(Action execute, Predicate<object> canExecute) {
			_canExecute = canExecute;
			_execute = obj => execute();
		}

		public RelayCommand(Action execute, Func<bool> canExecute) {
			_canExecute = obj => canExecute();
			_execute = obj => execute();
		}

		public RelayCommand(Action execute) {
			_canExecute = _ => true;
			_execute = obj => execute();
		}

		public RelayCommand(Action<object> execute, Func<bool> canExecute) {
			_canExecute = obj => canExecute();
			_execute = execute;
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
			_canExecute = canExecute;
			_execute = execute;
		}

		public RelayCommand(Action<object> execute) {
			_canExecute = _ => true;
			_execute = execute;
		}

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
