using System;
using System.Windows.Input;

namespace WpfClientApplication.ViewModel
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _action;
		private readonly Func<bool> _func;

		public RelayCommand(Action<object> action, Func<bool> func)
		{
			_action = action;
			_func = func;
		}

		public bool CanExecute(object parameter)
		{
			if (_func == null)
				return true;

			return _func();
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_action(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}