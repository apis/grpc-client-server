using System;
using System.Windows.Input;

namespace WpfClientApplication.ViewModel
{
	public class SimpleDelegateCommand : ICommand
	{
		private readonly Action<object> _executeDelegate;
		private readonly Func<object, bool> _canExecuteDelegate;

		public SimpleDelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate)
		{
			_executeDelegate = executeDelegate;
			_canExecuteDelegate = canExecuteDelegate;
		}

		public void Execute(object parameter)
		{
			_executeDelegate(parameter);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecuteDelegate(parameter);
		}

		public event EventHandler CanExecuteChanged;

		public void FireCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}